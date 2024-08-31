using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Tooltip("총알의 속도")]
    public float speed = 20f;  // 총알의 기본 속도

    [Tooltip("총알의 수명 (초)")]
    public float lifetime = 5f; // 총알이 사라지기 전까지의 시간

    private Rigidbody rb;       // Rigidbody 컴포넌트 참조
    private float lifeTimer;    // 총알의 수명 타이머

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        rb.velocity = transform.forward * speed; // 총알을 전방으로 발사
        lifeTimer = lifetime; // 수명 타이머 초기화
    }

    void Update()
    {
        // 수명이 다하면 총알 파괴
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 적과 충돌 시 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // 적 태그로 설정된 오브젝트에만 반응
        {
            // 적에게 피해를 주거나 다른 효과를 추가할 수 있음
            Destroy(other.gameObject); // 예시로 적을 파괴
            Destroy(gameObject); // 총알도 파괴
        }
    }
}
