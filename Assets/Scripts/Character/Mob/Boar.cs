using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Mob
{
    void Awake()
    {
        base.Awake();
        Hp = 5;
        speed = 5f;
        jump = 10f;
        atk = 1;
        gold = 20;
    }
}
