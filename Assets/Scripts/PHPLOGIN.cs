using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Lean.Gui;
using Michsky.UI.ModernUIPack;
public class PHPLOGIN : MonoBehaviour
{
    public LeanButton LOginButton;

    public TMP_InputField UserNAme;
    public TMP_InputField Password;
    [SerializeField]bool RememberMe;

    public GameObject LoadingScreen;
    public RadialSlider slider;
    public TMP_Text percent;

    private void Start()
    {
        RememberMe = PlayerPrefs.GetInt("RememberMe", 0) == 1;
        if (RememberMe && PlayerPrefs.GetString("Name","NULL")!="NULL") 
        {
            StartCoroutine(LoadAsycLevel(3));
        }
    }

    public void Toggle() 
    {
        RememberMe = !RememberMe;
    }

    public void Login()
    {
        bool IsInteractable = (UserNAme.text.Length >= 8 && Password.text.Length >= 8);
       
       if(IsInteractable)
       {
            StartCoroutine(StartLogin());
       }else
       {
           Toast.Instance.Show("ID and password must be larger than 8 characters long",3f,Toast.ToastColor.Magenta);
       }    
    }

    IEnumerator StartLogin()
    {
        WWWForm USerForm = new WWWForm();
        USerForm.AddField("Name", UserNAme.text);
        USerForm.AddField("Password", Password.text);
        UnityWebRequest PHP = UnityWebRequest.Post("https://homexbolyka.000webhostapp.com/UsersLogin.php", USerForm);
        yield return PHP.SendWebRequest();
        if (PHP.result == UnityWebRequest.Result.ProtocolError)
        {
            Toast.Instance.Show(PHP.error);
        }
        Toast.Instance.Show(PHP.downloadHandler.text.Split('\t')[0] );
        yield return new WaitForSeconds(1f);
        string ErrorText = PHP.downloadHandler.text.Split('\t')[1];
        if (ErrorText == "#NoERRORS")
        {
            if (RememberMe)
            {
                PlayerPrefs.SetInt("RememberMe", 1);
            }
            else 
            {
                PlayerPrefs.SetInt("RememberMe", 0);
            }
            PlayerPrefs.SetString("Name", UserNAme.text);
            StartCoroutine(LoadAsycLevel(3));
        }
    }

    public IEnumerator LoadAsycLevel(int index)
    {
        AsyncOperation AOperation = SceneManager.LoadSceneAsync(index);
        LoadingScreen.SetActive(true);


        while (!AOperation.isDone) {

            float loadingProgress = Mathf.Clamp01(AOperation.progress/.9f);

            slider.currentValue = loadingProgress;
            percent.text = loadingProgress * 100f + "%";

            yield return null;
        }

    }

    public void GotoRegister() 
    {
        SceneManager.LoadSceneAsync(2);
    }
}
