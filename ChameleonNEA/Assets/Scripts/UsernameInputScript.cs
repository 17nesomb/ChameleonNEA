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
        if (usernameInput.text.Length > 16)
        {
            usernameInput.text = usernameInput.text.Substring(0, 16);
        }
    }

    public string getUsername()
    {
        return usernameInput.text;
    }

    [SerializeField] TextMeshProUGUI errorBox;
    public bool isValid()
    {
        if(usernameInput.text == "")
        {
            errorBox.text = ("Please input a username");
            return false;
        }
        else
        {
            errorBox.text = ("");
            return true;
        }

        
    }
    
}
