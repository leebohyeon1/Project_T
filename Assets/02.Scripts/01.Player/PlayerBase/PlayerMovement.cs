using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Movement Settings"), LabelText("�̵� �ӵ� ����")]
    public float moveSpeed = 5f;

    [BoxGroup("Jump Settings"), LabelText("���� �� ����")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings"), LabelText("�߷� ����")]
    public float gravity = -9.81f;

    [BoxGroup("Jump Settings"), LabelText("�� üũ�� ���� ���̾� ����ũ")]
    public LayerMask groundLayer;

    private CharacterController characterController;
    private Vector3 velocity;
    private Transform cameraTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������
    }

    public void Move(Vector2 moveInput)
    {
        // ī�޶� �������� �̵� ���� ����
        Vector3 moveDirection = (cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x).normalized;
        moveDirection.y = 0f; // ���� �̵� ����

        // ���� �̵� �ӵ� ����
        Vector3 horizontalMovement = moveDirection * moveSpeed;

        // ���� �̵� ���͸� �߷¿� ���� ���� �̵��� ���� �̵��� �ջ��Ͽ� ���
        Vector3 finalMovement = horizontalMovement + velocity;

        // �̵� ó��
        characterController.Move(finalMovement * Time.deltaTime);
    }

    public void Jump(bool jumpInput)
    {
        // ���� üũ: CharacterController�� IsGrounded�� ���� üũ ����
        bool isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; // ���鿡 ���� �� �ӵ��� �ణ ���� �����Ͽ� �������� ���� ����
            }

            if (jumpInput)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;
    }
}
