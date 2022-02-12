using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioStartPosition : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    void Start()
    {
        Vector3 Newlocation = new Vector3((canvas.GetComponent<RectTransform>().rect.width), 0f, 0f);
        gameObject.GetComponent<RectTransform>().localPosition = Newlocation;
    }

}
