using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoTrigger : MonoBehaviour
{

    bool RememberMe = false;
    
    void Start()
    {
        RememberMe = PlayerPrefs.GetInt("RememberMe", 0) == 1;
        LoadNextScene();

    }
    public void LoadNextScene()
    {
        if (RememberMe && PlayerPrefs.GetString("Name", "NULL") != "NULL")
        {
            StartCoroutine(LoadAsycLevel(3));
        }
        else
        {
            StartCoroutine(LoadAsycLevel(1));
        }
    }

    public IEnumerator LoadAsycLevel(int index)
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation AOperation = SceneManager.LoadSceneAsync(index);


    }
}
