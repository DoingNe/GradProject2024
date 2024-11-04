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
    public bool isEnhance;                                      // ķ�����̾� ��ư ����
    public TMP_Text costText;
    public TMP_Text upgradeGoldText;                            // ��ȭ �� �ʿ� ��ȭ���� ǥ���� �ؽ�Ʈ ������Ʈ
    public Define.StatNum statNum;                              // ���� ����
    public string sceneName;                                    // �̵��� �� �̸�

    // �˾�
    public void PopUp()
    {
        if (popUpUI != null)
        {
            popUpUI.SetActive(!popUpUI.activeInHierarchy);
        }
    }

    public void EnhancePopUp()
    {
        if (popUpUI != null)
        {
            // �Ͻ�����
            if (isEnhance && Time.timeScale == 1f && GameManager.Instance.Player.Gold >= int.Parse(costText.text))  // ����
            {
                Time.timeScale = 0f;            // �Ͻ�����

                GameManager.Instance.Player.Gold -= int.Parse(costText.text);   // ��ȭ ����
                GameManager.Instance.spendGold += int.Parse(costText.text);
                costText.text = (int.Parse(costText.text) + 25).ToString();
                GameManager.Instance.Player.GainHp();                       // �ӽ� ü�� ȸ��
                popUpUI.SetActive(true);
            }
            else if (isEnhance && Time.timeScale == 0f)             // �ݱ�
            {
                Time.timeScale = 1f;
                popUpUI.SetActive(false);
            }
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
            GameManager.Instance.Player.GainHp();           // ü�� ȸ��
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
