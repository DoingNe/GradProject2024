using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Camera camera;
    public Player Player;

    public int[] playerStat;        // 0 = atk, 1 = speed, 2 = gold
    public int kill;
    public int earnGold;
    public int spendGold;
    public float time;

    public int currentStage = 0;    // 0 = First stage, 1 = Second stage, 2 = Boss room

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("GameManager");
                    instance = instanceContainer.AddComponent<GameManager>();
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    void Start()
    {
        playerStat = new int[3];

        InitGame();
    }

    private void FixedUpdate()
    {
        if (Instance.Player != null && Instance.Player.isPlaying)
        {
            time += Time.fixedDeltaTime;
        }
    }

    public void InitGame()
    {
        for (int i = 0; i < playerStat.Length; i++) playerStat[i] = 0;
        kill = 0;
        earnGold = 0;
        spendGold = 0;
        time = 0f;
    }
}
