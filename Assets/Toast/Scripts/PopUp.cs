using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopUp : MonoBehaviour
{

    public static PopUp Instance;

    [SerializeField] TMP_Text PopUp_Title = null;
    [SerializeField] TMP_Text PopUp_Description = null;
    [SerializeField] Animator anim = null;

    [HideInInspector] public static Action CancelbuttonCallBacks = null;
    [HideInInspector] public static Action ConfirmbuttonCallBacks = null;
    [HideInInspector] public static Action ExtralbuttonCallBacks=null;
    [SerializeField] Button ExtraButton = null;



    float counter = 0f;
    float duration = 0;
    bool IsLoading = false;
    bool IsShown = false;


/*    public void Show(string Title,string Description, float _duration)
    {
        PopUp_Title.text = Title;
        PopUp_Description.text = Description;

        duration = _duration;
        counter = 0f;
        if (!IsLoading) IsLoading = true; //Starts the loading process if it's not already started 
    }
*/

    public void Show(string Title, string Description, float _duration, Action ConfirmCallBack,Action CancelCallBack)
    {
        PopUp_Title.text = Title;
        PopUp_Description.text = Description;

        duration = _duration;
        counter = 0f;
        if (!IsLoading) IsLoading = true; //Starts the loading process if it's not already started 
        ConfirmbuttonCallBacks = ConfirmCallBack;
        CancelbuttonCallBacks = CancelCallBack;
        ExtraButton.gameObject.SetActive(false);

    }

    public void Show(string Title, string Description, float _duration, Action ConfirmCallBack, Action CancelCallBack,Action ExtraButtonCallBack,string ExtraButtonText)
    {
        PopUp_Title.text = Title;
        PopUp_Description.text = Description;

        duration = _duration;
        counter = 0f;
        if (!IsLoading) IsLoading = true; //Starts the loading process if it's not already started 
        ConfirmbuttonCallBacks = ConfirmCallBack;
        CancelbuttonCallBacks = CancelCallBack;
        ExtralbuttonCallBacks = ExtraButtonCallBack;
        ExtraButton.gameObject.SetActive(true);
        ExtraButton.GetComponentInChildren<TMP_Text>().text = ExtraButtonText;
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

    public void ExtraFunction()
    {
        ExtralbuttonCallBacks();
    }
    public void ExtraFunction(Action P)
    {
        ExtralbuttonCallBacks();
        P();
    }

    public void confirmFunction()
    {
        ConfirmbuttonCallBacks();
    }

    public void HidePanel()
    {
        PopUp_Title.text = "Title";
        PopUp_Description.text = "Description";
        counter = 0f;
        IsLoading = false;
        anim.SetBool("FadeIn", false);
        CancelbuttonCallBacks();

        CancelbuttonCallBacks = null;
        ConfirmbuttonCallBacks = null;
        ExtralbuttonCallBacks = null;
    }
    public void HidePanel(Action p)
    {
        counter = 0f;
        IsLoading = false;
        anim.SetBool("FadeIn", false);
        p();

        CancelbuttonCallBacks = null;
        ConfirmbuttonCallBacks = null;
        ExtralbuttonCallBacks = null;

        PopUp_Title.text = "Title";
        PopUp_Description.text = "Description";
    }

    private void ShowPanel(Action p)
    {
        anim.SetBool("FadeIn", true);
    }
}
