using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [FoldoutGroup("PlayerSetting")]
    public Transform player; // �÷��̾� Transform
    [FoldoutGroup("PlayerSetting"), SerializeField, LabelText("�÷��̾� ��� ��")]
    private int playerLives = 3; // �÷��̾��� ��� ��
    [FoldoutGroup("PlayerSetting"), LabelText("�÷��̾� ���� ���"), Space(5f)]
    public Transform spawnLocation; // �÷��̾� ���� ���

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
        // �÷��̾��� ����� ���Ƿ� ���� (���÷� "R" Ű�� ����� ��� ó��)
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
        // �÷��̾� ���� �� ��ġ �ʱ�ȭ
        player.position = spawnLocation.position;

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<CharacterController>().Move(Vector3.zero);
        // ���⿡ �÷��̾��� ü��, ���� ���� �ʱ�ȭ �ڵ带 �߰��� �� �ֽ��ϴ�.
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
            // ���� ���� ó��
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

        foreach(GameObject enemy in enemyList) //�� ����
        {
            if(enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemyList.Clear();
        
    }
}

