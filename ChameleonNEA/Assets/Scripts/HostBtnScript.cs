using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostBtnScript : MonoBehaviour
{
    GameScreenManager gameScreenManager;
    UsernameInputScript usernameInputScript;
    GameManager gameManager;

    private void Awake()
    {
        gameScreenManager = GameObject.Find("GameScreenManager").GetComponent<GameScreenManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        usernameInputScript = GameObject.Find("UsernameInp").GetComponent<UsernameInputScript>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] GameObject gameScreen;
    /// <summary>
    /// Executes when the host game button is clicked
    /// </summary>
    public void onClick()
    {
        if (usernameInputScript.isValid())
        {
            gameScreenManager.showScreen(gameScreen);
            string username = usernameInputScript.getUsername();
            gameScreenManager.addPlayerToList(username);
            gameManager.StartHostWithRelay();
            gameScreenManager.setStartBtnVisible(true);
        }
    }
}
