using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fly : ZombieAI
{

    [SerializeField] AnimationCurve animCurve;
    private float timer = 1f;


    void Update()
    {
        if (gameManager.instance.isGameActive)
        {
            Attack();
            MoveToBunker();
        }
    }

    private void Start()
    {
        Init();
        StartCoroutine(GunAnim());
    }

    private IEnumerator GunAnim()
    {
        while (true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                // Изменяем масштаб объекта по оси Y с использованием анимационной кривой
                transform.localScale = new Vector3(animCurve.Evaluate(timer), animCurve.Evaluate(timer), 2);
            }
            else
            {
                timer = 1;
            }
            yield return null;
        }
    }

}
