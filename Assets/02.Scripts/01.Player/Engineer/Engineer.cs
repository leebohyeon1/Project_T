using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static EngineerActionRecorder;

public class Engineer : Player
{
    private CharacterController characterController;
    private Animator animator;
    private EngineerInput engineerInput;
    private EngineerActionRecorder actionRecorder;
    private EngineerBuilder builder;
    private EngineerCamera engineerCamera;


    private List<List<EngineerActionRecorder.PlayerAction>> allRecordedActions = new List<List<EngineerActionRecorder.PlayerAction>>();

    private List<GameObject> actionClones = new List<GameObject>();

    [FoldoutGroup("CloneSetting"), LabelText("Ŭ�� ������")]
    public CloneReplayer npcReplayerPrefab; // NPCReplayer ������ ����



    protected override void Awake()
    {
        base.Awake();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        engineerInput = GetComponent<EngineerInput>();
        actionRecorder = GetComponent<EngineerActionRecorder>();
        builder = GetComponent<EngineerBuilder>();
        engineerCamera = GetComponent<EngineerCamera>();
    }



    protected override void Update()
    {
        base.Update();

        Jump();

        Building();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
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


        for(int i  = 0; i < allRecordedActions.Count; i++)
        {
            CreateNPC(allRecordedActions[i]);// �н� ����
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
        actionClones.Add(npcInstance.gameObject); // Ŭ�� ����Ʈ�� ������Ʈ �߰�
        npcInstance.SetEngineer(builder);         // ������ ����
        npcInstance.StartReplay(actions);         // �÷��̾� �ൿ ���� �ѱ��
    }

    private void Building()
    {
        // ���� ��� �Է�
        builder.HandleBuildingInput(engineerInput.isBuildingModeActive, engineerInput.placeObject);

        // ���� ��� �ִϸ��̼�
        if (engineerInput.isBuildingModeActive && engineerInput.placeObject)
        {
            animator.SetTrigger("Place");
        }

        // ���� ��� ī�޶� ������ ����
        engineerCamera.CameraMove(engineerInput.isBuildingModeActive);
    }

    private void Move()
    {
        // �̵� �Է� ó��
        playerMovement.Move(engineerInput.moveInput);

        // �̵� �ִϸ��̼�
        if (engineerInput.moveInput.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("xDir", engineerInput.moveInput.x);
            animator.SetFloat("zDir", engineerInput.moveInput.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Jump()
    {
        // ���� �Է� ó��
        playerMovement.Jump(engineerInput.jumpInput);

        if(engineerInput.jumpInput && characterController.isGrounded)
        {
            animator.SetTrigger("Jump");

            // ���� ���
            actionRecorder.RecordJump(transform.position, transform.rotation);
        }
       
    }
}
