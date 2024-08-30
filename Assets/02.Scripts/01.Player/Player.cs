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
        // ���� �Է� ó��
        playerMovement.Jump(playerInput.jumpInput);
    }

    void FixedUpdate()
    {
        // �̵� �Է� ó��
        playerMovement.Move(playerInput.moveInput);
    }
}
