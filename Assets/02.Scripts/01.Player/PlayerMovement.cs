using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector ���ӽ����̽� �߰�

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Movement Settings"), LabelText("�̵� �ӵ� ����")]
    public float moveSpeed = 5f;

    [BoxGroup("Jump Settings"), LabelText("���� �� ����")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings"), LabelText("�� üũ�� ���� ���̾� ����ũ")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping; // ���� ���� ����
    private bool isTouchingWall; // �� ���� ���� ����
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ���� ��� ȸ���� ���� �ڵ�θ� ȸ�� ����
        cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������
    }

    public void Move(Vector2 moveInput)
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        // ī�޶� �������� �̵� ���� ����
        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;
        moveDirection.y = 0f; // ���� �̵� ����

        // �̵� �ӵ� ����
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
