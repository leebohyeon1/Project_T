using System.Collections.Generic;
using UnityEngine;

public class PlayerActionRecorder : MonoBehaviour
{
    public class PlayerAction
    {
        public Vector3 Position;      // 플레이어의 위치
        public Quaternion Rotation;   // 플레이어의 회전
        public float TimeStamp;       // 행동이 기록된 시간
    }

    private List<PlayerAction> actions = new List<PlayerAction>(); // 녹화된 행동 리스트
    private bool isRecording = false;  // 현재 녹화 중인지 여부
    private float startTime; // 녹화 시작 시간

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

    // 행동을 기록하는 메서드
    private void RecordAction()
    {
        var currentAction = new PlayerAction
        {
            Position = transform.position,
            Rotation = transform.rotation,
            TimeStamp = Time.time - startTime // 경과 시간 사용
        };

        actions.Add(currentAction);
    }

    // 녹화를 시작하는 메서드
    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
    }

    // 녹화를 중지하는 메서드
    public void StopRecording()
    {
        isRecording = false;
    }

    // 녹화 데이터를 초기화하는 메서드
    public void ResetRecording()
    {
        actions.Clear();
        isRecording = false;
    }


    // 녹화된 행동 리스트를 반환하는 메서드
    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions);
    }

}
