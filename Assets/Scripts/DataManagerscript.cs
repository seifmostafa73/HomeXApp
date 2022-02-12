using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManagerscript : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Loggedin",0)==1)
        {
            StartCoroutine(WaitBeforeLoad());
        }
    }

    string FullName ="";
    [SerializeField] TextMeshProUGUI ErrorsField = null;
    [SerializeField] TMP_InputField NameField = null;

    public void Loadscene() 
    {
        if (NameField.text == ""||NameField.text == null||NameField.text.Length<=2)
        {
            ErrorsField.text = "* Please Re-enter your name";
        }
        else
        {
            FullName = NameField.text;
            PlayerPrefs.SetString("Name", FullName);
            PlayerPrefs.SetInt("Loggedin", 1);
            StartCoroutine(WaitBeforeLoad());
        }
    }

    IEnumerator WaitBeforeLoad ()
    {
        ErrorsField.color = Color.green;
        ErrorsField.text = "Welcome Back, " + (NameField.text.Split(' '))[0];
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
}
