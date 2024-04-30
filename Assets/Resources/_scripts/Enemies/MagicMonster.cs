using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMonster : ZombieAI
{
    Base bunkerController;
    [SerializeField] private GameObject fireBall;

    void Start()
    {
        Init();
        bunkerController = FindObjectOfType<Base>();
    }


    public override void Attack()
    {
        if (canAttack)
        {
            if (timeBetweenAttack <= 0)
            {
                PlayClips(1);
                anim.SetTrigger("attack");
                timeBetweenAttack = startBetweenAttack;
            }
            else
            {
                timeBetweenAttack -= Time.deltaTime;
            }
        }
    }

    public override void OnAttack()
    {
        Instantiate(fireBall, attackPos.position, Quaternion.identity);
    }

    void Update()
    {
        float distanceToBuker = Vector2.Distance(transform.position, bunkerController.transform.position);

        if (gameManager.instance.isGameActive)
        {
            if (distance < distanceToBuker + 9.601265f)
            {
                MoveToBunker();
            }
            else
            {
                canAttack = true;
                Attack();
            }
        }
    }
}
