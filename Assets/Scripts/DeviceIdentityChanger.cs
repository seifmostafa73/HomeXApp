using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Gui;
public class DeviceIdentityChanger : MonoBehaviour
{
    [SerializeField] Transform togglesGridParent = null;
    [SerializeField] GameObject newToggle = null;
    public TMP_Text NameUI = null;
    public Image IconUI = null;
    [HideInInspector]   public string ProductName = "";
    [HideInInspector]   public int NoOfPlugs = 0;
    [HideInInspector]   public string[] ToggleNames = null;
    [HideInInspector]   public bool[] State = { };
    [HideInInspector]   public bool[] Favourites = { };

    mqttTest Mqtt;


    private void Start()
    {
        Mqtt = GameObject.FindGameObjectWithTag("Manager").GetComponent<mqttTest>();
        for (int plugindex = 0; plugindex < NoOfPlugs; plugindex++)
        {
            int TempValue = plugindex;
            GameObject AddedPlug = Instantiate(newToggle, togglesGridParent.transform);
            Button UIButtonlement = AddedPlug.GetComponent<Button>();
            Image UIImageElement = AddedPlug.GetComponent<Image>();
            var CurrentThemePallette = colorthemeController.CurrentTheme;

            AddedPlug.GetComponentInChildren<TMP_Text>().text = ToggleNames[plugindex];
            AddedPlug.GetComponentInChildren<FavouritesButton>().Index = plugindex;
            UIImageElement.color = State[plugindex]? CurrentThemePallette.ExtraColor :CurrentThemePallette.FrontColor;
            UIButtonlement.onClick.AddListener(delegate { SendDeviceMqttMssg(TempValue, UIImageElement); });
        }
    }


    public void SendDeviceMqttMssg(int Channel, Image imagetoggle)
    {
            State[Channel] = !State[Channel];
            string MssgToSend = $"{Channel}{State[Channel].ToString()[0]}";
            Debug.Log("Sending to " + ProductName + "In" + MssgToSend);
            Mqtt.PublishButton(MssgToSend, ProductName);
            var CurrentThemePallette = colorthemeController.CurrentTheme;
            imagetoggle.color = State[Channel] ? CurrentThemePallette.ExtraColor : CurrentThemePallette.FrontColor;
    }

}
