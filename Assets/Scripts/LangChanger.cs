using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LangChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_InputField NickName = null;
    [SerializeField] TMP_Dropdown Lang = null;
    string[] Languages = { "en-US", "ar-EG" };
    public void SendLangCode() 
    {
        if (NickName.text != null) { PlayerPrefs.SetString("NickName", NickName.text); }
        VoiceCommandController.ChangeLang(Languages[Lang.value]);
        VoiceCommandController.Setup();
    }
}
