using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [FoldoutGroup("PlayerSetting")]
    public Transform player; // 플레이어 Transform
    [FoldoutGroup("PlayerSetting"), SerializeField, LabelText("플레이어 목숨 수")]
    private int playerLives = 3; // 플레이어의 목숨 수
    [FoldoutGroup("PlayerSetting"), LabelText("플레이어 스폰 장소"), Space(5f)]
    public Transform spawnLocation; // 플레이어 스폰 장소

    private List<GameObject> enemyList_ = new List<GameObject>();
    public List<GameObject> enemyList => enemyList_;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartNewLife();
    }

    void Update()
    {
        // 플레이어의 사망을 임의로 감지 (예시로 "R" 키를 사용해 사망 처리)
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnPlayerDeath();
        }
    }

    private void StartNewLife()
    {
        ResetPlayerState();
        player.GetComponent<Player>().ResetPlayer();
    }

    private void ResetPlayerState()
    {
        player.GetComponent<CharacterController>().enabled = false;
        // 플레이어 상태 및 위치 초기화
        player.position = spawnLocation.position;

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<CharacterController>().Move(Vector3.zero);
        // 여기에 플레이어의 체력, 점수 등의 초기화 코드를 추가할 수 있습니다.
    }

    private void OnPlayerDeath()
    {
        playerLives--;

        player.GetComponent<Player>().Die();

        if (playerLives > 0)
        {
            RestartGame();
        }
        else
        {
            // 게임 오버 처리
            Debug.Log("Game Over");
        }
    }

    private void RestartGame()
    {
        ResetLevel();

        StartNewLife();    
    }

 

    private void ResetLevel()
    {

        foreach(GameObject enemy in enemyList) //적 제거
        {
            if(enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemyList.Clear();
        
    }
}

