using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JoinCodeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] TMP_InputField codeInputBox;
    [SerializeField] TextMeshProUGUI errorBox;

    public string getInputtedCode()
    {
        return (codeInputBox.text);
    }
    
    /// <summary>
    /// Used to valid the join code
    /// </summary>
    /// <returns></returns>
    public bool isValidCode()
    {
        if (codeInputBox.text.Length != 6 || !(codeInputBox.text.All(char.IsLetterOrDigit))) //https://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
        {
            errorBox.text = "Please enter a valid Join Code";  
            return (false);
        }
        else
        {
            errorBox.text = (""); //clears the error box
            return true;
        }
    }
}
