using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeBtn : MonoBehaviour
{
    public string sceneName;

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
