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
    private bool isRecording = true; // 게임 시작 시 기본적으로 기록 시작

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
            //AnimationState = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running") ? "Running" : "Idle", // 예시 애니메이션 상태
            TimeStamp = Time.time
        };

        actions.Add(currentAction);
    }

    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions); // 리스트 복사본 반환
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
