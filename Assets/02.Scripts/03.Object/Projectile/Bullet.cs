using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [BoxGroup("Setting"), LabelText("�Ѿ� ������")]
    public int damage = 5;
    [BoxGroup("Setting"), LabelText("�Ѿ� �ӵ�")]
    public float speed = 20f;  // �Ѿ��� �⺻ �ӵ�
    [BoxGroup("Setting"), LabelText("�Ѿ� ����(��)")]
    public float lifetime = 5f; // �Ѿ��� ������� �������� �ð�

    private Rigidbody rb;       // Rigidbody ������Ʈ ����
    private float lifeTimer;    // �Ѿ��� ���� Ÿ�̸�

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        rb.velocity = transform.forward * speed; // �Ѿ��� �������� �߻�
        lifeTimer = lifetime; // ���� Ÿ�̸� �ʱ�ȭ
    }

    void Update()
    {
        // ������ ���ϸ� �Ѿ� �ı�
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // �� �±׷� ������ ������Ʈ���� ����
        {
            // ������ ���ظ� �ְų� �ٸ� ȿ���� �߰��� �� ����
           EnemyBase enemyBase = other.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(damage);

       
            }

            Destroy(gameObject); // �Ѿ� �ı�
        }
       
    }

   
}
