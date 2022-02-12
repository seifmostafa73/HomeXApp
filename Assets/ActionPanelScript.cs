using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelScript : MonoBehaviour
{
    public void minusButton()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<ScenarioAddingScript>().NumberOfActions--;
        Destroy(gameObject);
    }
}
