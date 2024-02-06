using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    GameScreenManager gameScreenManager;
    UsernameInputScript usernameInputScript;
    StartGameScript startGameScript;
    public Dictionary<ulong, string> playerDict = new Dictionary<ulong, string>();
    enum currentScreen
    {
        GameScreen,
        VoteScreen,
        LeaderboardScreen
    }

    void Awake()
    {
        usernameInputScript = GameObject.Find("UsernameInp").GetComponent<UsernameInputScript>();
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        startGameScript = GameObject.Find("StartGameBtn").GetComponent<StartGameScript>();
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn) //If the user doesn't already have a sign in cached
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //Get a playerId
        }
        Debug.Log(AuthenticationService.Instance.PlayerId);

        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) => //Subscribes the anonymous function to the OnClientConnected Action
        {

            if (NetworkManager.Singleton.IsHost)
            {
                requestUsernameClientRPC(clientId); //Runs requestUsernameClientRPC on all of the client's GameManagers
                foreach (string username in playerDict.Values)
                {
                    addPlayerClientRPC(username);
                }
            }
        };

        NetworkManager.Singleton.OnServerStarted += () => //Subscribes the anonymous function to the OnServerStarted Action
        {
            if (NetworkManager.Singleton.IsHost)
            {
                ulong hostId = 0;
                requestUsernameClientRPC(hostId); //Requests the username from the host, this can't go in the client connected as the host isn't a client
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            removePlayerClientRPC(playerDict[clientId]);
            playerDict.Remove(clientId);
        };





    }

    // Update is called once per frame
    void Update()
    {

    }



    //For the host-------------------------------------------------------------------------------------------------------------
    public async void StartHostWithRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7); //Creates an allocation on the relay server with a max of seven clients
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); //creates a variable to hold the server data. "dtls" is the communication protocol
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); //gets the joinCode of the allocation
        NetworkManager.Singleton.StartHost();
        Debug.Log(joinCode);
        gameScreenManager.showJoinCode(joinCode);
    }

    [SerializeField] GameObject gameScreen;
    [ClientRpc]
    void requestUsernameClientRPC(ulong clientId)
    {

        if (NetworkManager.Singleton.LocalClientId == clientId)  //Checks that the computer the RPC is running on is the right client
        {
            string username = usernameInputScript.getUsername(); //gets the username
            sendUsernameServerRPC(username); //runs the sendUsernameServerRPC on the host computer
        }
        gameScreenManager.showScreen(gameScreen);

    }

    [ClientRpc]
    void addPlayerClientRPC(string username)
    {
        gameScreenManager.addPlayerToList(username);
    }

    [ClientRpc]
    void removePlayerClientRPC(string username)
    {
        gameScreenManager.removePlayerFromList(username);
    }

    [ClientRpc]

    public void sendCardClientRPC(int cardIndex)
    {
        gameScreenManager.showGameCard(cardIndex);
    }

    /// <summary>
    /// Shows the clue input panel on the client's screen
    /// </summary>
    /// <param name="clientRpcParams"> only sends to the clientID specified by the params </param>
    [ClientRpc]
    public void turnStartClientRPC(ClientRpcParams clientRpcParams)
    {
        gameScreenManager.setCluePanelVisible(true);
    }

    //For the clients-----------------------------------------------------------------------------------------------------
    public async void JoinGameWithRelay(string joinCode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode); //Joins the allocation with the joinCode
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); //creates a variable to hold the server data. "dtls" is the communication protocol
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); 
        NetworkManager.Singleton.StartClient();
    }

    [ServerRpc(RequireOwnership = false)]
    void sendUsernameServerRPC(string username, ServerRpcParams serverRpcParams = default)
    {
        ulong senderId = serverRpcParams.Receive.SenderClientId; //Gets the id of the client that sent their username
        if (playerDict.ContainsValue(username)) //If there is already a client with that name
        {
            username = username += ("1");
        }

        Debug.Log(username + " --> " + senderId.ToString()); 
        playerDict.Add(senderId, username);
        addPlayerClientRPC(username);
    }

    [ServerRpc(RequireOwnership =false)]
    void sendClueServerRPC(string clue)
    {

    }
}
