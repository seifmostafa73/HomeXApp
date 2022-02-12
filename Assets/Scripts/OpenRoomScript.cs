using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class OpenRoomScript : MonoBehaviour
{
    UImanager Uimanager = null;
    public byte roomattribute = 0;
    public int NumberOfDevices = 0;
    GameObject Device = null;

    private void Awake()
    {
        Uimanager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
        Device = GameObject.FindGameObjectWithTag("Devices");
    }
    private void Start()
    {
        NumberOfDevices = Uimanager.AllDeviceData.Where(s => s.RoomNumber == roomattribute).Count();
    }

    public void OpenRoomFunction()
    {
        NumberOfDevices = Uimanager.AllDeviceData.Where(s => s.RoomNumber == roomattribute).Count();
        if (NumberOfDevices > 0)
        {
            var RoomTitles= GameObject.FindGameObjectsWithTag("RoomTitleUI");
            foreach(GameObject Title in RoomTitles)
            {
                Title.GetComponentInChildren<TMPro.TMP_Text>().text = GetComponentInChildren<TMPro.TMP_Text>().text;
            }
            Device.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.25f);
            Uimanager.InitializeAllDevices(roomattribute);
        }
        else
        {
            Toast.Instance.Show("Add a device to the room first!", .5f, Toast.ToastColor.Blue);
        }
    }

}
