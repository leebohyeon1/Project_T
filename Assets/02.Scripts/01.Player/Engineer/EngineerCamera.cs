using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;

    [BoxGroup("Camera Setting"), LabelText("건축 모드 카메라 오프셋")]
    public Vector3 builngModeCameraOffset = new Vector3(0,2.5f,0);
    [BoxGroup("Camera Setting"), LabelText("카메라 이동 시간")]
    public float changeTime = 0.4f;

    private bool isBuildingMode = false; // 빌딩 모드인지 이차 검증
    private Vector3 defaultOffset;       // 기본 오프셋
    private Coroutine currentCoroutine; // 현재 실행 중인 코루틴의 참조



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

            // 기존 코루틴이 실행 중이면 중지
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            if (isBuildingModeActive)
            {
                // 오프셋을 부드럽게 변경하는 코루틴 호출
                currentCoroutine = StartCoroutine(SmoothChangeOffset(builngModeCameraOffset, 0.3f)); // 1초 동안 변경
            }
            else
            {
                // 오프셋을 기본값으로 부드럽게 변경하는 코루틴 호출
                currentCoroutine = StartCoroutine(SmoothChangeOffset(defaultOffset, 0.3f)); // 1초 동안 변경
            }
        }
     
    }

    // 오프셋을 부드럽게 변경하는 코루틴
    private IEnumerator SmoothChangeOffset(Vector3 targetOffset, float duration)
    {
        Vector3 initialOffset = composer.m_TrackedObjectOffset; // 시작 시 오프셋
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 선형 보간을 사용하여 오프셋을 점진적으로 변경
            composer.m_TrackedObjectOffset = Vector3.Lerp(initialOffset, targetOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막에 정확하게 타겟 오프셋으로 설정
        composer.m_TrackedObjectOffset = targetOffset;
    }
}
