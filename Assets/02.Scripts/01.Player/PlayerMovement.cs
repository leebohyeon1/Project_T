using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector 네임스페이스 추가

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Movement Settings"), LabelText("이동 속도 설정")]
    public float moveSpeed = 5f;

    [BoxGroup("Jump Settings"), LabelText("점프 힘 설정")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings"), LabelText("땅 체크를 위한 레이어 마스크")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping; // 점프 상태 추적
    private bool isTouchingWall; // 벽 접촉 상태 추적
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 물리 기반 회전을 막고 코드로만 회전 제어
        cameraTransform = Camera.main.transform; // 메인 카메라의 Transform 가져오기
    }

    public void Move(Vector2 moveInput)
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        // 카메라 기준으로 이동 방향 설정
        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;
        moveDirection.y = 0f; // 수직 이동 방지

        // 이동 속도 설정
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
  
    }

    public void Jump(bool jumpInput)
    {
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.1f, groundLayer);

        if (isGrounded && jumpInput)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
