using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * ���� ��ȭ UI
 * ���� ��ȭ�� Ƚ���� ���� ��ġ�� ������
*/
public class EnhanceTextBox : MonoBehaviour
{
    public Define.StatNum statNum;                                  // ���� ����
    public TMP_Text lvTxt;                                          // ����� �ؽ�Ʈ ������Ʈ
    
    void Update()
    {
        int lv = GameManager.Instance.playerStat[(int)statNum];     // ���� ������ ��ȭ ��ġ

        switch (statNum)
        {
            case Define.StatNum.Atk:
                lvTxt.text = "LV." + lv + "\n+" + lv;
                break;
            case Define.StatNum.Spd:
                lvTxt.text = "LV." + lv + "\n+" + (5 * lv) + "%";
                break;
            case Define.StatNum.Gold:
                lvTxt.text = "LV." + lv + "\n+" + (10 * lv) + "%";
                break;
            default:
                break;
        }
    }
}
