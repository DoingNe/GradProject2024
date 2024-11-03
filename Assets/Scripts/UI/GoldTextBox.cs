using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * 골드 표시 UI
 * 현재 보유 중인 골드량을 표시
 */
public class GoldTextBox : MonoBehaviour
{
    public TMP_Text tmpTxt;

    void Update()
    {
        tmpTxt.text = GameManager.Instance.Player.Gold.ToString();  
    }
}
