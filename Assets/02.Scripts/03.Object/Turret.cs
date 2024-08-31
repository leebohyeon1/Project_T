using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [BoxGroup("Turret Settings"), LabelText("탐지 범위")]
    public float detectionRange = 20f;  // 탐지 범위
    [BoxGroup("Turret Settings"), LabelText("회전 속도")]
    public float rotationSpeed = 5f;    // 회전 속도
    [BoxGroup("Turret Settings"), LabelText("공격 속도")]
    public float fireRate = 1f;         // 공격 속도 (초당 발사 횟수)
    [BoxGroup("Turret Settings")]
    public GameObject bulletPrefab;     // 총알 프리팹
    [BoxGroup("Turret Settings")]
    public Transform firePoint;         // 총알이 발사될 위치
    [BoxGroup("Turret Settings")]
    public Transform turretHead;        // 터렛 머리

    [BoxGroup("Pitch Constraints")]
    public float minPitchAngle = -45f;  // 터렛의 최소 피치 각도
    [BoxGroup("Pitch Constraints")]
    public float maxPitchAngle = 30f;   // 터렛의 최대 피치 각도

    [BoxGroup("Bullet Settings"), LabelText("총알 속도")]
    public float bulletSpeed = 20f;     // 총알 속도

    [BoxGroup("Turret Settings"), LabelText("적 레이어"), Space(10f)]
    public LayerMask enemyLayer;        // 적 레이어

    private Transform target;           // 현재 타겟
    private float fireCooldown;         // 발사 간격을 위한 쿨다운 타이머

    void Update()
    {
        DetectClosestEnemy();
        RotateTowardsTarget();

        if (target != null)
        {
            Attack();
        }
    }

    // 가장 가까운 적 탐지
    void DetectClosestEnemy()
    {
        // 적 레이어로 설정된 오브젝트 탐지
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

        target = closestEnemy;  // 가장 가까운 적을 타겟으로 설정
    }

    // 타겟을 향해 터렛 회전
    void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 현재 회전 각도를 EulerAngles로 가져옴
        Vector3 currentEulerAngles = turretHead.rotation.eulerAngles;
        Vector3 targetEulerAngles = lookRotation.eulerAngles;

        // 피치(pitch) 각도를 제한하는 부분
        float pitchAngle = targetEulerAngles.x;
        if (pitchAngle > 180f) pitchAngle -= 360f; // 피치 각도를 -180 ~ 180 범위로 조정
        pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);

        // 최종 회전 설정: Yaw는 목표 각도, Pitch는 제한된 각도, Roll은 0으로 고정
        Quaternion clampedRotation = Quaternion.Euler(pitchAngle, targetEulerAngles.y, 0f);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, clampedRotation, Time.deltaTime * rotationSpeed);
    }


    // 공격 메커니즘
    void Attack()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    // 총알 발사
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.speed = bulletSpeed;  // 총알의 속도를 터렛에서 설정한 값으로 변경
            }
        }
    }

    // 터렛의 시야 범위를 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
