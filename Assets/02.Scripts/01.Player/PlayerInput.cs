using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector ���ӽ����̽� �߰�

public class PlayerInput : MonoBehaviour
{
    [BoxGroup("Input Settings"), LabelText("�̵� �Է�")]
    public Vector2 moveInput; // �̵� �Է��� ������ ����

    [BoxGroup("Input Settings"), LabelText("���� �Է�")]
    public bool jumpInput; // ���� �Է��� ������ ����

    [BoxGroup("Input Settings"), LabelText("�Ǽ� ���")]
    public bool isBuildingModeActive;

    [BoxGroup("Input Settings"), LabelText("��ġ �Է�")]
    public bool placeObject;

    void Update()
    {
        // �Է� ó��
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        // �̵� �Է� �ޱ� (WASD �Ǵ� ȭ��ǥ Ű)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // ���� �Է� �ޱ� (�����̽���)
        jumpInput = Input.GetButtonDown("Jump");

        // �Ǽ� ��� Ȱ��ȭ / ��Ȱ��ȭ �Է�
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingModeActive = !isBuildingModeActive;
        }

        // �ͷ� ��ġ �Է�
        placeObject = Input.GetMouseButtonDown(0);
    }
}
