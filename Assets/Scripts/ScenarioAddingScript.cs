using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ScenarioAddingScript : MonoBehaviour
{
    int currentPage = 1;
    [SerializeField] GameObject PanelObject = null;
    [SerializeField] [Range(0.1f, 1)] float easing = 0f;

    [Header("VOICE COMMAND")]
    [SerializeField] Transform TagsGroup = null;
    [SerializeField] GameObject TagsInputField = null;
    private int NumberOfTags = 0;
    int MaxTagNumber = 5;

    [Header("Actions")]
    [SerializeField] Transform ActionsGroup=null;
    [SerializeField] GameObject ActionsInputField = null;
    public int NumberOfActions = 0;
    int MaxActionNumber = 3;

    [Header("Location")]
    [SerializeField] TMP_Dropdown TriggerDropDown;
    [SerializeField] TMP_Dropdown LocationDropDown;

    [Header("initializing")]
    [SerializeField] public List<Scenariosavingscript.DATA> SO = new List<Scenariosavingscript.DATA>();
    [SerializeField] Transform ScenarioParent = null;
    [SerializeField] GameObject ScenarioPrefab = null;

    [Header("Icons")] [SerializeField] Sprite[] ICONS = null;

    [Header("UI Elements")]
    [SerializeField] TMP_InputField ScenarioText = null;
    [SerializeField] TMP_Dropdown IconsDropDown = null;
    [SerializeField] TMP_Dropdown[] Functions = null;
    [SerializeField] TMP_Dropdown[] Channels = null;

    private void Start()
    {
        SO = Scenariosavingscript.Load_Scenario_Data();
        Debug.Log("Number of scnerarios : "+SO.Count);
        for (int i = 0; i < SO.Count; i++)
        {
            GameObject NewScenario = Instantiate(ScenarioPrefab, ScenarioParent);
            var IDCHNG = NewScenario.GetComponent<ScenarioIdentityChanger>();
            IDCHNG.ChangeableICon = ICONS[SO[i].icon];
            IDCHNG.ScenarioName = SO[i].ScenarioName;
            IDCHNG.Channel = SO[i].Channel;
            IDCHNG.Function = SO[i].Function;
            IDCHNG.scenarioindex = i;
            IDCHNG.OnHomeActivation = SO[i].HomeEnterTrigger;
            IDCHNG.OnExitActivation = SO[i].HomeExitTrigger;
        }
    }

    public void ADDScenePanel(int funtion)
    {
        if (funtion == 1)
        {
            if (NumberOfTags < MaxTagNumber)
            {
                NumberOfTags++;
                Instantiate(TagsInputField, TagsGroup);
            }
        }
        else if (funtion == 2)
        {
            if (NumberOfActions < MaxActionNumber)
            {
                NumberOfActions++;
                Instantiate(ActionsInputField, ActionsGroup);
            }
        }
    }

    public void autoswipe(bool toright)
    {
        if (toright)
        {
            if (currentPage < 4)
            {
                Vector3 newLocation = PanelObject.transform.position; // saving the first location of the panel to return it if the user exceeds the last or first page
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                StartCoroutine(SmoothMove(PanelObject.transform.position, newLocation, easing)); // moves the panel smoothly
            }
        }
        else {
            if (currentPage > 1)
            {
                Vector3 newLocation = PanelObject.transform.position; // saving the first location of the panel to return it if the user exceeds the last or first page
                currentPage--;
                newLocation += new Vector3(+Screen.width, 0, 0);
                StartCoroutine(SmoothMove(PanelObject.transform.position, newLocation, easing)); // moves the panel smoothly
            }

        }

    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            PanelObject.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

    }

    public void AddnewScenarioOBject()
    {
        RefreshDropDowns();

        var Name = ScenarioText.text;
        var icon = IconsDropDown.value;

        string[] CHANNELArray = new string[Functions.Length];
        int[] FUNCTIONArray = new int[Functions.Length];

        for (int index =0 ; index < Functions.Length; index++)
        {
            CHANNELArray[index] = Channels[index].captionText.text;
            FUNCTIONArray[index] = Functions[index].value;
        }

        bool HasLocationTrigger = TriggerDropDown.value == 1;
        bool[] LocationType = {false,false };
        LocationType[LocationDropDown.value] = true;
        Scenariosavingscript.DATA NewElement = new Scenariosavingscript.DATA(Name, icon, FUNCTIONArray,CHANNELArray, LocationType[0], LocationType[1]);
        SO.Add(NewElement);
        int SOSIZE = SO.Count-1;

        GameObject NewScenario = Instantiate(ScenarioPrefab, ScenarioParent);
        if (ScenarioPrefab == null) { Debug.LogError("no prefab Found"); }
        var IDCHNG = NewScenario.GetComponent<ScenarioIdentityChanger>();
        IDCHNG.ChangeableICon = ICONS[SO[SOSIZE].icon];
        IDCHNG.ScenarioName = SO[SOSIZE].ScenarioName;
        IDCHNG.Channel = SO[SOSIZE].Channel;
        IDCHNG.Function = SO[SOSIZE].Function;
        IDCHNG.OnHomeActivation = SO[SOSIZE].HomeEnterTrigger;
        IDCHNG.OnExitActivation = SO[SOSIZE].HomeExitTrigger;
        Save();

    }

    void RefreshDropDowns() 
    {
        Debug.Log("Refreshing");
        GameObject[] FunctionsObjs = GameObject.FindGameObjectsWithTag("Functions");
        GameObject[] ChannelsObjs = GameObject.FindGameObjectsWithTag("Channels");

        Functions = new TMP_Dropdown[FunctionsObjs.Length];
        Channels = new TMP_Dropdown[FunctionsObjs.Length];

        for (int index=0;index <FunctionsObjs.Length ;index++) 
        {
            Functions[index] = FunctionsObjs[index].GetComponent<TMP_Dropdown>();
            Channels[index] = ChannelsObjs[index].GetComponent<TMP_Dropdown>();
        }
    }

    public void Save() {
        Scenariosavingscript.Save_Scenario_inJASON(SO);
    }


    public void LOAD() {
        SO = Scenariosavingscript.Load_Scenario_Data();
    }
}
