using System.Collections.Generic;
using UnityEngine;

public class PlayerActionRecorder : MonoBehaviour
{
    public class PlayerAction
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string AnimationState;
        public float TimeStamp;
    }

    private List<PlayerAction> actions = new List<PlayerAction>();
    private Animator playerAnimator;
    private bool isRecording = true; // ���� ���� �� �⺻������ ��� ����

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isRecording)
        {
            RecordAction();
        }
    }

    private void RecordAction()
    {
        var currentAction = new PlayerAction
        {
            Position = transform.position,
            Rotation = transform.rotation,
            //AnimationState = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running") ? "Running" : "Idle", // ���� �ִϸ��̼� ����
            TimeStamp = Time.time
        };

        actions.Add(currentAction);
    }

    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions); // ����Ʈ ���纻 ��ȯ
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void ResetAction()
    {
        actions = new List<PlayerAction>();
        isRecording = true;
    }
}
