using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Tooltip("�Ѿ��� �ӵ�")]
    public float speed = 20f;  // �Ѿ��� �⺻ �ӵ�

    [Tooltip("�Ѿ��� ���� (��)")]
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

    // ���� �浹 �� ó��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // �� �±׷� ������ ������Ʈ���� ����
        {
            // ������ ���ظ� �ְų� �ٸ� ȿ���� �߰��� �� ����
            Destroy(other.gameObject); // ���÷� ���� �ı�
            Destroy(gameObject); // �Ѿ˵� �ı�
        }
    }
}
