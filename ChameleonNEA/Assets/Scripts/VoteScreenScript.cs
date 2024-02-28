using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VoteScreenScript : MonoBehaviour
{
    GameManager gameManager;
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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    [SerializeField] GameObject[] votePanels;
    [SerializeField] GameObject confirmVotePnl;

    public void organisePositions(int numPlayers)
    {
        confirmVotePnl.SetActive(false);

        for (int i = numPlayers; i < votePanels.Length - 1; i++)
        {
            votePanels[i].gameObject.SetActive(false);
        }

        List<Vector2> positions = calculatePositions(numPlayers);
        int count = 0;
        foreach (GameObject panel in votePanels)
        {
            if(count < numPlayers)
            {
                panel.SetActive(true);
                panel.transform.position = positions[count];
            }
            else
            {
                panel.gameObject.SetActive(false);
            }
            count++;
        }

    }

    List<Vector2> calculatePositions(int numPlayers) 
    {
        List<Vector2> positions = new List<Vector2>();
        int angle = 360 / numPlayers; //angle in degrees
        int horizontalRadius = Screen.width / 3; // radii are one third of the screen
        int verticalRadius = Screen.height / 3;
        for (int i = 0; i < numPlayers; i++)
        {
            positions.Add(calcLocationFromAngle(i * angle, horizontalRadius, verticalRadius));
            Debug.Log("Inside calcPositions Loop");
        }

        return positions;
    }

    private Vector2 calcLocationFromAngle(int angle, int horizontalRadius, int verticalRadius)
    {
        Vector2 centre = new Vector2(Screen.width/2, Screen.height/2);

        double angleRad = angle * Math.PI / 180; //converts the angle to radians so that cos() and sin() will work
        float yPos = (float)Math.Cos(angleRad) * verticalRadius; //uses trigonometry to find the y coord
        float xPos = (float)Math.Sin(angleRad) * horizontalRadius; //uses trigonometry to find the x coord (doubled so its an elipse)
        return centre + new Vector2(xPos, yPos);
    }

    int votesFilled = 0;
    public void addClueInfo(string username, string clue)
    {
        TextMeshProUGUI[] playerTMPs = votePanels[votesFilled].GetComponentsInChildren<TextMeshProUGUI>();
        playerTMPs[0].text = username;
        playerTMPs[1].text = clue;
        votesFilled++;
    }

    public void voteButtonClick(TextMeshProUGUI namePanel)
    {
        confirmVotePnl.SetActive(true);
        TextMeshProUGUI confirmLabel = confirmVotePnl.GetComponent<TextMeshProUGUI>();
        string playerName = namePanel.text;
        confirmLabel.text = ("Confirm vote for " + playerName);
    }
}
