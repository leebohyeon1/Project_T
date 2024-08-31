using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerActionRecorder;

public class NPCReplayer : MonoBehaviour
{
    private List<PlayerAction> recordedActions;
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
    public void StartReplay(List<PlayerAction> actions)
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
            switch (action.Type)
            {
                case ActionType.Move:
                    transform.position = action.Position;
                    transform.rotation = action.Rotation;
                    break;
                case ActionType.PlaceObject:
                    GameObject placedObject = Instantiate(action.Prefab, action.Position, action.Rotation);
                    placedObject.GetComponent<Collider>().isTrigger = true;
                    changebjectPreviewColor(placedObject);
                    GameManager.Instance.objectClones.Add(placedObject);
                    break;
            }
            // ���� �ൿ���� �̵�
            currentActionIndex++;
        }
    }

    // ������Ʈ ���׸��� ����
    private void changebjectPreviewColor(GameObject placeObject)
    {
        Renderer[] objectRenderers = placeObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in objectRenderers)
        {
            renderer.materials = transform.GetComponent<Renderer>().materials; // ���׸��� ��ü
        }
    }
}
