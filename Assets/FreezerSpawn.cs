using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezerSpawn : MonoBehaviour
{
    public DropFridge dropFridge;
    public Text freezeText;
    public float freezeCount;
    public GameObject Freezer;
    
    void Start()
    {
        dropFridge.OnDragEnd += FeezerSpawn;
        freezeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        freezeText.text = freezeCount.ToString();
    }
    public void FeezerSpawn(Vector3 pos)
    {
        if(freezeCount > 0)
        {
            Vector3 distance = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, 20, 0);
            Instantiate(Freezer, distance, Quaternion.identity);
            freezeCount--;
        }
      
    }
}
