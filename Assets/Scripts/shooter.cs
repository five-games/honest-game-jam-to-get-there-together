using UnityEngine;

public class shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float fireInterval = 1.5f;

    float nextFireTime;

    private void Awake()
    {
        if (spawnPoint == null) spawnPoint = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireInterval  > 0 && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
        }
    }

    public void Fire()
    {
        if (projectilePrefab == null) return;
        Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
    }
}
