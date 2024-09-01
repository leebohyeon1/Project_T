using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected PlayerMovement playerMovement;

    protected virtual void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected virtual void Update()
    {
       
    }

    protected virtual void FixedUpdate()
    {
       
    }

    public virtual void ResetPlayer() { } // ���� ���� �Ǵ� ������� �� ȣ��

    public virtual void Die() { } // �״� ���� ȣ��
}
