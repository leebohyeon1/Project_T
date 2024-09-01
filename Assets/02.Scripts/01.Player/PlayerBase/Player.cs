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

    public virtual void ResetPlayer() { } // 게임 시작 또는 재시작할 때 호출

    public virtual void Die() { } // 죽는 순간 호출
}
