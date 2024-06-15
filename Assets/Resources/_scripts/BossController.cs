using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : ZombieAI
{
    public ParticleSystem groundedEffect;

    void Start()
    {
        Init();
        // Передаем ссылку на Particle System в StateMachineBehaviour
        var bossFalls = anim.GetBehaviour<BoosFalls>();
        bossFalls.groundedEffect = groundedEffect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayClips(0);
            collision.gameObject.GetComponent<Test>().TakeDamage(collisionDamage);
        }
    }
}
