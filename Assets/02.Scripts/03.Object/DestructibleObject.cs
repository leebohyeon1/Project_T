using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [LabelText("체력"), SerializeField]
    protected int health = 100; // 장애물의 체력

    public virtual void TakeDamage(int damage)
    {
        Debug.Log(name + "이(가) 공격받았습니다.");

        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // 장애물 파괴
        }
    }
}
