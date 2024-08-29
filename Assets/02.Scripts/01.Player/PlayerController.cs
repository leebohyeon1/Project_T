using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector ���ӽ����̽� �߰�

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [BoxGroup("Movement Settings")] // Odin Inspector�� �׷�ȭ
    [Tooltip("�̵� �ӵ� ����")]
public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [Tooltip("ȸ�� �ӵ� ����")]
    public float rotationSpeed = 700f;

    [BoxGroup("Jump Settings")]
    [Tooltip("���� �� ����")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings")]
    [Tooltip("�� üũ�� ���� ���̾� ����ũ")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ���� ��� ȸ���� ���� �ڵ�θ� ȸ�� ����
        cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������
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

        // ī�޶� �������� �̵� ���� ����
        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;
        moveDirection.y = 0f; // ���� �̵� ����

        // �̵� �ӵ� ����
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // �÷��̾� ȸ��
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
