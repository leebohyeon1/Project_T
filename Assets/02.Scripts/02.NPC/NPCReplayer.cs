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

    // ���÷��̸� �����ϴ� �޼���
    public void StartReplay(List<PlayerActionRecorder.PlayerAction> actions)
    {
        if (actions == null || actions.Count == 0) return;

        recordedActions = actions;
        currentActionIndex = 0;
        isReplaying = true;
        replayStartTime = Time.time;
    }

    // �ൿ�� ����ϴ� �޼���
    private void ReplayAction()
    {
        if (currentActionIndex >= recordedActions.Count)
        {
            isReplaying = false;
            Destroy(gameObject);
            return;
        }

        // ���� �ൿ ��������
        var action = recordedActions[currentActionIndex];

        // ���� �ð��� ��� ���� �ð��� ���� ���
        float elapsedTime = Time.time - replayStartTime;

        // ���� ���� �ൿ�� Ÿ�ӽ������� ����� �ð����� �۰ų� ������ �ൿ ���
        if (action.TimeStamp <= elapsedTime)
        {
            transform.position = action.Position;
            transform.rotation = action.Rotation;

            // ���� �ൿ���� �̵�
            currentActionIndex++;
        }
    }
}
