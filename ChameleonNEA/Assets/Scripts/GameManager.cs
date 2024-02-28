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
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    GameScreenManager gameScreenManager;
    UsernameInputScript usernameInputScript;
    StartGameScript startGameScript;
    VoteScreenScript voteScreenScript;
    public Dictionary<ulong, string> playerDict = new Dictionary<ulong, string>();

    void Awake()
    {
        usernameInputScript = GameObject.Find("UsernameInp").GetComponent<UsernameInputScript>();
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        startGameScript = GameObject.Find("StartGameBtn").GetComponent<StartGameScript>();
        voteScreenScript = GameObject.Find("VoteScreenPnl").GetComponent<VoteScreenScript>();
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

    [ClientRpc]
    public void sendWordClientRPC(int wordIndex, ClientRpcParams clientRpcParams = default)
    {
        gameScreenManager.showSecretWord(wordIndex);
    }

    [ClientRpc]
    public void sendChameleonClientRPC(ClientRpcParams clientRpcParams = default)
    {
        gameScreenManager.showChameleon();
    }

    [ClientRpc]
    public void turnStartClientRPC(ClientRpcParams clientRpcParams)
    {
        gameScreenManager.setCluePanelVisible(true);
    }

    [ClientRpc]
    public void sendClueClientRPC(string senderName, string clue)
    {
        Debug.Log(senderName + clue);
        playerClues.Add(senderName, clue);
        voteScreenScript.addClueInfo(senderName, clue);
    }

    [SerializeField] GameObject voteScreen;
    [ClientRpc]
    public void startVoteStageClientRPC()
    {
        voteScreenScript.organisePositions(playerClues.Count);
        gameScreenManager.showScreen(voteScreen);
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

    Dictionary<string, string> playerClues = new Dictionary<string, string>();
    [ServerRpc(RequireOwnership = false)]
    public void sendClueServerRPC(string clue, ServerRpcParams serverRpcParams = default)
    {
        ulong senderID = serverRpcParams.Receive.SenderClientId;
        string senderName = playerDict[senderID];
        Debug.Log(senderName + clue);
        sendClueClientRPC(senderName, clue);
        //Show clue panel on next clients screen
        startGameScript.nextTurn(senderID);
        
        
    }
}
