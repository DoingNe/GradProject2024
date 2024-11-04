using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * ���â UI
 * �̹� ������ ����� ǥ��
 */
public class ResultTextBox : MonoBehaviour
{
    public Define.StatNum statNum;
    public Define.ResultNum resultNum;

    public TMP_Text textBox;

    void Start()
    {
        switch (statNum)                        // ���� ��ȭ ��ǥ ���
        {
            case Define.StatNum.Atk:
            case Define.StatNum.Spd:
            case Define.StatNum.Gold:
            case Define.StatNum.Life:
                textBox.text = "LV. " + GameManager.Instance.playerStat[(int)statNum].ToString();
                break;
            default:
                break;
        }

        switch (resultNum)
        {
            case Define.ResultNum.Time:         // �÷��� Ÿ�� ��ǥ ���
                textBox.text = FormatTime(GameManager.Instance.time);
                break;
            case Define.ResultNum.Kill:         // ų ��ǥ ���
                textBox.text = GameManager.Instance.kill.ToString();
                break;
            case Define.ResultNum.Gold:         // ���� ��ȭ ��ǥ ���
                textBox.text = GameManager.Instance.earnGold.ToString() + " G";
                break;
            case Define.ResultNum.Spend:        // �Һ��� ��ȭ ��ǥ ���
                textBox.text = GameManager.Instance.spendGold.ToString() + " G";
                break;
            default:
                break;
        }
    }

    string FormatTime(float floatTime)
    {
        int hours = (int)(floatTime / 3600);
        int minutes = (int)((floatTime % 3600) / 60);
        int seconds = (int)(floatTime % 60);

        Debug.Log(string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds));

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
