using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiveClueScript : MonoBehaviour
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

    private void Awake()
    {
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    bool validClue(string clue)
    {
        if(clue.Length <= 30)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [SerializeField] TMP_InputField clueInputField;

    public void giveClueOnClick()
    {
        string clue = clueInputField.text;
        if (validClue(clue)) 
        {
            gameManager.sendClueServerRPC(clue);
            gameScreenManager.setCluePanelVisible(false);
        }
    }
}
