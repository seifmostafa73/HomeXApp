using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Gui;

public class VC_Settings : MonoBehaviour,IPointerExitHandler
{
    private bool IsDown = false;
    [SerializeField]LeanWindow VC_EditMenu=null;
    float Timer = 0;

    public void OnDown()
    {
        IsDown = true;
        Timer = Time.time;  //start time
    }

    public void OnUp()
    {
        IsDown = false;
        Timer = 0f;
    }

    void HoldingCheck(float timer, float TimeLimit)
    {

        if (Time.time - timer >= TimeLimit)
        {
            VC_EditMenu.TurnOn();
            OnUp();
            Timer = 0f;
        }
    }

    public void onClick()
    {
        if (Time.time - Timer < 2f) { Debug.Log("Listening"); VoiceCommandController.StartListening(); }
        OnUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDown)
        {
            HoldingCheck(Timer, 2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUp();
    }
}
