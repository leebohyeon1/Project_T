using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerActionRecorder playerRecorder;
    public GameObject npcPrefab; // NPC ������
    private int playerLives = 3;
    private List<List<PlayerActionRecorder.PlayerAction>> pastActions = new List<List<PlayerActionRecorder.PlayerAction>>();

    void Update()
    {
        // �÷��̾� ��� ���� ���� (���⼱ ������ �ı� �� ���)
        if (/* ������ �ı� ���� */Input.GetKeyDown(KeyCode.R))
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        playerLives--;

        // ���� �÷��̾��� �ൿ ����� ����
        pastActions.Add(playerRecorder.GetRecordedActions());
        playerRecorder.StopRecording();

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
        // ���� ���¸� �ʱ�ȭ�ϰ�, ���ο� ������� ���� ����
        // ... (�ʱ�ȭ �ڵ�)


        // ���� �ൿ�� �����ϴ� NPC ����
        foreach (var actions in pastActions)
        {
            CreateNPC(actions);
        }

        // ���ο� ������� �÷��̾� �ൿ ��� �簳
        playerRecorder.enabled = true;
        playerRecorder.ResetAction();
        playerRecorder.gameObject.transform.position = new Vector3(0, 1, 0);
    }

    private void CreateNPC(List<PlayerActionRecorder.PlayerAction> actions)
    {
        GameObject npc = Instantiate(npcPrefab, actions[0].Position, actions[0].Rotation);
        NPCReplayer npcReplayer = npc.GetComponent<NPCReplayer>();
        npcReplayer.StartReplay(actions);
    }
}
