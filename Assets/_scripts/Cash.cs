using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cash : MonoBehaviour
{

    private void Start()
    {
        transform.DOMoveY(-0.9f, 1f).SetEase(Ease.InOutSine).SetLoops(13, LoopType.Yoyo);
    }
    void Update()
    {
        Destroy(gameObject, 6f);
    }
    
}
