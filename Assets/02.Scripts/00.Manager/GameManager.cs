using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerActionRecorder playerActionRecorder; // PlayerActionRecorder ��ũ��Ʈ ����
    public NPCReplayer npcReplayerPrefab; // NPCReplayer ������ ����
    public Transform player; // �÷��̾� Transform
    private int playerLives = 3; // �÷��̾��� ��� ��

    private List<List<PlayerActionRecorder.PlayerAction>> allRecordedActions = new List<List<PlayerActionRecorder.PlayerAction>>();
    private List<GameObject> actionClones = new List<GameObject>();

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
        player.position = new Vector3(0, 1, 0);
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
    }
}
