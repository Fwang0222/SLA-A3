using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Effects")]
    public TrailRenderer trailEffect;
    public GameObject hitEffectPrefab;

    private float range;
    private int damage;
    private Vector3 startPosition;

    public void Initialize(float range, int damage)
    {
        this.range = range;
        this.damage = damage;
        this.startPosition = transform.position;
        
        if (trailEffect != null)
        {
            trailEffect.Clear();
            trailEffect.enabled = true;
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            DestroyBullet();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == transform.parent) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                SpawnHitEffect(transform.position);
            }
            DestroyBullet();
        }
    }

    void SpawnHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void DestroyBullet()
    {
        if (trailEffect != null)
        {
            trailEffect.transform.parent = null;
            Destroy(trailEffect.gameObject, trailEffect.time);
        }
        Destroy(gameObject);
    }
}