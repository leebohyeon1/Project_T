using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReplayer : MonoBehaviour
{
    private List<PlayerActionRecorder.PlayerAction> recordedActions;
    private int currentActionIndex = 0;
    private bool isReplaying = false;
    private float replayStartTime;

    void FixedUpdate()
    {
        if (isReplaying)
        {
            ReplayAction();
        }
    }

    // 리플레이를 시작하는 메서드
    public void StartReplay(List<PlayerActionRecorder.PlayerAction> actions)
    {
        if (actions == null || actions.Count == 0) return;

        recordedActions = actions;
        currentActionIndex = 0;
        isReplaying = true;
        replayStartTime = Time.time;
    }

    // 행동을 재생하는 메서드
    private void ReplayAction()
    {
        if (currentActionIndex >= recordedActions.Count)
        {
            isReplaying = false;
            Destroy(gameObject);
            return;
        }

        // 현재 행동 가져오기
        var action = recordedActions[currentActionIndex];

        // 현재 시간과 재생 시작 시간의 차이 계산
        float elapsedTime = Time.time - replayStartTime;

        // 만약 현재 행동의 타임스탬프가 경과된 시간보다 작거나 같으면 행동 재생
        if (action.TimeStamp <= elapsedTime)
        {
            transform.position = action.Position;
            transform.rotation = action.Rotation;

            // 다음 행동으로 이동
            currentActionIndex++;
        }
    }
}
