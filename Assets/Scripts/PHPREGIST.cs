using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Lean.Gui;
public class PHPREGIST : MonoBehaviour
{
    public LeanButton REgisterButton;

    public TMP_InputField UserNAme;
    public TMP_InputField Password;

    public void Register()
    {
        bool IsInteractable = (UserNAme.text.Length >= 8 && Password.text.Length >= 8);
       
       if(IsInteractable)
       {
        StartCoroutine(StartRegister());
       }else
       {
           Toast.Instance.Show("ID and password must be larger than 8 characters long",3f,Toast.ToastColor.Magenta);
       } 
    }

    IEnumerator StartRegister()
    {
        WWWForm USerForm = new WWWForm();
        USerForm.AddField("Name", UserNAme.text);
        USerForm.AddField("Password", Password.text);
         UnityWebRequest PHP = UnityWebRequest.Post("https://homexbolyka.000webhostapp.com/UsersRegister.php", USerForm);
        yield return PHP.SendWebRequest();
        if (PHP.result == UnityWebRequest.Result.ProtocolError)
        {
           Toast.Instance.Show(PHP.error);
        }
        Toast.Instance.Show(PHP.downloadHandler.text.Split('\t')[0]);
        string ErrorText = PHP.downloadHandler.text.Split('\t')[1];
        if (ErrorText == "#NoERRORS")
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(1);
    }

}
