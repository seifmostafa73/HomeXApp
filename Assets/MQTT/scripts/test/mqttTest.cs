 using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using TMPro;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
public class mqttTest : MonoBehaviour
{
    public MqttClient client;
    [Header("Broker info")]
    public string RemoteTopicName;
    public string BrokerIP;
    public int BrokerPort;
    [SerializeField] bool IsbrokerSecured = false;

    public TextAsset certificate;

    [System.Serializable] public struct StateClass
    {
        public List<char> ProductStateList;
    }
    [Header("States Data")]
    public List<StateClass> AllStateData = new List<StateClass>();

    public static string directory = "/SavedStates";
    public static string FileName = "States.txt";
    string direct;
    
    [System.Serializable]
    public struct RecievedMssg
    {
        public string Mssg;
        public string topic;
        public RecievedMssg(string RMssg,string STopic)
        {
            Mssg = RMssg;
            topic = STopic;
        }
    }
    public List<RecievedMssg> RecievedMssgList = new List<RecievedMssg>();

    public static mqttTest MQTT;
    #region ActualStates

    public void Save_States_inJASON(List<StateClass> Object)
    {


        if (!Directory.Exists(direct)) { Directory.CreateDirectory(direct); }
        string JSONDATA = null;
        if (Object != null)
        {
            for (int i = 0; i < Object.Count; i++)
            {
                if (i + 1 != Object.Count)
                {
                    JSONDATA += JsonUtility.ToJson(Object[i]) + ",";
                }
                else
                {
                    JSONDATA += JsonUtility.ToJson(Object[i]);
                }
            }
            Debug.Log(JSONDATA);
            File.WriteAllText(direct + FileName, "[" + JSONDATA + "]");
        }

    }

    public static List<StateClass> Load_States_Data()
    {
        string path = Application.persistentDataPath + directory + FileName;
        if (File.Exists(path))
        {
            string jasonData = File.ReadAllText(path);
            if (JsonHelper.GetArray<List<char>>(jasonData) != null)
            {
                StateClass[] Savedarray = JsonHelper.GetArray<StateClass>(jasonData);
                List<StateClass> SavedObject = new List<StateClass>(Savedarray);
                return SavedObject;
            }
            else
            {
                File.WriteAllText(path, "{}");
                return null;
            }
        }
        else
        {
            Debug.LogError("JSON FILE WAS NOT CREATED");
            Debug.LogError(path);
            File.WriteAllText(path, "{}");
            Load_States_Data();
            return null;
        }

    }

    #endregion

    void Start()
    {
        MQTT = this;
        direct = Application.persistentDataPath + directory;


        if (Load_States_Data() == null)
        {
            AllStateData.Add(new StateClass());
        }
        else { AllStateData = Load_States_Data(); }
        if (AllStateData != null && AllStateData[0].ProductStateList != null)
        { Debug.Log("Count" + (char)(AllStateData[0].ProductStateList.Count + 48)); }



        // create client instance 
        client = new MqttClient(BrokerIP, BrokerPort, IsbrokerSecured, null);
        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; //this is called whenever a client publishes something

        client.MqttMsgSubscribed += client_MqttMsgSubscribed; //this is called whenever a client subscribes

        client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;  // thisis called whenever a client unsubscribes
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId, "cQbhdHrh6E", "Dc8ZrNcPPB");
    }


    public void RestartMQTTConnection(string TopicName)
    {
        if (TopicName != null)
        {
            client.Subscribe(new string[] { TopicName + "Out" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }); //array of strings and QOS is to
        }          
        //subscribe to multiple topics in one time
         // other functions -->   client.Unsubscribe()
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {


        string RecievedMessage = System.Text.Encoding.UTF8.GetString(e.Message);

        Debug.Log("Received: " + RecievedMessage);
        var NewMessage = new RecievedMssg(RecievedMessage,e.Topic);
        RecievedMssgList.Add(NewMessage);
      
        Save_States_inJASON(AllStateData);


    }

    void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
    {

    }

    void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
    }

    public void PublishButton(string MSSG, string TopicName)
    {
        // string History = HistoryText.text;
        string MssgSend = MSSG;
        if (TopicName != null)
        {
            StartCoroutine(PublishB(TopicName, MSSG)); 
        }
        Debug.Log("sent");
    }

    IEnumerator PublishB(string Topic,string Message) 
    {
        RestartMQTTConnection(Topic);
        yield return new WaitForSeconds(0.2f);
        client.Publish(Topic + "In", System.Text.Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
    }

    public void Unsubscribe(string TopicName)
    {
        client.Unsubscribe(new string[] { TopicName + "Out" });
    }

}
