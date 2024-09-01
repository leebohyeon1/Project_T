using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Player
{
    private EngineerInput engineerInput;
    private EngineerActionRecorder actionRecorder;
    private EngineerBuilder builder;

    private List<List<EngineerActionRecorder.PlayerAction>> allRecordedActions = new List<List<EngineerActionRecorder.PlayerAction>>();
    private List<GameObject> actionClones = new List<GameObject>();

    [FoldoutGroup("CloneSetting"), LabelText("클론 프리팹")]
    public CloneReplayer npcReplayerPrefab; // NPCReplayer 프리팹 참조

    protected override void Awake()
    {
        base.Awake();
        engineerInput = GetComponent<EngineerInput>();
        actionRecorder = GetComponent<EngineerActionRecorder>();
        builder = GetComponent<EngineerBuilder>();
    }

    protected override void Update()
    {
        base.Update();

        // 점프 입력 처리
        playerMovement.Jump(engineerInput.jumpInput);

        builder.HandleBuildingInput(engineerInput.isBuildingModeActive, engineerInput.placeObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 이동 입력 처리
        playerMovement.Move(engineerInput.moveInput);
    }

    public override void ResetPlayer()
    {
        base.ResetPlayer();
        actionRecorder.ResetRecording();
        actionRecorder.StartRecording();

        foreach (GameObject clone in actionClones) // 분신 제거
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        actionClones.Clear();

        foreach (GameObject clone in builder.GetObjectClones()) //설치한 오브젝트 제거
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        builder.GetObjectClones().Clear();


        foreach (var recordedActions in allRecordedActions) // 분신 생성
        {
            CreateNPC(recordedActions);
        }
    }

    public override void Die()
    {
        base.Die();
        // 플레이어의 행동을 저장
        allRecordedActions.Add(actionRecorder.GetRecordedActions());
        actionRecorder.StopRecording();
    }


    private void CreateNPC(List<EngineerActionRecorder.PlayerAction> actions)
    {
        CloneReplayer npcInstance = Instantiate(npcReplayerPrefab, transform.position, Quaternion.identity);
        actionClones.Add(npcInstance.gameObject);
        npcInstance.SetEngineer(builder);
        npcInstance.StartReplay(actions);
    }
}
