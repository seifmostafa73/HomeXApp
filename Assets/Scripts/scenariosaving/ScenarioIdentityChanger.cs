using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Lean.Gui;
 

public class ScenarioIdentityChanger : MonoBehaviour,IPointerExitHandler,IPointerDownHandler
{
    [HideInInspector]public int scenarioindex = 0;
    public string ScenarioName = "";
    public Sprite ChangeableICon = null;
    public int[] Function;//holds Action ->OFF=1, ON=0, TOGGLE=2  , and channel and device [ext ON-1-0 == open second device in product number 0]
    [HideInInspector] public string[] Channel = null;
    [HideInInspector] public int[] Device = null;

    public bool OnHomeActivation = false;
    [HideInInspector] public bool OnExitActivation = false;

    [SerializeField] Image Icon = null;
    [SerializeField] TMP_Text Name = null;
    Button FunctionalityButton = null;

    colorthemeController themeController = null;

    //MQTT
    mqttTest Mqtt = null;
    UImanager UIMANAGER = null;
    //Lean Panel
    bool IsDown = false;
    LeanWindow DeletePanel = null;
    float Timer=0;

    enum MssgState {S=0,T,F };
    private void Awake()
    {
        Mqtt = GameObject.FindGameObjectWithTag("Manager").GetComponent<mqttTest>();
        UIMANAGER = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
        FunctionalityButton = gameObject.GetComponent<Button>();
    }

    public void Sendtopublish()
    {
        for (int index =0;index < Function.Length; index++) 
        {
            var AllChanelinfo = Channel[index].Split('>');
            string ChannelIndex = "0";
            var DeviceData = UIMANAGER.AllDeviceData.Find(x => x.DeviceName == AllChanelinfo[0]);
            string ProductName = DeviceData.ProductName;
            for (int i =0; i< DeviceData.ToggleNames.Length;i++) 
            {
                if (DeviceData.ToggleNames[i] == AllChanelinfo[1]) 
                {
                    ChannelIndex = i.ToString(); 
                }
            }
            MssgState Command = (MssgState)Function[index];
            string MQTTmssg = ChannelIndex + Command.ToString();
            Debug.Log(MQTTmssg);
            FunctionalityButton.onClick.AddListener(delegate { Mqtt.PublishButton(MQTTmssg, ProductName); Mqtt.Unsubscribe(ProductName); });
            
        }

    }


    private void UIMANAGER_OnHomeStateChange()
    {
        if ((UIMANAGER.IsUserInHome && OnHomeActivation) || (!UIMANAGER.IsUserInHome && OnExitActivation))
        {
            FunctionalityButton.onClick.Invoke();
            Debug.Log("Sending Scenario");
        }
    }

    private void FixedUpdate()
    {
        if (IsDown)
        {
            HoldingCheck(Timer, 1f);
        }  
    }

   void HoldingCheck(float timer, float TimeLimit)
    {
        if (Time.time - timer >= TimeLimit)
        {
            DeletePanel.TurnOn();
            OnUp();
            var DeleteScript = DeletePanel.GetComponent<ScenarioDeletation>();
            DeleteScript.IndexOfScenario = scenarioindex;
            DeleteScript.ScenarioToDelete = this.gameObject;
            Timer = 0f;
        }
    }

    public void OnDown()
    {
        IsDown = true;
        Timer = Time.time;  //start time
    }
    public void OnUp()
    {
        IsDown = false;
        Timer = 0f;
    }
 

    
    void Start()
    {
        DeletePanel = GameObject.FindGameObjectWithTag("DeleteScenarioPanel").GetComponent<LeanWindow>();
        themeController = GameObject.FindGameObjectWithTag("Manager").GetComponent<colorthemeController>();
        Icon.sprite = ChangeableICon;
        Icon.color = colorthemeController.CurrentTheme.ExtraColor;
        themeController.ExtraObjects.Add(Icon);
        Name.text = ScenarioName;

        if (OnHomeActivation || OnExitActivation)
            UIMANAGER.OnHomeStateChange += UIMANAGER_OnHomeStateChange;

        Sendtopublish();

    }

    private void OnDestroy()
    {
        themeController.ExtraObjects.Remove(Icon);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown();
    }

}
