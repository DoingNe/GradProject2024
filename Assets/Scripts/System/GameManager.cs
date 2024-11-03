using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 플레이 중 얻은 지표를 기록 및 관리
 */
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Camera camera;
    public Player Player;

    public int[] playerStat;        // 0 = atk, 1 = speed, 2 = gold, 3 = life

    // 임시 지표
    public int kill;                // 잡은 몬스터 수
    public int earnGold;            // 얻은 재화량
    public int spendGold;           // 소비한 재화량
    public float time;              // 플레이 시간

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
        playerStat = new int[4];

        InitGame();
    }

    private void FixedUpdate()
    {
        // 플레이 시간 기록
        if (Instance.Player != null && Instance.Player.isPlaying)
        {
            time += Time.fixedDeltaTime;
        }
    }

    // 지표 초기화
    public void InitGame()
    {
        for (int i = 0; i < playerStat.Length; i++) playerStat[i] = 0;

        kill = 0;
        earnGold = 0;
        spendGold = 0;
        time = 0f;
    }
}