using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(19f, 3f).SetEase(Ease.InOutSine).SetLoops(25, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
