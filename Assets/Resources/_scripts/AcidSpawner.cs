using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AcidSpawner : MonoBehaviour
{
    public DragAcidRain dropAcid;
    public Text acidRainText;
    public float AcidRainCount;
    public GameObject acidRain;

    void Start()
    {
        dropAcid.OnDragEnd += AcidRainSpawn;
        AcidRainCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        acidRainText.text = AcidRainCount.ToString();
    }
    public void AcidRainSpawn(Vector3 pos)
    {
        if (AcidRainCount > 0)
        {
            Vector3 distance = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(pos).y, 0);
            Instantiate(acidRain, distance, Quaternion.identity);
            AcidRainCount--;
        }

    }
}
