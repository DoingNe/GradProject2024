using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * ��� ǥ�� UI
 * ���� ���� ���� ��差�� ǥ��
 */
public class GoldTextBox : MonoBehaviour
{
    public TMP_Text tmpTxt;

    void Update()
    {
        tmpTxt.text = GameManager.Instance.Player.Gold.ToString();  
    }
}
