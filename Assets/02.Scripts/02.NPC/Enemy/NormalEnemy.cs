using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static NormalEnemy;

public class NormalEnemy : EnemyBase
{

    protected override void Update()
    {
        switch(enemyState)
        {
            case EnemyState.Trace:
                base.Update();
                agent.isStopped = false;
                
                // ������Ʈ�� ��ΰ� ��Ȯ���� �ʰų� ���� ���
                if (agent.pathStatus == NavMeshPathStatus.PathPartial ||
                    agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    target = FindNearObstacle();
                    if (target != null)
                    {
                        targetCollider = target.GetComponent<Collider>();
                    }
                }

                // ������Ʈ�� ��θ� ��� ���� �ƴϰ� ��ΰ� ��ȿ���� Ȯ��
                if (!agent.pathPending && agent.hasPath)
                {
                    // ������Ʈ�� ��ǥ ��� ������ �Ÿ� ���
                    float distanceToBoundary = DistanceToTargetBoundary();

                    // ��ǥ ��迡 �������� �� ������Ʈ ���߱�
                    if (distanceToBoundary <= attackRange)
                    {
                        enemyState = EnemyState.Attack;
                        attackCooldown = 2f / attackSpeed;
                    }
                }
                break;

            case EnemyState.Attack:
                agent.isStopped = true;
                Attack();
                break;
        }
     
    }

    // �ֺ��� ���� ����� ��ֹ� ã��
    private Transform FindNearObstacle()
    {
        GameObject nearObstacle = null; 

        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity, ObstacleLayer);

        float minDistance = Mathf.Infinity;

        foreach (Collider collider in colliders) //���� ����� ��ֹ� ã��
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearObstacle = collider.gameObject;
            }
        }

        if (nearObstacle != null)
        {
            return nearObstacle.transform;
        }
        return null; // ��ó�� ��ֹ��� ���� ��� null ��ȯ
    }

    // ���� 
    private void Attack()
    {
        if (target == null)
        {
            enemyState = EnemyState.Trace; // Ÿ���� ���� �� ���� ���·� ��ȯ
            return;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackCooldown = 2f / attackSpeed;

            target.GetComponent<DestructibleObject>().TakeDamage(attackDamage);
        }
    }

    public override void TakeDamage(int damage, Player player = null)
    {
        //�÷��̾�� ���ݴ��ϸ� ��Ž�� ���� �߰� ����

        base.TakeDamage(damage);
    }

    // ������ Ž�� ����, ���� ���� ǥ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ���� ����
    }
}


