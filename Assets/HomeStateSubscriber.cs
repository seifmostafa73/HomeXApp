using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeStateSubscriber : MonoBehaviour
{
    [SerializeField]List<UImanager.DeviceData> LockInDevices = null;
    void Start()
    {
        UImanager.UIMANAGER.OnHomeStateChange += ChangeDeviceState;
    }

    private void ChangeDeviceState()
    {
        foreach (UImanager.DeviceData Device in LockInDevices)
        {
            for(int index =0;index <Device.States.Length ;index++)
            mqttTest.MQTT.PublishButton(index+"F", Device.ProductName);
        }
    }

}