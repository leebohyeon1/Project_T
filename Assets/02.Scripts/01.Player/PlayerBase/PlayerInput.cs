using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector ���ӽ����̽� �߰�

public class PlayerInput : MonoBehaviour
{
    [BoxGroup("Input Settings"), LabelText("�̵� �Է�")]
    public Vector2 moveInput; // �̵� �Է��� ������ ����

    [BoxGroup("Input Settings"), LabelText("���� �Է�")]
    public bool jumpInput; // ���� �Է��� ������ ����

    protected virtual void Update()
    {
        // �Է� ó��
        ProcessInputs();
    }

    protected virtual void ProcessInputs()
    {
        // �̵� �Է� �ޱ� (WASD �Ǵ� ȭ��ǥ Ű)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // ���� �Է� �ޱ� (�����̽���)
        jumpInput = Input.GetButtonDown("Jump");

     
    }
}
