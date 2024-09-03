using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;  // NavMeshAgent�� ����� ��θ� ����ϰ� �̵�
    protected Animator animator;

    public enum EnemyState
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    [BoxGroup("AI Setting"), LabelText("Ÿ��")]
    public Transform target;       // ������ ���, �ַ� �÷��̾�
    [BoxGroup("AI Setting"),LabelText("��ֹ� ���̾�"), SerializeField]
    protected LayerMask ObstacleLayer; // ��ֹ� ���̾�

    [BoxGroup("Enemy Setting"), LabelText("ü��")]
    public int health = 10;
    [BoxGroup("Enemy Setting"), LabelText("�̵��ӵ�"), Space(10f)]
    public float moveSpeed = 3.5f; // ���� �̵� �ӵ�
    [BoxGroup("Enemy Setting"), LabelText("���� ����")]
    public float attackRange = 3f;
    [BoxGroup("Enemy Setting"), LabelText("���� �ӵ�")]
    public float attackSpeed = 1f;
    [BoxGroup("Enemy Setting"), LabelText("���ݷ�")]
    public int attackDamage = 3;
    protected float attackCooldown; // ���� ��ٿ� Ÿ�̸�

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
                    agent.SetDestination(target.position); // ��ǥ ����(�÷��̾� ��ġ)���� �̵�
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

    // ������Ʈ�� ��ǥ ������Ʈ�� ��� �� �ּ� �Ÿ� ��� �Լ�
    protected float DistanceToTargetBoundary()
    {
        // ������Ʈ ��ġ�� ��ǥ ������Ʈ �ݶ��̴��� ClosestPoint ���
        Vector3 closestPoint = targetCollider.ClosestPoint(agent.transform.position);

        // ������Ʈ�� ���� ����� ���� ���� �Ÿ� ���
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
            animator.SetTrigger("Die"); // �״� �ִϸ��̼� ����
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                return;
            animator.SetTrigger("GetHit");
        }
    }

    public virtual void Die() // �ִϸ��̼� �̺�Ʈ
    {
        Destroy(gameObject);
    }

    public virtual void StopEnemy()
    {

    }
}
