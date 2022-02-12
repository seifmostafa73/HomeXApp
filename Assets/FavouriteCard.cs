using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
public class FavouriteCard : MonoBehaviour
{
    public string RoomName = "";
    public string CardName = "";
    public string ProductName = "";
    public bool State = false;
    public int Index = 0;

    [SerializeField] TMP_Text CardNameText = null;
    [SerializeField] TMP_Text RoomNameText = null;
    public Image Cap = null;
    [SerializeField] Color OnColor = new Color(0, 0, 0, 0);
    [SerializeField] Color OffColor = new Color(0, 0, 0, 0);
    mqttTest MQTT = null;
    private bool previousState = false;


    void Start()
    {
        MQTT = GameObject.FindGameObjectWithTag("Manager").GetComponent<mqttTest>();
        UpdateText();
        previousState = State;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { SendMqttMessage(); });
        MQTT.RestartMQTTConnection(ProductName);
        MQTT.client.MqttMsgPublishReceived += CheckState;
        ChangeGraphics();

    }
    public void UpdateText()
    {
        CardNameText.text = CardName;
        RoomNameText.text = RoomName;
    }

    void SendMqttMessage()
    {
        State =! State;
        string Message = State ? Index+"T":Index+"F"; 
        MQTT.PublishButton(Message, ProductName);
        State = !State;

    }

    void CheckState(object sender, MqttMsgPublishEventArgs e)
    {
        if (e.Topic == ProductName+"Out") {
            string RecievedMessage = System.Text.Encoding.UTF8.GetString(e.Message);
            if (RecievedMessage[0] == (char)(Index + 48))
            {
                State = (RecievedMessage[1] == 'T') ? true : false;
                UImanager.UIMANAGER.AllDeviceData.Find(x => x.ProductName == ProductName).States[Index] = State;
            }
        }
    }
    private void FixedUpdate()
    {
        if (State != previousState)
        {
            previousState = State;
            ChangeGraphics();
        }
    }
    void ChangeGraphics()
    {
        Color ToColor = State ?OnColor:OffColor;
        StartCoroutine(Changecolor(Cap, ToColor, 1f));
    }

    public IEnumerator Changecolor(Image image, Color color, float time)
    {
        float ElapsedTime = 0.0f;
        float TotalTime = time;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            image.color = Color.Lerp(image.color, color, (ElapsedTime / TotalTime));
            yield return null;
        }
    }
}
