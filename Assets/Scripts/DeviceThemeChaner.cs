using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DeviceThemeChaner : MonoBehaviour
{
    [SerializeField] Image Cap =null;
    [SerializeField] TMP_Text Font = null;
    [SerializeField] colorthemeController themeController = null;

    Button Scenariobutton = null;

    void Start()
    {
        themeController = GameObject.FindGameObjectWithTag("Manager").GetComponent<colorthemeController>();
        Scenariobutton = gameObject.GetComponent<Button>();
        var Colors = Scenariobutton ? Scenariobutton.colors:ColorBlock.defaultColorBlock ;
        if (Cap != null&& Colors!=null)
        {
            Cap.color = colorthemeController.CurrentTheme.FrontColor;
            Colors.normalColor = colorthemeController.CurrentTheme.FrontColor;
            Colors.pressedColor = colorthemeController.CurrentTheme.ExtraColor;
            themeController.PrimaryObjects.Add(Cap);
        }
        if (Font != null)
        {
            Font.color = colorthemeController.CurrentTheme.FontColor;
            themeController.TextElements.Add(Font);
        }
    }
    private void OnDestroy()
    {
        themeController.PrimaryObjects.Remove(Cap);
        themeController.TextElements.Remove(Font);
    }

}
