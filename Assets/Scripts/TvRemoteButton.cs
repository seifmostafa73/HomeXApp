using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TvRemoteButton : MonoBehaviour
{
    [SerializeField] Dropdown RemoteSelector;

    [SerializeField] string SendCode = null;
    [SerializeField] string RemoteTopic = null;
    [SerializeField] int ButtonOrder = 0;
    public bool hasFunctionBeenSet = false;
    [SerializeField] bool IsExcuteFunction = true;
    mqttTest MQTT = null;

    private void Start()
    {
        MQTT = GameObject.FindGameObjectWithTag("Manager").GetComponent<mqttTest>();
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { if (IsExcuteFunction) { RemoteButtonFunction(); } });
        RemoteSelector.onValueChanged.AddListener(delegate { SelectorListCheckValue(); });
        SelectorListCheckValue();
    }

    void SelectorListCheckValue() 
    {
        SendCode = PlayerPrefs.GetString("RemoteData" + ButtonOrder + RemoteSelector.value, "");
        RemoteTopic = (RemoteSelector.options.Count !=0 )?  RemoteSelector.options[RemoteSelector.value].text : "";
        hasFunctionBeenSet = !(SendCode == "");
    }

    void RemoteButtonFunction()
    {
        hasFunctionBeenSet = !(SendCode== "");
        if (hasFunctionBeenSet)
        {
            //Send it to the Remote topic in mqtt
            MQTT.PublishButton(SendCode, RemoteTopic);
            gameObject.GetComponent<AudioSource>().Play();
            Toast.Instance.Show("Remote Signal Sent", 2f, Toast.ToastColor.Blue);
        }
        else 
        {
            //Send the flag to the same topic and wait for mssg
            StopAllCoroutines();
            MQTT.RecievedMssgList.Clear();
            Debug.Log("WaitingForCode");
            StartCoroutine("CheckForCode");
            Loading.Instance.Show("⏲ Waiting for signal", "Click on the Desired Remote Signal",5f,
                ()=> { StopCoroutine("CheckForCode"); });
        }
    }

    public void OnHoldRemoteButton()
    {
        IsExcuteFunction = false;
        StartCoroutine(Hold());
    }

    IEnumerator Hold()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (IsExcuteFunction == false)
        {
            Toast.Instance.Show("Remote Signal cleared");
            SendCode = "";
        }
    }

    public void OnReleaseRemoreButton()
    {
        IsExcuteFunction = true;
    }



    IEnumerator CheckForCode()
    {
        MQTT.PublishButton("A", RemoteTopic);
        float duration = 5f;
        float normalizedTime = 0;

        while (MQTT.RecievedMssgList.Count ==0 || normalizedTime >= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return new WaitForSeconds(1f);
        }
        SendCode = MQTT.RecievedMssgList[MQTT.RecievedMssgList.Count - 1].Mssg;
        Loading.Instance.HidePanel();
        MQTT.Unsubscribe(RemoteTopic);
        PopUp.Instance.Show("✔️ Signal Found", "Click Confirm to save the signal or Try to try it first", 20f,
            ()=> { SAVERSIGNAL(SendCode);PopUp.Instance.HidePanel(); },
            () => { Debug.Log("PopUpCanceled");SendCode = ""; },
            () => { MQTT.PublishButton(SendCode, RemoteTopic); },
            "Try Signal");
    }
    void SAVERSIGNAL(string IRCode) 
    {
        PlayerPrefs.SetString("RemoteData" + ButtonOrder + RemoteSelector.value, IRCode);
        PlayerPrefs.Save();
        Toast.Instance.Show("Code Added", .2f, Toast.ToastColor.Blue);
    }

   
}
