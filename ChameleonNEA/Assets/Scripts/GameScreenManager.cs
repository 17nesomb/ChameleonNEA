using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject GameScreen;
    [SerializeField] GameObject VoteScreen;


    // Start is called before the first frame update
    void Start()
    {
        showScreen(TitleScreen);
        cluePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void showScreen(GameObject screen)
    {
        Debug.Log("Showing " + screen.ToString());
        TitleScreen.SetActive(false);
        GameScreen.SetActive(false);
        VoteScreen.SetActive(false);
        screen.SetActive(true);
    }

    [SerializeField] TextMeshProUGUI[] usernameList;

    /// <summary>
    /// Adds the username given to the list of usernames
    /// </summary>
    /// <param name="playerName"></param>
    public void addPlayerToList(string playerName)
    {
        foreach (TextMeshProUGUI textBox in usernameList)
        {
            if (textBox.text == "" || textBox.text == playerName)
            {
                textBox.text = playerName;
                break;
            }
        }
    }

    public void removePlayerFromList(string playerName)
    {
        string[] playerNames = new string[8];
        int count = 0;
        foreach (TextMeshProUGUI textBox in usernameList) //loops through each of the text boxes
        {
            if(textBox.text != playerName) //If the name isnt being removed
            {
                playerNames[count] = textBox.text; //add it to the list and increment count
                count++; 
            }
        }

        for(int i = 0; i < 8;)
        {
            usernameList[i].text = playerNames[i];
        }
        
    }

    [SerializeField] TextMeshProUGUI joinCodeTextBox;
    public void showJoinCode(string joinCode)
    {
        joinCodeTextBox.text = (joinCode);
    }

    [SerializeField] Button startGameBtn;
    public void setStartBtnVisible(bool visible)
    {
        startGameBtn.gameObject.SetActive(visible);
    }

    [SerializeField] GameObject cluePanel;
    public void setCluePanelVisible(bool visible)
    {
        cluePanel.SetActive(visible);
    }

    [SerializeField] TextMeshProUGUI themeTMP;
    [SerializeField] TextMeshProUGUI[] wordListTMPs;
    [SerializeField] public Card[] cards;
    Card currentCard;
    public void showGameCard(int cardIndex)
    {
        currentCard = cards[cardIndex];
        themeTMP.text = currentCard.theme;
        for(int i = 0; i < 16; i++)
        {
            wordListTMPs[i].text = currentCard.words[i];
        }
    }

    [SerializeField] TextMeshProUGUI secretWordTMP;
    public void showSecretWord(int wordIndex)
    {
        word = currentCard.words[wordIndex];
        secretWordTMP.text = word;
    }

    public void showChameleon()
    {
        secretWordTMP.text = ("You are the chameleon - Blend in!");
    }
}


