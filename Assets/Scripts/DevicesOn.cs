using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicesOn : MonoBehaviour
{
    [SerializeField] Animation DeviceAnimator = null;
    [SerializeField] Animation RoomAnimator = null;
    [SerializeField] Transform DevicesParent = null;

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeviceBackButton();
            }
     }

    public void DeviceBackButton()
    {
        var DevicesMenu = GameObject.FindGameObjectWithTag("Devices");
        DeviceIdentityChanger[] Children = DevicesParent.GetComponentsInChildren<DeviceIdentityChanger>();
        foreach (DeviceIdentityChanger child in Children)
        {
            Destroy(child.gameObject);
        }
        RoomOn();
    }
    public void DeviceOpen() 
    {
        RoomAnimator.gameObject.SetActive(false);
        DeviceAnimator.gameObject.SetActive(true);
        DeviceAnimator.Play();

    }
   public void RoomOn()
    {
        DeviceAnimator.gameObject.SetActive(false);
        RoomAnimator.gameObject.SetActive(true);
        RoomAnimator.Play();
    }
}
