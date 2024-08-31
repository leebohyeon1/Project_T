using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [FoldoutGroup("PlayerSetting")]
    public PlayerActionRecorder playerActionRecorder; // PlayerActionRecorder ��ũ��Ʈ ����
    [FoldoutGroup("PlayerSetting")]
    public Transform player; // �÷��̾� Transform
    [FoldoutGroup("PlayerSetting"), SerializeField, LabelText("�÷��̾� ��� ��")]
    private int playerLives = 3; // �÷��̾��� ��� ��
    [FoldoutGroup("PlayerSetting"), LabelText("�÷��̾� ���� ���"), Space(5f)]
    public Transform spawnLocation; // �÷��̾� ���� ���

    [FoldoutGroup("CloneSetting"), LabelText("Ŭ�� ������")]
    public NPCReplayer npcReplayerPrefab; // NPCReplayer ������ ����

    private List<List<PlayerActionRecorder.PlayerAction>> allRecordedActions = new List<List<PlayerActionRecorder.PlayerAction>>();
    private List<GameObject> actionClones = new List<GameObject>();

    private List<GameObject> objectClones_ = new List<GameObject>();
    public List<GameObject> objectClones => objectClones_;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
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
        playerActionRecorder.ResetRecording();
        playerActionRecorder.StartRecording();
    }

    private void ResetPlayerState()
    {
        // �÷��̾� ���� �� ��ġ �ʱ�ȭ
        player.position = spawnLocation.position;
        // ���⿡ �÷��̾��� ü��, ���� ���� �ʱ�ȭ �ڵ带 �߰��� �� �ֽ��ϴ�.
    }

    private void OnPlayerDeath()
    {
        playerLives--;

        // �÷��̾��� �ൿ�� ����
        allRecordedActions.Add(playerActionRecorder.GetRecordedActions());
        playerActionRecorder.StopRecording();

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
        DestroyClones();

        StartNewLife();

        foreach (var recordedActions in allRecordedActions)
        {
            CreateNPC(recordedActions);
        }

    }

    private void CreateNPC(List<PlayerActionRecorder.PlayerAction> actions)
    {
        NPCReplayer npcInstance = Instantiate(npcReplayerPrefab, player.position, Quaternion.identity);
        actionClones.Add(npcInstance.gameObject);
        npcInstance.StartReplay(actions);
    }

    private void DestroyClones()
    {
        foreach (GameObject clone in actionClones)
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        actionClones.Clear();

        foreach (GameObject clone in objectClones)
        {
            if(clone != null)
            {
                Destroy(clone);
            }
        }
        objectClones.Clear();
        
    }
}

