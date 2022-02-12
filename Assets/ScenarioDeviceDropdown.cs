using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScenarioDeviceDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown DevicesDropDown;
    UImanager UIMANAGER = null;
    void Start()
    {
        UIMANAGER = GameObject.FindGameObjectWithTag("Manager").GetComponent<UImanager>();
        StartCoroutine(UpdateList());
    }

    IEnumerator UpdateList() 
    {
        DevicesDropDown.options.Clear();
        List<string> OptionStrings = new List<string>();
        foreach (var Device in UIMANAGER.AllDeviceData)
        {
            foreach (var Plug in Device.ToggleNames)
            {
                OptionStrings.Add(Device.DeviceName +">"+ Plug);

            }
        }
        DevicesDropDown.AddOptions(OptionStrings);
        yield return new WaitForSeconds(3f);
    }

}
