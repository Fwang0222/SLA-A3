using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Effects")]
    public GameObject deathEffectPrefab;
    public int health = 1;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        ScoreManager.Instance?.AddScore(1);
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}