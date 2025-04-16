using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    public float invincibilityTime = 1f;
    private float invincibilityTimer;
    private bool isInvincible;

    private Transform waterTransform;
    private float waterWidth;
    private float waterLength;

    void Start()
    {
        currentHealth = maxHealth;
        ScoreManager.Instance?.UpdateHealthDisplay(currentHealth);

        GameObject waterObject = GameObject.Find("Water");
        if (waterObject != null)
        {
            waterTransform = waterObject.transform;
            MeshFilter meshFilter = waterObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                waterWidth = mesh.bounds.size.x * waterTransform.localScale.x;
                waterLength = mesh.bounds.size.z * waterTransform.localScale.z;
            }
        }
    }

    void Update()
    {
        // 无敌时间倒计时
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f) isInvincible = false;
        }

        // 移动控制
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

        // 限制移动范围
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, waterTransform.position.x - waterWidth / 2, waterTransform.position.x + waterWidth / 2);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, waterTransform.position.z - waterLength / 2, waterTransform.position.z + waterLength / 2);
        transform.position = clampedPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1);
            Destroy(collision.gameObject); // 碰撞后消灭敌人
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isInvincible = true;
        invincibilityTimer = invincibilityTime;
        ScoreManager.Instance?.UpdateHealthDisplay(currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}