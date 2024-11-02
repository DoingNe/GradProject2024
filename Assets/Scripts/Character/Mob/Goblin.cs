using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Mob
{
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        Hp = 3;
        speed = 3f;
        jump = 10f;
        atk = 1;
        gold = 10;
    }
}
