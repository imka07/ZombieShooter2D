using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Transform player;
    [SerializeField] GameObject targetDoor;
    [SerializeField] SpriteRenderer doorDark;
    public GameObject key;
   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        key.SetActive(false);
       
    }

    void Update()
    {
       
    }

    public void In()
    {
        StartCoroutine(InCoroutine());
    }

    public void Out()
    {
        StartCoroutine(OutCoroutine());
    }

    private IEnumerator InCoroutine()
    {
        //создаем переменную для прозрачности
        float alpha = 0;

        //перемещаем персонажа в центр двери, в которую заходим, и блокируем движение перса
        player.transform.position = transform.position;
        player.GetComponent<Test>().LockControls();

        //плавно меняем прозрачность (от 0 до 1)
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            doorDark.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        //перемещаем персонажа в центр двери, из которой выходим
        player.transform.position = targetDoor.transform.position;

        //делаем обратно дверь прозрачной
        alpha = 0;
        doorDark.color = new Color(1, 1, 1, alpha);

        //запускаем функцию Out в двери, из которой выходим
        targetDoor.GetComponent<DoorController>().Out();
    }

    private IEnumerator OutCoroutine()
    {
        //создаем переменную для прозрачности
        float alpha = 1;

        //разблокируем перса
        player.GetComponent<Test>().UnlockControls();

        //плавно меняем прозрачность (от 1 до 0)
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 2;
            doorDark.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            key.SetActive(true);
            targetDoor.GetComponent<DoorController>().key.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            key.SetActive(false);
            targetDoor.GetComponent<DoorController>().key.SetActive(false);
        }
       
       
    }
}
