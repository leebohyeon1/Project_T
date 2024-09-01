using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [LabelText("ü��")]
    public int health = 100; // ��ֹ��� ü��

    public void TakeDamage(int damage)
    {
        Debug.Log(name + "��(��) ���ݹ޾ҽ��ϴ�.");

        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // ��ֹ� �ı�
        }
    }
}