using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldTextBox : MonoBehaviour
{
    public TMP_Text tmpTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmpTxt.text = GameManager.Instance.Player.Gold.ToString();
    }
}
