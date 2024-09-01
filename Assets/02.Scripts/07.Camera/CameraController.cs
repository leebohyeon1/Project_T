using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Cinemachine Virtual Camera ����
    public float mouseSensitivity = 100f; // ���콺 ���� ����
    public Transform playerBody; // �÷��̾� Transform

    private float xRotation = 0f; // ���� ȸ���� ���� ����
    private CinemachineTransposer transposer; // ī�޶��� Transposer ����

    void Start()
    {
        if(virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
         
        }
        playerBody = GameObject.FindGameObjectWithTag("Player").transform;

        virtualCamera.LookAt = playerBody;
        virtualCamera.Follow = playerBody;

        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ��� ���·� ����

        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // ���콺 �Է� �޾ƿ���
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ���� ī�޶� ���� ������Ʈ
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 70f); // ���� ȸ�� ������ -30���� 70���� ����

        // �÷��̾��� �¿� ȸ��
        playerBody.Rotate(Vector3.up * mouseX);

        // ī�޶��� ���� ȸ�� ����
        Vector3 followOffset = transposer.m_FollowOffset;
        followOffset.y = Mathf.Tan(Mathf.Deg2Rad * xRotation) * Mathf.Abs(followOffset.z);
        transposer.m_FollowOffset = followOffset;
    }
}
