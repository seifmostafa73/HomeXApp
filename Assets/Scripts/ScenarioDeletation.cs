using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioDeletation : MonoBehaviour
{

    public int IndexOfScenario = 0;
    ScenarioAddingScript SOManager;
    [HideInInspector]public GameObject ScenarioToDelete;

    private void Start()
    {
        SOManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ScenarioAddingScript>();
    }
    public void YesCase()
    {
        var ScenarioList = SOManager.SO;
        Destroy(ScenarioToDelete);
        ScenarioList.Remove(ScenarioList[IndexOfScenario]);
        SOManager.Save();
    }

}
