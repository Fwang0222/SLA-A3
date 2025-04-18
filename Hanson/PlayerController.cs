using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Damage Effects")]
    public AudioClip damageSound; // 受伤害音效
    private AudioSource audioSource;
    public Image damageImage; // 屏幕变红的 Image
    public float flashSpeed = 5f; // 屏幕变红的闪烁速度
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // 屏幕变红的颜色

    private Color originalColor;

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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 将 damageImage 的颜色透明度设置为 0，使其初始时不可见
        originalColor = new Color(flashColour.r, flashColour.g, flashColour.b, 0f);
        damageImage.color = originalColor;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f) isInvincible = false;
        }

        // 让屏幕红色逐渐消失
        damageImage.color = Color.Lerp(damageImage.color, originalColor, flashSpeed * Time.deltaTime);

        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

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
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isInvincible = true;
        invincibilityTimer = invincibilityTime;
        ScoreManager.Instance?.UpdateHealthDisplay(currentHealth);

        // 播放受伤害音效
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // 屏幕变红
        damageImage.color = flashColour;

        if (currentHealth <= 0)
        {
            GameUIManager uiManager = FindObjectOfType<GameUIManager>();
            if (uiManager != null)
            {
                uiManager.ShowLosePopup();
            }

            GetComponent<PlayerController>().enabled = false;
            GetComponent<Shooter>().SetShootingEnabled(false);
        }
    }
}