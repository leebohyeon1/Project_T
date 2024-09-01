using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerInput : PlayerInput
{
    [BoxGroup("Input Settings"), LabelText("�Ǽ� ���")]
    public bool isBuildingModeActive;

    [BoxGroup("Input Settings"), LabelText("��ġ �Է�")]
    public bool placeObject;

    protected override void Update()
    {
        base.Update();        
    }

    protected override void ProcessInputs()
    {
        base.ProcessInputs();

        // �Ǽ� ��� Ȱ��ȭ / ��Ȱ��ȭ �Է�
        if (Input.GetMouseButtonDown(1))
        {
            isBuildingModeActive = !isBuildingModeActive;
        }

        // ��ġ �Է�
        placeObject = Input.GetMouseButtonDown(0);
    }
}
