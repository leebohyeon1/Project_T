using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [BoxGroup("Turret Settings"), LabelText("Ž�� ����")]
    public float detectionRange = 20f;  // Ž�� ����
    [BoxGroup("Turret Settings"), LabelText("ȸ�� �ӵ�")]
    public float rotationSpeed = 5f;    // ȸ�� �ӵ�
    [BoxGroup("Turret Settings"), LabelText("���� �ӵ�")]
    public float fireRate = 1f;         // ���� �ӵ� (�ʴ� �߻� Ƚ��)
    [BoxGroup("Turret Settings")]
    public GameObject bulletPrefab;     // �Ѿ� ������
    [BoxGroup("Turret Settings")]
    public Transform firePoint;         // �Ѿ��� �߻�� ��ġ
    [BoxGroup("Turret Settings")]
    public Transform turretHead;        // �ͷ� �Ӹ�

    [BoxGroup("Pitch Constraints")]
    public float minPitchAngle = -45f;  // �ͷ��� �ּ� ��ġ ����
    [BoxGroup("Pitch Constraints")]
    public float maxPitchAngle = 30f;   // �ͷ��� �ִ� ��ġ ����

    [BoxGroup("Bullet Settings"), LabelText("�Ѿ� �ӵ�")]
    public float bulletSpeed = 20f;     // �Ѿ� �ӵ�

    [BoxGroup("Turret Settings"), LabelText("�� ���̾�"), Space(10f)]
    public LayerMask enemyLayer;        // �� ���̾�

    private Transform target;           // ���� Ÿ��
    private float fireCooldown;         // �߻� ������ ���� ��ٿ� Ÿ�̸�

    void Update()
    {
        DetectClosestEnemy();
        RotateTowardsTarget();

        if (target != null)
        {
            Attack();
        }
    }

    // ���� ����� �� Ž��
    void DetectClosestEnemy()
    {
        // �� ���̾�� ������ ������Ʈ Ž��
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var collider in colliders)
        {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider.transform;
                }
        }

        target = closestEnemy;  // ���� ����� ���� Ÿ������ ����
    }

    // Ÿ���� ���� �ͷ� ȸ��
    void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // ���� ȸ�� ������ EulerAngles�� ������
        Vector3 currentEulerAngles = turretHead.rotation.eulerAngles;
        Vector3 targetEulerAngles = lookRotation.eulerAngles;

        // ��ġ(pitch) ������ �����ϴ� �κ�
        float pitchAngle = targetEulerAngles.x;
        if (pitchAngle > 180f) pitchAngle -= 360f; // ��ġ ������ -180 ~ 180 ������ ����
        pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);

        // ���� ȸ�� ����: Yaw�� ��ǥ ����, Pitch�� ���ѵ� ����, Roll�� 0���� ����
        Quaternion clampedRotation = Quaternion.Euler(pitchAngle, targetEulerAngles.y, 0f);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, clampedRotation, Time.deltaTime * rotationSpeed);
    }


    // ���� ��Ŀ����
    void Attack()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    // �Ѿ� �߻�
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.speed = bulletSpeed;  // �Ѿ��� �ӵ��� �ͷ����� ������ ������ ����
            }
        }
    }

    // �ͷ��� �þ� ������ �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
