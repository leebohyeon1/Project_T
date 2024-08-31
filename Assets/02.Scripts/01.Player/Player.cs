using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerActionRecorder actionRecorder;
    private PlayerBuilder playerBuilder;    

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        actionRecorder = GetComponent<PlayerActionRecorder>();
        playerBuilder = GetComponent<PlayerBuilder>();
    }

    void Update()
    {
        // 점프 입력 처리
        playerMovement.Jump(playerInput.jumpInput);

        playerBuilder.HandleBuildingInput(playerInput.isBuildingModeActive, playerInput.placeObject);
    }

    void FixedUpdate()
    {
        // 이동 입력 처리
        playerMovement.Move(playerInput.moveInput);
    }

}
