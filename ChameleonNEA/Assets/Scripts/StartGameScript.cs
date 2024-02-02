using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
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
    void gameSetup()
    {

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
        foreach(KeyValuePair<string,ulong> player in gameManager.playerDict)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] {player.Value}
                }
            };

            //show clue input panel
            gameManager.turnStartClientRPC(clientRpcParams); 
            //wait for response
            
            //write response somewhere

        }


    }
}
