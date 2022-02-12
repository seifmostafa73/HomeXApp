using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FavouritesButton : MonoBehaviour
{
    public int Index = 0;
    [SerializeField] DeviceIdentityChanger PlugParent = null;
    [SerializeField] Image StarImage = null;
    [SerializeField] Sprite[] StarIcons = null;
    private void Start()
    {
        PlugParent = GetComponentInParent<DeviceIdentityChanger>();
        TMPro.TMP_Text RoomTextUI = GameObject.FindGameObjectWithTag("RoomTitleUI").GetComponent<TMPro.TMP_Text>();

        if (UImanager.UIMANAGER.AllDeviceData.Exists(x => (x.DeviceName == PlugParent.NameUI.text && x.ProductName == PlugParent.ProductName)))
        {
            int I = UImanager.UIMANAGER.AllDeviceData.FindIndex(v => v.DeviceName == PlugParent.NameUI.text && v.ProductName == PlugParent.ProductName);
            StarImage.sprite = StarIcons[(UImanager.UIMANAGER.AllDeviceData[I].Favourtie[Index]) ? 1 : 0];
        }

    }
    public void OnFavourtiePress()
    {
        TMPro.TMP_Text RoomTextUI = GameObject.FindGameObjectWithTag("RoomTitleUI").GetComponent<TMPro.TMP_Text>();
        int Roomindex = 0;
        for (int i = 1; i <= PlayerPrefs.GetInt("Created Rooms", 0) && PlayerPrefs.GetInt("Created Rooms", 0) != 0; i++)
        {
            string Name = PlayerPrefs.GetString("Room Name" + i, "null");
            if (Name == RoomTextUI.text) Roomindex = i;
        }
        
        if (UImanager.UIMANAGER.AllDeviceData.Exists(x => (x.DeviceName == PlugParent.NameUI.text && x.ProductName == PlugParent.ProductName ) ) ) 
        {
            int I = UImanager.UIMANAGER.AllDeviceData.FindIndex(v => v.DeviceName == PlugParent.NameUI.text&&v.ProductName == PlugParent.ProductName );

            UImanager.UIMANAGER.AllDeviceData[I].Favourtie[Index] = !UImanager.UIMANAGER.AllDeviceData[I].Favourtie[Index];

            var Toaststring = (UImanager.UIMANAGER.AllDeviceData[I].Favourtie[Index]) ? "Item Added to favourites" : "Item Removed from favourites";

            Toast.Instance.Show(Toaststring, 2f, Toast.ToastColor.Blue);
            StarImage.sprite = StarIcons[(UImanager.UIMANAGER.AllDeviceData[I].Favourtie[Index])? 1:0];
            UImanager.Save_Devices_inJASON(UImanager.UIMANAGER.AllDeviceData);
            UImanager.UIMANAGER.InvokeFavouriteList();

            return;
        }
    }

}
