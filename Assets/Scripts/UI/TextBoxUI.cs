using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * 골드 표시 UI
 * 현재 보유 중인 골드량을 표시
 */
public class TextBoxUI : MonoBehaviour
{
    public Define.TextBoxNum textBoxNum;
    public TMP_Text tmpTxt;

    void Update()
    {
        switch (textBoxNum)
        {
            case Define.TextBoxNum.Gold:
                tmpTxt.text = GameManager.Instance.Player.Gold.ToString();
                break;
            case Define.TextBoxNum.Timer:
                tmpTxt.text = tmpTxt.text = FormatTime(GameManager.Instance.time);
                break;
        }
        
    }

    string FormatTime(float floatTime)
    {
        int hours = (int)(floatTime / 3600);
        int minutes = (int)((floatTime % 3600) / 60);
        int seconds = (int)(floatTime % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
