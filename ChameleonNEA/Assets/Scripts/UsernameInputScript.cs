using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameInputScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] TMP_InputField usernameInput;

    /// <summary>
    /// limits the length of the username input
    /// </summary>
    public void limitLength()
    {
        if (usernameInput.text.Length > 16) //if the username input length is more than 16 characters
        {
            usernameInput.text = usernameInput.text.Substring(0, 16); //sets the text in the box to the first 16 characters
        }
    }

    public string getUsername()
    {
        return usernameInput.text;
    }

    [SerializeField] TextMeshProUGUI errorBox;
    public bool isValid()
    {
        if(usernameInput.text == "") //if text box is empty
        {
            errorBox.text = ("Please input a username");
            return false;
        }
        else
        {
            errorBox.text = (""); //removes the error box
            return true;
        }

        
    }
    
}
