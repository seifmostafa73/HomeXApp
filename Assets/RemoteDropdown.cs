using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteDropdown : MonoBehaviour
{
   [SerializeField] List<string> RemotesList = new List<string>();
    [SerializeField] Dropdown RemoteDropDown = null;

    void Start()
    {
        StartCoroutine(RefreshDropDown());
    }

    private IEnumerator RefreshDropDown()
    {
        while (true)
        {
            RemotesList.Clear();
            RemoteDropDown.options.Clear();
            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.GetString("Remote" + i, "") != "")
                {
                    RemotesList.Add(PlayerPrefs.GetString("Remote" + i, ""));

                }
            }
            RemoteDropDown.AddOptions(RemotesList);
            yield return new WaitForSeconds(3f);
        }
    }
}
