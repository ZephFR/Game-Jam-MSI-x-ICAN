using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject ballPrefab;       
    public Transform firePoint;         
    public float shootForce = 20f;      

    [Header("Cadence de tir")]
    public float fireRate = 3f;       
    public bool autoFire = true;

    private float nextFireTime;

    private void Update()
    {
        if (autoFire && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void Shoot()
    {
        if (ballPrefab == null || firePoint == null) return;

        
        GameObject ball = Instantiate(ballPrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);
        }
    }
}