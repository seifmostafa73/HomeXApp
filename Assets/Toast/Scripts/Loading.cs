using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Loading : MonoBehaviour
{

    public static Loading Instance;

    [SerializeField] TMP_Text Loading_Title = null;
    [SerializeField] TMP_Text Loading_Description = null;
    [SerializeField] Animator Loading_anim = null; // Animator for the Rotating Image
    [SerializeField] Animator anim = null;

    [HideInInspector] public static Action CancelbuttonCallBacks;

    float counter = 0f;
    float duration = 0;
    bool IsLoading = false;
    bool IsShown = false;


    public void Show(string Title,string Description, float _duration)
    {
        Loading_Title.text = Title;
        Loading_Description.text = Description;

        duration = _duration;
        counter = 0f;
        Loading_anim.enabled = true;
        if (!IsLoading) IsLoading = true; //Starts the loading process if it's not already started 
    }

    public void Show(string Title, string Description, float _duration, Action CallBack)
    {
        Loading_Title.text = Title;
        Loading_Description.text = Description;

        duration = _duration;
        counter = 0f;
        Loading_anim.enabled = true;
        if (!IsLoading) IsLoading = true; //Starts the loading process if it's not already started 
        CancelbuttonCallBacks = CallBack;
    }


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsLoading)
        {
            if (!IsShown)
            {
                ShowPanel(()=> {
                    IsShown = true; });
            }
            counter += Time.deltaTime;
            if (counter >= duration)
            {  
                HidePanel(()=> {
                    IsShown = false;
                    CancelbuttonCallBacks();
                });
            }
        }
    }

    public void HidePanel()
    {
        Loading_Title.text = "Title";
        Loading_Description.text = "Description";
        counter = 0f;
        IsLoading = false;
        Loading_anim.enabled = false;
        anim.SetBool("IsLoading", false);
        CancelbuttonCallBacks();
        CancelbuttonCallBacks = null;
    }
    public void HidePanel(Action p)
    {
        Loading_Title.text = "Title";
        Loading_Description.text = "Description";
        counter = 0f;
        IsLoading = false;
        Loading_anim.enabled = false;
        anim.SetBool("IsLoading", false);
        p();
        CancelbuttonCallBacks = null;
    }

    private void ShowPanel(Action p)
    {
        anim.SetBool("IsLoading", true);
    }
}
