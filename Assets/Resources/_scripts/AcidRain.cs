using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : MonoBehaviour
{
    private float timeBtSpawn;
    private float StarttimeBtSpawn;
    public GameObject acid;
    public GameObject PointOfacidSpawn;

    void Start()
    {
        Die();
        StarttimeBtSpawn = 0.8f;
    }
    private void Die()
    {
      
        Destroy(gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAcidDroupe();
    }
    private void SpawnAcidDroupe()
    {
        if (timeBtSpawn <= 0)
        {
            Instantiate(acid, PointOfacidSpawn.transform.position, Quaternion.identity);
            timeBtSpawn = StarttimeBtSpawn;
        }
        else
            timeBtSpawn -= Time.deltaTime;
    }
}
