using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * 스탯 강화 UI
 * 현재 강화된 횟수와 얻은 수치를 보여줌
*/
public class EnhanceTextBox : MonoBehaviour
{
    public Define.StatNum statNum;                                  // 스탯 종류
    public TMP_Text lvTxt;                                          // 출력할 텍스트 오브젝트
    
    void Update()
    {
        int lv = GameManager.Instance.playerStat[(int)statNum];     // 현재 스탯의 강화 수치

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
