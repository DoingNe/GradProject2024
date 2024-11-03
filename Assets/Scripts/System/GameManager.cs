using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �÷��� �� ���� ��ǥ�� ��� �� ����
 */
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Camera camera;
    public Player Player;

    public int[] playerStat;        // 0 = atk, 1 = speed, 2 = gold, 3 = life

    // �ӽ� ��ǥ
    public int kill;                // ���� ���� ��
    public int earnGold;            // ���� ��ȭ��
    public int spendGold;           // �Һ��� ��ȭ��
    public float time;              // �÷��� �ð�

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
        // �÷��� �ð� ���
        if (Instance.Player != null && Instance.Player.isPlaying)
        {
            time += Time.fixedDeltaTime;
        }
    }

    // ��ǥ �ʱ�ȭ
    public void InitGame()
    {
        for (int i = 0; i < playerStat.Length; i++) playerStat[i] = 0;

        kill = 0;
        earnGold = 0;
        spendGold = 0;
        time = 0f;
    }
}