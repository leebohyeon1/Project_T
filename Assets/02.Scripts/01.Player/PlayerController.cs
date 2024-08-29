using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector 네임스페이스 추가

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [BoxGroup("Movement Settings")] // Odin Inspector로 그룹화
    [Tooltip("이동 속도 설정")]
public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [Tooltip("회전 속도 설정")]
    public float rotationSpeed = 700f;

    [BoxGroup("Jump Settings")]
    [Tooltip("점프 힘 설정")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings")]
    [Tooltip("땅 체크를 위한 레이어 마스크")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 물리 기반 회전을 막고 코드로만 회전 제어
        cameraTransform = Camera.main.transform; // 메인 카메라의 Transform 가져오기
    }

    void Update()
    {
        Rotate();
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 카메라 기준으로 이동 방향 설정
        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;
        moveDirection.y = 0f; // 수직 이동 방지

        // 이동 속도 설정
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // 플레이어 회전
        Vector3 rotation = new Vector3(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    private void Jump()
    {
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.1f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
