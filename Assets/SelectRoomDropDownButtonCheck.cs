using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectRoomDropDownButtonCheck : MonoBehaviour
{
    [SerializeField] Button NextButton;
    
    void Start()
    {
        CheckbuttonState();
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate{ CheckbuttonState(); });
    }

    void CheckbuttonState()
    {
        NextButton.interactable = (GetComponent<TMP_Dropdown>().value !=0);
    }
}
