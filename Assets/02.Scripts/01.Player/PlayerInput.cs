using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector ���ӽ����̽� �߰�

public class PlayerInput : MonoBehaviour
{
    [BoxGroup("Input Settings"), LabelText("�̵� �Է�")]

    public Vector2 moveInput; // �̵� �Է��� ������ ����

    [BoxGroup("Input Settings"), LabelText("���� �Է�")]
    public bool jumpInput; // ���� �Է��� ������ ����

    void Update()
    {
        // �Է� ó��
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        // �̵� �Է� �ޱ� (WASD �Ǵ� ȭ��ǥ Ű)
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        // ���� �Է� �ޱ� (�����̽���)
        jumpInput = Input.GetButtonDown("Jump");
    }
}
