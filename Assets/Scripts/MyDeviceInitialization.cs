using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDeviceInitialization : MonoBehaviour
{
    UImanager uImanager;
    private void Start()
    {
        uImanager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
       // uImanager.initializeAllDevices();
    }
}

