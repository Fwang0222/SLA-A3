using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float stickToGroundForce = 5f; // 强制贴地的力度
    public float groundCheckDistance = 0.5f; // 地面检测距离
    public LayerMask groundLayer; // 地面层级

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    public float invincibilityTime = 1f;
    private float invincibilityTimer;
    private bool isInvincible;

    [Header("Damage Effects")]
    public AudioClip damageSound;
    private AudioSource audioSource;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

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

        // 屏幕红色逐渐消失
        damageImage.color = Color.Lerp(damageImage.color, originalColor, flashSpeed * Time.deltaTime);

        // 移动输入
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // 旋转
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

        // 计算移动方向（基于当前朝向）
        Vector3 moveDirection = transform.forward * verticalInput;
        
        // 直接通过Transform移动（避免物理干扰）
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 强制贴地逻辑
        StickToGround();
    }

    void StickToGround()
{
    RaycastHit hit;
    // 射线起点略微抬高（避免从模型内部检测）
    Vector3 rayStart = transform.position + Vector3.up * 0.2f; 
    
    if (Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance, groundLayer))
    {
        // 仅当角色离地面超过阈值时，才调整位置（避免每帧强制贴地）
        float distanceToGround = hit.distance;
        if (distanceToGround > 0.05f)
        {
            transform.position = hit.point + Vector3.up * 0.05f; // 微小偏移防止陷入地面
        }
    }
    // 移除 else 中的自动下沉逻辑（防止无限下坠）
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

        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        damageImage.color = flashColour;

        if (currentHealth <= 0)
        {
            GameUIManager uiManager = FindObjectOfType<GameUIManager>();
            if (uiManager != null)
            {
                uiManager.ShowLosePopup();
            }

            enabled = false; // 直接禁用脚本
        }
    }
}