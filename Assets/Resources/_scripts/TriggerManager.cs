using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    BossController bossController;

    private void Start()
    {
        bossController = FindObjectOfType<BossController>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bossController.isAttacking) // �������� ������������ � �������
        {
            bossController.StartAttackiing();
            Destroy(gameObject);
        }
    }
}
