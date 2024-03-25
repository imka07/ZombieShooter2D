using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] GameObject ricohetPrefab;
    [SerializeField] GameObject hitPrefab;
    public float speed = 50;
    public float damage = 15;
    public bool right;
    private float timeToDestroy = 10;

    private Vector3 previousPosition, currentPosition;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = !right;
    }

    void Update()
    {   // время жизни пули зависит от реального времени
        timeToDestroy -= Time.deltaTime;
        // если время меньше заданного , то уничтожить обьект
        if (timeToDestroy <= 0) Destroy(gameObject);
        // тут мы в начале присваеваем прошлую позицию как обычную позицию 
        previousPosition = transform.position;
        // Тут мы спрашиваем это право?Если да то вектор вправо это правда, а вектор влево это неправда,это строка отвечает за передвижение пули по горизонтали , умноженная на скорость и на реальное время
        transform.Translate((right ? Vector3.right : Vector3.left) * speed * Time.deltaTime);
        // потом текущая позиция становится нашей основной позицией
        currentPosition = transform.position;
        // тут мы запускаем Рэйкаст , задаем прошлую и текущиую позицию и высчитываем дистанцию между ними
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, currentPosition, Vector3.Distance(previousPosition, currentPosition));
        // идет проверка , если это Hit и тэг Player, то наносим урон , и дальше как мы делали с GunController
        if (hit)
        {
            switch (hit.transform.tag)
            {
                case "Player":
                    hit.transform.GetComponent<Test>().TakeDamage(damage);
                    Instantiate(hitPrefab, hit.point, Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case "bunker":
                {
                  hit.transform.GetComponent<Base>().TakeDamage(damage);
                        Destroy(gameObject);
                        break;
                }

                default:
                    Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    break;
            }
        }
    }
}
