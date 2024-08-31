using UnityEngine;
using Sirenix.OdinInspector; // Odin Inspector 네임스페이스 추가

public class PlayerInput : MonoBehaviour
{
    [BoxGroup("Input Settings"), LabelText("이동 입력")]
    public Vector2 moveInput; // 이동 입력을 저장할 변수

    [BoxGroup("Input Settings"), LabelText("점프 입력")]
    public bool jumpInput; // 점프 입력을 저장할 변수

    [BoxGroup("Input Settings"), LabelText("건설 모드")]
    public bool isBuildingModeActive;

    [BoxGroup("Input Settings"), LabelText("설치 입력")]
    public bool placeObject;

    void Update()
    {
        // 입력 처리
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        // 이동 입력 받기 (WASD 또는 화살표 키)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 점프 입력 받기 (스페이스바)
        jumpInput = Input.GetButtonDown("Jump");

        // 건설 모드 활성화 / 비활성화 입력
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingModeActive = !isBuildingModeActive;
        }

        // 터렛 설치 입력
        placeObject = Input.GetMouseButtonDown(0);
    }
}
