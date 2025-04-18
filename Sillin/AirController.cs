using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AirController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;      // 前后左右移动的速度
    public float rotationSpeed = 100f; // 旋转速度
    public float climbSpeed = 3f;      // 上下移动的速度

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
        
        // 前后移动（W和S键）
        float verticalInput = Input.GetAxis("Vertical"); // W -> forward, S -> backward
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);

        // 左右旋转（A和D键）
        float horizontalInput = Input.GetAxis("Horizontal"); // A -> left, D -> right
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

        // 上下移动（空格键 -> 上升，左Ctrl键 -> 下降）
        float climbInput = 0f;
        if (Input.GetKey(KeyCode.Space)) climbInput = 1f;   // 空格键 -> 上升
        if (Input.GetKey(KeyCode.LeftControl)) climbInput = -1f;  // 左Ctrl键 -> 下降

        // 应用上下移动
        transform.Translate(Vector3.up * climbInput * climbSpeed * Time.deltaTime);
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
