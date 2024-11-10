using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : DestructibleObject
{
    private int defaultHealth = 0;

    private void Start()
    {
        defaultHealth = health;    
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(name + "��(��) ���ݹ޾ҽ��ϴ�.");

        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ReStartGame()
    {
        health = defaultHealth;
        gameObject.SetActive(true);
    }
}
