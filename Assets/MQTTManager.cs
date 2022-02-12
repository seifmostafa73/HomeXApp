using System.Collections;
using System.Collections.Generic;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine;

public class MQTTManager : MonoBehaviour
{
    MqttClient MQClient;
    [Space(20)][Header("MQTT Broker Info")]
    [SerializeField] string BrokerIP;
    [SerializeField] int BrokerPort;
    [SerializeField] bool IsbrokerSecured;
    [SerializeField] string MainTopic = "CoffeMachine";

    void Start()
    {
        MQClient = new MqttClient(BrokerIP, BrokerPort, IsbrokerSecured, null);
        // register to message received 
        MQClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived; //this is called whenever a client publishes something

        MQClient.MqttMsgSubscribed += client_MqttMsgSubscribed; //this is called whenever a client subscribes

        MQClient.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;  // thisis called whenever a client unsubscribes
        string clientId = Guid.NewGuid().ToString();
        MQClient.Connect(clientId, "cQbhdHrh6E", "Dc8ZrNcPPB");
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string RecievedMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + RecievedMessage);
    }
    void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
    {

    }

    void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
    }

    void PublishButton(string MSSG, string TopicName)
    {
        // string History = HistoryText.text;
        string MssgSend = MSSG;
        if (TopicName != null)
        {
            StartCoroutine(PublishB(TopicName, MSSG)); 
        }
        Debug.Log("sent");
    }


    void Unsubscribe(string TopicName)
    {
        MQClient.Unsubscribe(new string[] { TopicName });
    }

    public void SendMQTTMEssage(string Message) 
    {
        PublishB(MainTopic,Message);
    }

        IEnumerator PublishB(string Topic,string Message) 
    {
        yield return new WaitForSeconds(0.2f);
        MQClient.Publish(Topic, System.Text.Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
    }
}
