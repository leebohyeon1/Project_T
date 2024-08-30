using System.Collections.Generic;
using UnityEngine;

public class PlayerActionRecorder : MonoBehaviour
{
    public class PlayerAction
    {
        public Vector3 Position;      // �÷��̾��� ��ġ
        public Quaternion Rotation;   // �÷��̾��� ȸ��
        public float TimeStamp;       // �ൿ�� ��ϵ� �ð�
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
            RecordAction();
        }
    }

    // �ൿ�� ����ϴ� �޼���
    private void RecordAction()
    {
        var currentAction = new PlayerAction
        {
            Position = transform.position,
            Rotation = transform.rotation,
            TimeStamp = Time.time - startTime // ��� �ð� ���
        };

        actions.Add(currentAction);
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


    // ��ȭ�� �ൿ ����Ʈ�� ��ȯ�ϴ� �޼���
    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions);
    }

}
