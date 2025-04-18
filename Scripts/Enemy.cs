using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Effects")]
    public GameObject deathEffectPrefab;

    [Header("Health")]
    public int health = 1;

    [Header("Score")]
    public int scoreValue = 1; // 每个怪物的得分，可在 Inspector 中设置

    // 被攻击调用
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // 死亡逻辑
    void Die()
    {
        // 加分（通过 ScoreManager）
        ScoreManager.Instance?.AddScore(scoreValue);

        // 播放死亡特效
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // 销毁敌人对象
        Destroy(gameObject);
    }
}
