using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnScript : MonoBehaviour
{
    public GameObject popUpUI;
    public bool timeStop;
    public TMP_Text upgradeGoldText;
    public Define.StatNum statNum;
    public string sceneName;

    public void PopUp()
    {
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

    public void EnhanceBtn()
    {
        if (upgradeGoldText != null && GameManager.Instance.Player.Gold >= int.Parse(upgradeGoldText.text))
        {
            int lv = GameManager.Instance.playerStat[(int)statNum];

            switch (statNum)
            {
                case Define.StatNum.Atk:
                    BuyStat(statNum, 5);
                    break;
                case Define.StatNum.Spd:
                    BuyStat(statNum, 5);
                    break;
                case Define.StatNum.Gold:
                    BuyStat(statNum, 5);
                    break;
                case Define.StatNum.Life:
                    BuyStat(statNum, 50);
                    break;
                default:
                    break;
            }
        }
    }

    void BuyStat(Define.StatNum statNum, int weight)
    {
        if (statNum == Define.StatNum.Life)
        {
            GameManager.Instance.Player.GainHeart(1);
        }

        GameManager.Instance.playerStat[(int)statNum]++;
        GameManager.Instance.Player.Gold -= int.Parse(upgradeGoldText.text);
        GameManager.Instance.spendGold += int.Parse(upgradeGoldText.text);
        upgradeGoldText.text = (int.Parse(upgradeGoldText.text) + weight).ToString();
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
