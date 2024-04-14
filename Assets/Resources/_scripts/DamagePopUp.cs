using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;

    public float fadeSpeed = 0.5f; 

    private float currentAlpha = 1.0f;


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(float damageAmount)
    {
        textMesh.SetText(damageAmount.ToString("0"));
    }

    private void Update()
    {
        float moveSpeedY = 15f;
        transform.position += new Vector3(0, moveSpeedY) * Time.deltaTime;
        currentAlpha -= fadeSpeed * Time.deltaTime;

        currentAlpha = Mathf.Clamp01(currentAlpha);

        textMesh.alpha = currentAlpha;

        if (currentAlpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
