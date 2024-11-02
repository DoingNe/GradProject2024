using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnScript : MonoBehaviour
{
    public GameObject popUpUI;
    public bool timeStop;

    public string sceneName;

    public void PopUp()
    {
        Debug.Log(popUpUI.gameObject.name);
        
        if (popUpUI != null)
        {
            if (timeStop && Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
            }
            else if (timeStop && Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
            }

            popUpUI.SetActive(!popUpUI.activeInHierarchy);
        }
    }

    public void MoveScene()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
