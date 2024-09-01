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
                
                // 에이전트의 경로가 정확하지 않거나 없는 경우
                if (agent.pathStatus == NavMeshPathStatus.PathPartial ||
                    agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    target = FindNearObstacle();
                    if (target != null)
                    {
                        targetCollider = target.GetComponent<Collider>();
                    }
                }

                // 에이전트가 경로를 계산 중이 아니고 경로가 유효한지 확인
                if (!agent.pathPending && agent.hasPath)
                {
                    // 에이전트와 목표 경계 사이의 거리 계산
                    float distanceToBoundary = DistanceToTargetBoundary();

                    // 목표 경계에 근접했을 때 에이전트 멈추기
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

    // 주변의 가장 가까운 장애물 찾기
    private Transform FindNearObstacle()
    {
        GameObject nearObstacle = null; 

        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity, ObstacleLayer);

        float minDistance = Mathf.Infinity;

        foreach (Collider collider in colliders) //가장 가까운 장애물 찾기
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
        return null; // 근처에 장애물이 없을 경우 null 반환
    }

    // 공격 
    private void Attack()
    {
        if (target == null)
        {
            enemyState = EnemyState.Trace; // 타겟이 없을 때 추적 상태로 전환
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
        //플레이어에게 공격당하면 재탐색 로직 추가 예정

        base.TakeDamage(damage);
    }

    // 기즈모로 탐지 범위, 공격 범위 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위
    }
}


