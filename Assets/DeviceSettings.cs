using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class DeviceSettings : MonoBehaviour
{

   public static DeviceSettings Instance = null;

    [SerializeField] Animator anim = null;

    [Space(10)]
    [SerializeField] TMP_Text Title = null;
    public TMP_InputField NameField = null;
    [Space(10)]

    public int IconIndex = 0;
    [SerializeField] bool IsShown = false;
    [SerializeField] GameObject ButtonsParent = null;
    [SerializeField] List<Button> ButtonList = new List<Button>();

    System.Action CancelButtonCallBacks = null;
    System.Action SaveButtonCallBack = null;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PopulateButtonList();
    }

    void PopulateButtonList()
    {
        Button[] buttons = ButtonsParent.GetComponentsInChildren<Button>();
        for (int index = 0; buttons.Length > index; index++)
        {
            var i = index;
            var Button = buttons[index];
            ButtonList.Add(Button);
            Button.onClick.AddListener(delegate { SetButtonIndex(i); });
        }
    }

    private void SetButtonIndex(int index)
    {
        DeviceSettings.Instance.IconIndex = index;
    }

    public void Show(string PanelTitle,string DeviceName,System.Action CancelCallBack, System.Action SaveCallBack)
    {
        Title.text = PanelTitle;
        NameField.placeholder.GetComponent<TMP_Text>().text = DeviceName;

        if (!IsShown) IsShown = true;
        CancelButtonCallBacks = CancelCallBack;
        SaveButtonCallBack = SaveCallBack;
    }
    public void Show(string PanelTitle, string DeviceName,System.Action SaveCallBack)
    {
        Title.text = PanelTitle;
        NameField.placeholder.GetComponent<TMP_Text>().text = DeviceName;

        if (!IsShown) IsShown = true;
        SaveButtonCallBack = SaveCallBack;
    }

    void FixedUpdate()
    {
        if (IsShown)
        {
            ShowPanel(() => {
                IsShown = true;
            });
        }        
    }

    private void ShowPanel(System.Action p)
    {
        anim.SetBool("FadeIn", true);
    }

    public void HidePanel()
    {
        IsShown = false;
        anim.SetBool("FadeIn", false);

        CancelButtonCallBacks = null;
        SaveButtonCallBack = null;

        Title.text = "Title";
        NameField.text = "Description";
        IconIndex = 0;
    }
    public void Save() 
    {
        SaveButtonCallBack();
        HidePanel();
    }

    /*HidePanel(() => {
        IsShown = false;
        CancelbuttonCallBacks();
    });*/

}
