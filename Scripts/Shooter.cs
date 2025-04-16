using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shoot setting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float range = 20f;

    private float nextFireTime = 0f;
    private bool isShootingEnabled = true;

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