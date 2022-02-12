using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;
using TMPro;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField]public static string LANG_Code = "en-US";
    [SerializeField]
    ScenarioAddingScript ScenarioScript;
    mqttTest Mqtt;

    #region toSpeech
    public void StartSpeaking(string TalkingWords) 
    {
        TextToSpeech.instance.StartSpeak(TalkingWords);
    }

    public void StopSpeaking() 
    {
        TextToSpeech.instance.StopSpeak();
    }
    #endregion

    #region ToText

    public static void StartListening() 
    {
        SpeechToText.instance.StartRecording();
        Toast.Instance.Show(LANG_Code);
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }
    public void OnFinalSpeechResult(string FinalResult) 
    {
        SearchVC(FinalResult);
        Toast.Instance.Show(FinalResult);
    }
    public void OnPartialSpeechResult(string FinalResult)
    {
        SearchVC(FinalResult);
    }
    public static void ChangeLang(string Lang)
    {
        LANG_Code = Lang;
    }

    #endregion
    public static void Setup() 
    {
        TextToSpeech.instance.Setting(LANG_Code,0.89f,0.8f);
        SpeechToText.instance.Setting(LANG_Code);
    }
    enum MssgState { T = 0, F, S };

    void SearchVC(string VCString)
    {

        if (VCString == "hey" || VCString == "hi")
        {
            string UName = PlayerPrefs.GetString("NickName", "Buddy");
            StartSpeaking("Hey" + UName);
        }
        else {
            var Scenarios = ScenarioScript.SO;
            if (Scenarios == null)
            {
                Toast.Instance.Show("Secanrios not found");
            }
            else
            {
                foreach (Scenariosavingscript.DATA Scenario in Scenarios)
                {
                    if (Scenario.ScenarioName == VCString)
                    {
                        Debug.Log(VCString);
                        var Function = Scenario.Function;
                        Debug.Log("Starting");
                        for (int index = 0; index < Function.Length; index++)
                        {
                            string ChannelIndex = Scenario.Channel[index].ToString();
                            MssgState Command = (MssgState)Function[index];
                            string MQTTmssg = ChannelIndex + Command.ToString();
                            Debug.Log(MQTTmssg);
                           // Mqtt.PublishButton(MQTTmssg);
                        }
                        StartSpeaking("Scenario Is Called");
                    }
                    else
                    {
                        StartSpeaking("Sorry Couldn't Find that scenario");
                    }
                }
            }
        }
    }

    private void Start()
    {
        GameObject UIMANAGER = GameObject.FindGameObjectWithTag("Manager");
        ScenarioScript = UIMANAGER.GetComponent<ScenarioAddingScript>();
        Mqtt = UIMANAGER.GetComponent<mqttTest>();
        Setup();
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;

        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        SpeechToText.instance.onReadyForSpeechCallback = OnFinalSpeechResult;

#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onDoneCallback = StopSpeaking;
    }
}
