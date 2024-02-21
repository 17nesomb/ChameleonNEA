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

    
    public void removePlayerFromList(string leavingPlayerName)
    {
        List<string> playersInGame = new List<string>();
        foreach (TextMeshProUGUI playerName in usernameList)
        {
            playersInGame.Add(playerName.text);
        }

        playersInGame.Remove(leavingPlayerName);
        playersInGame.Add("");

        int n = 0;

        foreach (TextMeshProUGUI playerName in usernameList)
        {
            playerName.text = playersInGame[n];
            n++;
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
        string word = currentCard.words[wordIndex];
        secretWordTMP.text = word;
    }

    public void showChameleon()
    {
        secretWordTMP.text = ("You are the chameleon - Blend in!");
    }
}


