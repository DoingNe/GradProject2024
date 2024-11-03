using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 버튼 관련 기능
 * 팝업, 씬이동, 스탯 강화, 게임 종료
 */
public class BtnScript : MonoBehaviour
{
    public GameObject popUpUI;                                  // 팝업창 오브젝트
    public bool timeStop;                                       // 일시정지 여부
    public TMP_Text upgradeGoldText;                            // 강화 시 필요 재화량을 표시할 텍스트 오브젝트
    public Define.StatNum statNum;                              // 스탯 종류
    public string sceneName;                                    // 이동할 씬 이름

    // 팝업
    public void PopUp()
    {
        if (popUpUI != null)
        {
            // 일시정지
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

    // 씬 이동
    public void MoveScene()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // 강화
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

    /*
     * 재화를 지불하고 스탯을 강화
     * 임시 가중치를 weight로 설정
     */
    void BuyStat(Define.StatNum statNum, int weight)
    {
        if (statNum == Define.StatNum.Life)
        {
            GameManager.Instance.Player.GainHeart(1);           // 체력 회복
        }

        GameManager.Instance.playerStat[(int)statNum]++;
        GameManager.Instance.Player.Gold -= int.Parse(upgradeGoldText.text);            // 재화 소비
        GameManager.Instance.spendGold += int.Parse(upgradeGoldText.text);              // 소비한 재화 기록
        upgradeGoldText.text = (int.Parse(upgradeGoldText.text) + weight).ToString();   // 필요 재화량 증가
    }

    // 게임 종료
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
