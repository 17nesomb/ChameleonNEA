
using System.Collections;
using System.Collections.Generic;
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

    
    /// <summary>
    /// This will do make the game playable, by choosing a random card, chameleon and word
    /// </summary>
    public void gameSetup()
    {
        //Choose game card
        int numOfCards = gameScreenManager.cards.Length;
        int cardIndex = Random.Range(0, numOfCards);
            //give number to host to send to all players
        gameManager.sendCardClientRPC(cardIndex);
        //Choose player index to be chameleon
        int numPlayers = gameManager.numOfPlayers();
        //random number from numPlayers
        int chameleonIndex = Random(0, numPlayers);
        ulong chameleonID;
        ulong[] notChameleonIDs = new ulong[numPlayers - 1];
            //give game to host to send the chameleon a trigger

        //Choose random word from card
        int wordIndex = Random.Range(0, 16);
        gameManager.sendWordClientRPC(wordIndex);
            //random number 1,16
            //give number to host to send to all players but the chameleon
    }

    public bool clueGiven = false;
    void clueEntered()
    {

    }

    void onClick()
    {
        //hide button
        gameScreenManager.setCluePanelVisible(false);
        //loop for each client
        foreach(KeyValuePair<ulong,string> player in gameManager.playerDict)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] {player.Key}
                }
            };

            //show clue input panel
            gameManager.turnStartClientRPC(clientRpcParams); 
            //wait for response
            
            //write response somewhere

        }


    }
}
