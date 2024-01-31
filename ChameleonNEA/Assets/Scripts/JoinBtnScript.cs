using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinBtnScript : MonoBehaviour
{
    JoinCodeScript joinCodeScript;
    GameScreenManager gameScreenManager;
    UsernameInputScript usernameInputScript;
    GameManager gameManager;

    private void Awake()
    {
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        joinCodeScript = GameObject.Find("JoinCodeInp").GetComponent<JoinCodeScript>();
        usernameInputScript = GameObject.Find("UsernameInp").GetComponent<UsernameInputScript>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Executes when the join button is clicked
    /// </summary>
    public void onClick()
    {
        if (joinCodeScript.isValidCode() && usernameInputScript.isValid())
        {
            string username = usernameInputScript.getUsername();
            string joinCode = joinCodeScript.getInputtedCode();
            gameManager.JoinGameWithRelay(joinCode);
            gameScreenManager.setStartBtnVisible(false);
        }

        
    }
}
