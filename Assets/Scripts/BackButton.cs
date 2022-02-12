using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackButton : MonoBehaviour
{
    [SerializeField] Transform PlugsParent = null;
    [SerializeField] bool GoDown = false;
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GOback();
        }
    }
    public void GOback() 
    {
        var GoToPostion = GoDown ? -Screen.height : Screen.height;
        gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, GoToPostion), 0.25f);
        if (PlugsParent != null)
        {
            DeviceIdentityChanger[] Children = PlugsParent.GetComponentsInChildren<DeviceIdentityChanger>();

            foreach (DeviceIdentityChanger child in Children)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
