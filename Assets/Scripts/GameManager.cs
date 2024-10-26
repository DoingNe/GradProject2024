using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Camera camera;
    public Player Player;

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

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        Player = FindObjectOfType<Player>();
    }
}
