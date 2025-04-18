using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shoot setting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float range = 20f;
    [Header("Sound Effects")]
    public AudioClip shootSound; // 发射音效
    public AudioSource audioSource; // 音频源组件

    private float nextFireTime = 0f;
    private bool isShootingEnabled = true;

    void Start()
    {
        // 如果没有指定AudioSource，尝试获取组件
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Update()
    {
        if (isShootingEnabled && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Bullet preform or firing point not set!");
            return;
        }

        // 播放发射音效
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullets are missing Rigidbody components!");
        }
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(range, 1);
        }
    }

    public void SetShootingEnabled(bool enabled)
    {
        isShootingEnabled = enabled;
    }
}