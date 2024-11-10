using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;  // NavMeshAgent를 사용해 경로를 계산하고 이동
    protected Animator animator;

    public enum EnemyState
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    [BoxGroup("AI Setting"), LabelText("타겟")]
    public Transform target;       // 추적할 대상, 주로 플레이어
    [BoxGroup("AI Setting"),LabelText("장애물 레이어"), SerializeField]
    protected LayerMask ObstacleLayer; // 장애물 레이어

    [BoxGroup("Enemy Setting"), LabelText("체력")]
    public int health = 10;
    [BoxGroup("Enemy Setting"), LabelText("이동속도"), Space(10f)]
    public float moveSpeed = 3.5f; // 적의 이동 속도
    [BoxGroup("Enemy Setting"), LabelText("공격 범위")]
    public float attackRange = 3f;
    [BoxGroup("Enemy Setting"), LabelText("공격 속도")]
    public float attackSpeed = 1f;
    [BoxGroup("Enemy Setting"), LabelText("공격력")]
    public int attackDamage = 3;
    protected float attackCooldown; // 공격 쿨다운 타이머

    protected EnemyState enemyState = EnemyState.Trace;

    protected  Collider targetCollider;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        FindTarget();
    }

    protected virtual void Update()
    {
        switch(enemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Trace:
                FindTarget();

                if (target != null)
                {
                    agent.SetDestination(target.position); // 목표 지점(플레이어 위치)으로 이동
                }
                break;
        }
       
    }

    protected virtual void FindTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Target").transform;
            if (target == null)
            {
                enemyState = EnemyState.Idle;
                agent.isStopped = true;
            }
            targetCollider = target.GetComponent<Collider>();
        }
    }

    // 에이전트와 목표 오브젝트의 경계 간 최소 거리 계산 함수
    protected float DistanceToTargetBoundary()
    {
        // 에이전트 위치와 목표 오브젝트 콜라이더의 ClosestPoint 계산
        Vector3 closestPoint = targetCollider.ClosestPoint(agent.transform.position);

        // 에이전트와 가장 가까운 지점 간의 거리 계산
        float distance = Vector3.Distance(agent.transform.position, closestPoint);
        return distance;
    }

    public virtual void TakeDamage(int damage, Player player = null)
    {
        health -= damage;
        if (health <= 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                return;
            animator.SetTrigger("Die"); // 죽는 애니메이션 실행
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                return;
            animator.SetTrigger("GetHit");
        }
    }

    public virtual void Die() // 애니메이션 이벤트
    {
        Destroy(gameObject);
    }

    public virtual void StopEnemy()
    {

    }
}
