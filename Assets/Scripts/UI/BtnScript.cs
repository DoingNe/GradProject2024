using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * ��ư ���� ���
 * �˾�, ���̵�, ���� ��ȭ, ���� ����
 */
public class BtnScript : MonoBehaviour
{
    public GameObject popUpUI;                                  // �˾�â ������Ʈ
    public bool timeStop;                                       // �Ͻ����� ����
    public TMP_Text upgradeGoldText;                            // ��ȭ �� �ʿ� ��ȭ���� ǥ���� �ؽ�Ʈ ������Ʈ
    public Define.StatNum statNum;                              // ���� ����
    public string sceneName;                                    // �̵��� �� �̸�

    // �˾�
    public void PopUp()
    {
        if (popUpUI != null)
        {
            // �Ͻ�����
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

    // �� �̵�
    public void MoveScene()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // ��ȭ
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
     * ��ȭ�� �����ϰ� ������ ��ȭ
     * �ӽ� ����ġ�� weight�� ����
     */
    void BuyStat(Define.StatNum statNum, int weight)
    {
        if (statNum == Define.StatNum.Life)
        {
            GameManager.Instance.Player.GainHeart(1);           // ü�� ȸ��
        }

        GameManager.Instance.playerStat[(int)statNum]++;
        GameManager.Instance.Player.Gold -= int.Parse(upgradeGoldText.text);            // ��ȭ �Һ�
        GameManager.Instance.spendGold += int.Parse(upgradeGoldText.text);              // �Һ��� ��ȭ ���
        upgradeGoldText.text = (int.Parse(upgradeGoldText.text) + weight).ToString();   // �ʿ� ��ȭ�� ����
    }

    // ���� ����
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
