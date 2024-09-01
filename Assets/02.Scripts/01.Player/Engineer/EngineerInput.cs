using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerInput : PlayerInput
{
    [BoxGroup("Input Settings"), LabelText("건설 모드")]
    public bool isBuildingModeActive;

    [BoxGroup("Input Settings"), LabelText("설치 입력")]
    public bool placeObject;

    protected override void Update()
    {
        base.Update();        
    }

    protected override void ProcessInputs()
    {
        base.ProcessInputs();

        // 건설 모드 활성화 / 비활성화 입력
        if (Input.GetMouseButtonDown(1))
        {
            isBuildingModeActive = !isBuildingModeActive;
        }

        // 설치 입력
        placeObject = Input.GetMouseButtonDown(0);
    }
}
