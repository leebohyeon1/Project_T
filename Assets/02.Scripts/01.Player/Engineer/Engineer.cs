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

    [FoldoutGroup("CloneSetting"), LabelText("클론 프리팹")]
    public CloneReplayer npcReplayerPrefab; // NPCReplayer 프리팹 참조



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


        for(int i  = 0; i < allRecordedActions.Count; i++)
        {
            CreateNPC(allRecordedActions[i]);// 분신 생성
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
        actionClones.Add(npcInstance.gameObject); // 클론 리스트에 오브젝트 추가
        npcInstance.SetEngineer(builder);         // 생성자 설정
        npcInstance.StartReplay(actions);         // 플레이어 행동 정보 넘기기
    }

    private void Building()
    {
        // 건축 모드 입력
        builder.HandleBuildingInput(engineerInput.isBuildingModeActive, engineerInput.placeObject);

        // 건축 모드 애니메이션
        if (engineerInput.isBuildingModeActive && engineerInput.placeObject)
        {
            animator.SetTrigger("Place");
        }

        // 건축 모드 카메라 움직임 설정
        engineerCamera.CameraMove(engineerInput.isBuildingModeActive);
    }

    private void Move()
    {
        // 이동 입력 처리
        playerMovement.Move(engineerInput.moveInput);

        // 이동 애니메이션
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
        // 점프 입력 처리
        playerMovement.Jump(engineerInput.jumpInput);

        if(engineerInput.jumpInput && characterController.isGrounded)
        {
            animator.SetTrigger("Jump");

            // 점프 기록
            actionRecorder.RecordJump(transform.position, transform.rotation);
        }
       
    }
}
