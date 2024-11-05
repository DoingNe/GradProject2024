using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Mob
{
    void Awake()
    {
        base.Awake();
        Hp = 5;
        speed = 10f;
        jump = 10f;
        atk = 2;
        gold = 20;
    }
}
