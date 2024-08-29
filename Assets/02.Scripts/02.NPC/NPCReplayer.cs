using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReplayer : MonoBehaviour
{
    private List<PlayerActionRecorder.PlayerAction> recordedActions;
    private int currentActionIndex = 0;
    private bool isReplaying = false;

    public void StartReplay(List<PlayerActionRecorder.PlayerAction> actions)
    {
        recordedActions = actions;
        currentActionIndex = 0;
        isReplaying = true;
        StartCoroutine(ReplayCoroutine());
    }

    private IEnumerator ReplayCoroutine()
    {
        while (isReplaying && currentActionIndex < recordedActions.Count)
        {
            var action = recordedActions[currentActionIndex];
            transform.position = action.Position;
            transform.rotation = action.Rotation;

            // 애니메이션 재생 (필요시 애니메이터 설정)
            // Example: animator.Play(action.AnimationState);

            if (currentActionIndex < recordedActions.Count - 1)
            {
                float waitTime = recordedActions[currentActionIndex + 1].TimeStamp - action.TimeStamp;
                yield return new WaitForSeconds(waitTime);
            }
            currentActionIndex++;
        }

        Destroy(gameObject); // 모든 행동이 끝나면 NPC 오브젝트 제거
    }
}
