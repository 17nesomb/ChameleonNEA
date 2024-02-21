
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    GameManager gameManager;
    GameScreenManager gameScreenManager; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    ulong[] turnOrder;
    
    /// <summary>
    /// This will do make the game playable, by choosing a random card, chameleon and word
    /// </summary>
    public void gameSetup()
    {
        gameScreenManager.setStartBtnVisible(false);

        //Chooses random card
        int numOfCards = gameScreenManager.cards.Length; 
        int cardIndex = Random.Range(0, numOfCards);
        gameManager.sendCardClientRPC(cardIndex);

        //Chooses who the chameleon is and created an array for every player that isnt the chameleon
        int numPlayers = gameManager.playerDict.Count;
        int chameleonIndex = Random.Range(0, numPlayers);
        ulong[] playerIdArray = gameManager.playerDict.Keys.ToArray();
        ulong chameleonID = playerIdArray[chameleonIndex];
        ulong[] notChameleonIDs = new ulong[numPlayers - 1];

        //Populates the array of players that arent the chameleon
        int count = 0;
        foreach(ulong id in playerIdArray)
        { 
            if(id != chameleonID)
            {
                notChameleonIDs[count] = id;
                count++;
            }
        }

        //Creates the paramaters to ensure that only the chameleon gets told they're the chameleon
        ClientRpcParams chameleonRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { chameleonID }
            }
        };

        //Lets the chameleon know theyre the chameleon
        gameManager.sendChameleonClientRPC(chameleonRpcParams);

        //Creates the parametes to ensure that the chameleone doesn't get the secret word
        ClientRpcParams secretWordRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = notChameleonIDs
            }
        };

        //Chooses the secret word and sends it to all players but the chameleon
        int wordIndex = Random.Range(0, 16);
        gameManager.sendWordClientRPC(wordIndex, secretWordRpcParams);

        //Chooses the first player, and creates an array of player IDs in order of the turns
        int firstPlayerIndex = Random.Range(0, numPlayers);

        ulong[] clientIDs = gameManager.playerDict.Keys.ToArray();
        turnOrder = new ulong[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            turnOrder[(i + (numPlayers - firstPlayerIndex)) % numPlayers] = clientIDs[i];
        }

        nextTurn(null);
    }

    /// <summary>
    /// Starts the turn for the next player, given the last player who inputted their clue
    /// </summary>
    /// <param name="latestPlayerID"></param>
    public void nextTurn(ulong? latestPlayerID)
    {
        int currentPlayerIndex = 0;

        //If that was the last turn
        if (latestPlayerID == turnOrder[turnOrder.Length - 1])
        {
            gameManager.startVoteStageClientRPC();
        }
        else
        {
            //if given a latest player
            if (latestPlayerID != null) { }
            {
                //finds the index of the latest player
                for (int i = 0; i < turnOrder.Length; i++)
                {
                    if (turnOrder[i] == latestPlayerID)
                    {
                        //sets the currentPlayerIndex to the one after that
                        currentPlayerIndex = i + 1;
                    }
                }
            }



            ClientRpcParams turnStartRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { turnOrder[currentPlayerIndex] }
                }
            };

            gameManager.turnStartClientRPC(turnStartRpcParams);

        }

    }
}
