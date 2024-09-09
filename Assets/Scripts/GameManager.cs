using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Player Player;

    public int currentStage = 0;    // 0 = First stage, 1 = Second stage, 2 = Boss room
    public int maxEnemy = 15;
    public bool isGameOver = false;

    public GameObject[] currentSpawnedList;
    public GameObject[] enemyList1;
    public GameObject[] enemyList2;
    public Transform[] enemySpawnPointGroup;   // 0 = First stage, 1 = Second stage, 2 = Boss room
    public Transform[] points;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        points = enemySpawnPointGroup[currentStage].GetComponentsInChildren<Transform>();
    }

}
