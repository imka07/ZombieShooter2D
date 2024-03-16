using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gun : MonoBehaviour
{
    [SerializeField] private int gunIndex;
    private MainUiController uiManager;
    [SerializeField] private Transform gunTransform;
    [SerializeField] AnimationCurve animCurve;

    [SerializeField] private GameplaySettings gameplaySettings;
    void Start()
    {
        uiManager = FindObjectOfType<MainUiController>();
        transform.DOMoveY(-0.9f, 1f).SetEase(Ease.InOutSine).SetLoops(13, LoopType.Yoyo);
        StartCoroutine(GunAnim());
        GetComponent<SpriteRenderer>().sprite = gameplaySettings.weaponSettings.weapons[gunIndex].weaponLootSprite;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 6.6f);
    }
    
    private float timer = 1f;
    private IEnumerator GunAnim()
    {
        while (true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                gunTransform.localPosition = new Vector3(0, animCurve.Evaluate(timer), 0);
            }
            else timer = 1;
            yield return null;
        }
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<GunController>().ChangeGun(gunIndex);
            uiManager.ChangeWeaponIcon(gunIndex);
            Destroy(gameObject);
        }
    }

}
