using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpBtn : MonoBehaviour
{
    public GameObject popUpUI;
    public bool timeStop;

    public void PopUp()
    {
        Debug.Log(popUpUI.gameObject.name);

        if (popUpUI != null)
        {
            if(timeStop && Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
            }
            else if(timeStop && Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
            }

            popUpUI.SetActive(!popUpUI.activeInHierarchy);
        }
    }
}
