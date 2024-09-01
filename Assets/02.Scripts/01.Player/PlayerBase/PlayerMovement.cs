using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Movement Settings"), LabelText("이동 속도 설정")]
    public float moveSpeed = 5f;

    [BoxGroup("Jump Settings"), LabelText("점프 힘 설정")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings"), LabelText("중력 설정")]
    public float gravity = -9.81f;

    [BoxGroup("Jump Settings"), LabelText("땅 체크를 위한 레이어 마스크")]
    public LayerMask groundLayer;

    private CharacterController characterController;
    private Vector3 velocity;
    private Transform cameraTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // 메인 카메라의 Transform 가져오기
    }

    public void Move(Vector2 moveInput)
    {
        // 카메라 기준으로 이동 방향 설정
        Vector3 moveDirection = (cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x).normalized;
        moveDirection.y = 0f; // 수직 이동 방지

        // 수평 이동 속도 설정
        Vector3 horizontalMovement = moveDirection * moveSpeed;

        // 최종 이동 벡터를 중력에 따른 수직 이동과 수평 이동을 합산하여 계산
        Vector3 finalMovement = horizontalMovement + velocity;

        // 이동 처리
        characterController.Move(finalMovement * Time.deltaTime);
    }

    public void Jump(bool jumpInput)
    {
        // 지면 체크: CharacterController는 IsGrounded로 지면 체크 가능
        bool isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; // 지면에 있을 때 속도를 약간 낮게 설정하여 떨어지는 상태 유지
            }

            if (jumpInput)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
    }
}
