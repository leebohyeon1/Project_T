using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerActionRecorder playerActionRecorder; // PlayerActionRecorder 스크립트 참조
    public NPCReplayer npcReplayerPrefab; // NPCReplayer 프리팹 참조
    public Transform player; // 플레이어 Transform
    private int playerLives = 3; // 플레이어의 목숨 수

    private List<List<PlayerActionRecorder.PlayerAction>> allRecordedActions = new List<List<PlayerActionRecorder.PlayerAction>>();
    private List<GameObject> actionClones = new List<GameObject>();

    void Start()
    {
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
        playerActionRecorder.ResetRecording();
        playerActionRecorder.StartRecording();
    }

    private void ResetPlayerState()
    {
        // 플레이어 상태 및 위치 초기화
        player.position = new Vector3(0, 1, 0);
        // 여기에 플레이어의 체력, 점수 등의 초기화 코드를 추가할 수 있습니다.
    }

    private void OnPlayerDeath()
    {
        playerLives--;

        // 플레이어의 행동을 저장
        allRecordedActions.Add(playerActionRecorder.GetRecordedActions());
        playerActionRecorder.StopRecording();

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
