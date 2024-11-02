using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldTextBox : MonoBehaviour
{
    public TMP_Text tmpTxt;

    // Update is called once per frame
    void FixedUpdate()
    {
        tmpTxt.text = GameManager.Instance.Player.Gold.ToString();
    }
}
