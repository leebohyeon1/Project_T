using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;

    [BoxGroup("Camera Setting"), LabelText("���� ��� ī�޶� ������")]
    public Vector3 builngModeCameraOffset = new Vector3(0,2.5f,0);
    [BoxGroup("Camera Setting"), LabelText("ī�޶� �̵� �ð�")]
    public float changeTime = 0.4f;

    private bool isBuildingMode = false; // ���� ������� ���� ����
    private Vector3 defaultOffset;       // �⺻ ������
    private Coroutine currentCoroutine; // ���� ���� ���� �ڷ�ƾ�� ����



    private void Start()
    {
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

        defaultOffset = composer.m_TrackedObjectOffset; 
    }


    public void CameraMove(bool isBuildingModeActive)
    {
        if (isBuildingMode == isBuildingModeActive)
        {
            return;
        }
        else
        { 
            isBuildingMode = isBuildingModeActive;

            // ���� �ڷ�ƾ�� ���� ���̸� ����
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            if (isBuildingModeActive)
            {
                // �������� �ε巴�� �����ϴ� �ڷ�ƾ ȣ��
                currentCoroutine = StartCoroutine(SmoothChangeOffset(builngModeCameraOffset, 0.3f)); // 1�� ���� ����
            }
            else
            {
                // �������� �⺻������ �ε巴�� �����ϴ� �ڷ�ƾ ȣ��
                currentCoroutine = StartCoroutine(SmoothChangeOffset(defaultOffset, 0.3f)); // 1�� ���� ����
            }
        }
     
    }

    // �������� �ε巴�� �����ϴ� �ڷ�ƾ
    private IEnumerator SmoothChangeOffset(Vector3 targetOffset, float duration)
    {
        Vector3 initialOffset = composer.m_TrackedObjectOffset; // ���� �� ������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ���� ������ ����Ͽ� �������� ���������� ����
            composer.m_TrackedObjectOffset = Vector3.Lerp(initialOffset, targetOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // �������� ��Ȯ�ϰ� Ÿ�� ���������� ����
        composer.m_TrackedObjectOffset = targetOffset;
    }
}
