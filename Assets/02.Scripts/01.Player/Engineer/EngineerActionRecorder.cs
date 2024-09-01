using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerActionRecorder : MonoBehaviour
{
    public enum ActionType
    {
        Move,
        Rotate,
        Jump,
        PlaceObject  // ������Ʈ ��ġ �׼� �߰�
    }

    [System.Serializable]
    public class PlayerAction
    {
        public ActionType Type;        // �ൿ�� ����
        public Vector3 Position;       // �÷��̾��� ��ġ �Ǵ� ������Ʈ ��ġ ��ġ
        public Quaternion Rotation;    // �÷��̾��� ȸ�� �Ǵ� ������Ʈ ��ġ ȸ��
        public string FunctionName;    // ȣ���� �Լ� �̸�
        public GameObject Prefab;      // ��ġ�� ������Ʈ ������
        public float TimeStamp;        // �ൿ�� ��ϵ� �ð�
    }

    private List<PlayerAction> actions = new List<PlayerAction>(); // ��ȭ�� �ൿ ����Ʈ
    private bool isRecording = false;  // ���� ��ȭ ������ ����
    private float startTime; // ��ȭ ���� �ð�

    void Start()
    {
        StartRecording();
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            // �⺻ �ൿ�� �ڵ����� ���
            RecordAction(ActionType.Move, transform.position, transform.rotation);
        }
    }

    // ��ȭ�� �����ϴ� �޼���
    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
    }

    // ��ȭ�� �����ϴ� �޼���
    public void StopRecording()
    {
        isRecording = false;
    }

    // ��ȭ �����͸� �ʱ�ȭ�ϴ� �޼���
    public void ResetRecording()
    {
        actions.Clear();
        isRecording = false;
    }

    // �ڵ� ��ȭ �޼���
    private void RecordAction(ActionType type, Vector3 position, Quaternion rotation, string functionName = null, GameObject prefab = null)
    {
        var currentAction = new PlayerAction
        {
            Type = type,
            Position = position,
            Rotation = rotation,
            FunctionName = functionName,
            Prefab = prefab,
            TimeStamp = Time.time - startTime
        };

        actions.Add(currentAction);
    }

    // ������Ʈ ��ġ ��ȭ �޼���
    public void RecordPlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        RecordAction(ActionType.PlaceObject, position, rotation, null, prefab);
    }

    // ��ȭ�� �ൿ ����Ʈ�� ��ȯ�ϴ� �޼���
    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions);
    }

}
