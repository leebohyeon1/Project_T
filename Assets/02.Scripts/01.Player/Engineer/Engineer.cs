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

    [FoldoutGroup("CloneSetting"), LabelText("Ŭ�� ������")]
    public CloneReplayer npcReplayerPrefab; // NPCReplayer ������ ����

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

        // ���� �Է� ó��
        playerMovement.Jump(engineerInput.jumpInput);

        builder.HandleBuildingInput(engineerInput.isBuildingModeActive, engineerInput.placeObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // �̵� �Է� ó��
        playerMovement.Move(engineerInput.moveInput);
    }

    public override void ResetPlayer()
    {
        base.ResetPlayer();
        actionRecorder.ResetRecording();
        actionRecorder.StartRecording();

        foreach (GameObject clone in actionClones) // �н� ����
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        actionClones.Clear();

        foreach (GameObject clone in builder.GetObjectClones()) //��ġ�� ������Ʈ ����
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        builder.GetObjectClones().Clear();


        foreach (var recordedActions in allRecordedActions) // �н� ����
        {
            CreateNPC(recordedActions);
        }
    }

    public override void Die()
    {
        base.Die();
        // �÷��̾��� �ൿ�� ����
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
