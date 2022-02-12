using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

public class RoomAddButtonFunctionality : MonoBehaviour
{
    [SerializeField] Transform LocalDeviceSpawn;
    void Start()
    {
        UImanager uImanager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
        int Roomnumber = GetComponentInParent<OpenRoomScript>().roomattribute;
        LeanButton ConfirmButton = GameObject.FindGameObjectWithTag("DeviceConfirm").GetComponent<LeanButton>();
        LeanWindow EditMenu = GameObject.FindGameObjectWithTag("DeViceEditMenu").GetComponent<LeanWindow>();
        Button button = gameObject.GetComponent<Button>();

        button.onClick.AddListener(delegate {
            EditMenu.TurnOn();
            ConfirmButton.OnClick.RemoveAllListeners();
           // ConfirmButton.OnClick.AddListener(delegate { uImanager.CreateDevice(LocalDeviceSpawn, Roomnumber); });
        }) ;
    }
}
