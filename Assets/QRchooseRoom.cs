using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QRchooseRoom : MonoBehaviour
{
    public string QRCODESTRING="";
    [SerializeField] TMP_InputField DeviceName = null;
    [SerializeField] TMP_Dropdown RoomNumberDropDown = null;
    [SerializeField] InpageSwiper PageManager=null;
    UImanager uimanager = null;
    private void Start()
    {
        uimanager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
        int roomsnumber = (byte)PlayerPrefs.GetInt("Created Rooms");
        List<string> OptionStrings = new List<string>();
        for (int i =1; roomsnumber>=i;i++)
        {
            string Name = PlayerPrefs.GetString("Room Name" + i, "null");
            OptionStrings.Add(Name);
        }
        RoomNumberDropDown.AddOptions(OptionStrings);
    }

    public void CheckForRemote()
    {
        if (QRCODESTRING == "") { return; }
        int ignorechar = QRCODESTRING[1] - 48;
        int Difference = QRCODESTRING[0] - 48;
        string DProductName = QRCODESTRING.Substring(ignorechar + 2, QRCODESTRING.Length - (2 + ignorechar));

        int[] TempArray = new int[DProductName.Length];

        for (int i = 0; i < DProductName.Length; i++)
        {
            TempArray[i] = (int)DProductName[i] - Difference;
        }
        DProductName = null;
        for (int i = 0; i < TempArray.Length; i++)
        {
            DProductName += (char)TempArray[i];
        }

        if (DProductName[0] == 'R')
        {
            AddRemote(DProductName);
        } 
    }

    void AddRemote(string Topic)
    {
        int i = 0;
        while (true)
        {
            if (PlayerPrefs.GetString("Remote" + i, "") == "")
            {
                PlayerPrefs.SetString("Remote" + i, Topic);
                PageManager.CancelButton();
                Debug.Log("Created Remote number" +i);
                break;
            }
            else { i++; }
        }
    }

    public void CreatDeviceInfo()
    {
        if (QRCODESTRING==null) { return; }
        int Difference = QRCODESTRING[0] - 48;
        int ignorechar = QRCODESTRING[1] -48;
        string DProductName = QRCODESTRING.Substring(ignorechar+2, QRCODESTRING.Length-(2+ ignorechar));
        Debug.Log("Decribteeed  : "+DProductName);
        int[] TempArray = new int[DProductName.Length];

        for (int i=0;i<DProductName.Length ;i++) 
        {
            TempArray[i] = (int)DProductName[i] - Difference;
        }
        DProductName = null;
        for (int i = 0; i < TempArray.Length; i++)
        {
            DProductName += (char)TempArray[i];
        }


        string newDevicename = null;
        if (DeviceName == null)
        {
            newDevicename = QRCODESTRING;
        }
        else
        {
            newDevicename = DeviceName.text;
        }
        int newDeviceType = 0;
        bool[] States = new bool[4];
        string[] Tnames = { "T1", "T2", "T3", "T4" };
        bool[] Favourite = { false,false,false,false };
        int Roomnumber = RoomNumberDropDown.value;
        var NewDevice = new UImanager.DeviceData(newDevicename,newDeviceType, Favourite, States,Tnames, Roomnumber,DProductName);
        uimanager.AllDeviceData.Add(NewDevice);
        UImanager.Save_Devices_inJASON(uimanager.AllDeviceData);
    }
}
