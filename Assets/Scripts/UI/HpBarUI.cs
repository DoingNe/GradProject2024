using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    public Slider hpBar;

    void Start()
    {
        hpBar.maxValue = GameManager.Instance.Player.MaxHp;
        hpBar.minValue = 0;
    }

    void Update()
    {
        hpBar.value = GameManager.Instance.Player.Hp;
    }
}
