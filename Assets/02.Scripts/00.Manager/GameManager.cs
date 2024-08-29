using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerActionRecorder playerRecorder;
    public GameObject npcPrefab; // NPC 프리팹
    private int playerLives = 3;
    private List<List<PlayerActionRecorder.PlayerAction>> pastActions = new List<List<PlayerActionRecorder.PlayerAction>>();

    void Update()
    {
        // 플레이어 사망 조건 예시 (여기선 발전기 파괴 시 사망)
        if (/* 발전기 파괴 조건 */Input.GetKeyDown(KeyCode.R))
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        playerLives--;

        // 현재 플레이어의 행동 기록을 저장
        pastActions.Add(playerRecorder.GetRecordedActions());
        playerRecorder.StopRecording();

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
        // 게임 상태를 초기화하고, 새로운 목숨으로 게임 시작
        // ... (초기화 코드)


        // 이전 행동을 재현하는 NPC 생성
        foreach (var actions in pastActions)
        {
            CreateNPC(actions);
        }

        // 새로운 목숨으로 플레이어 행동 기록 재개
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
