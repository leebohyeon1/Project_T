using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // 점프 입력 처리
        playerMovement.Jump(playerInput.jumpInput);
    }

    void FixedUpdate()
    {
        // 이동 입력 처리
        playerMovement.Move(playerInput.moveInput);
    }
}
