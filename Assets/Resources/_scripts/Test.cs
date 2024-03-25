using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Test : MonoBehaviour
{
    [SerializeField] DragAndDrop tntDrop;
    [SerializeField] GameObject aura;
    //public GameObject lootAir;
    private MainUiController MUC;
    Rigidbody2D rb;
    [SerializeField] private float jumpforce = 1f;
    private float force;
	private bool isGrounded = false;
    private bool isWall = false;
    private bool lockControls = false;
    public bool speedUp, buffJump, armor, med, dron = false;
    private Animator ch_animator;
    private float health;
    public float maxHealth = 100;
    public Text moneyText;
    [SerializeField] private AudioSource moneyTake;
    public  float cash;
    public delegate void OnHpChangeHandler(float maxHp, float currentHp);
    public OnHpChangeHandler OnHpChange;
    public GameObject darkness;
    public GameObject medButton;
    public GameObject diePannel;
    private SpriteRenderer[] ch_sprites;
    public static int DarkNumber;
    public int genAmmo { get; private set; } = 0;
    private Vector3 moveVector;
    [SerializeField] private GameObject tnt;
    [SerializeField] private GameObject tntSpawn;
    public int tntCount;
    [SerializeField] private Text tntCountText;
    public GameObject variant;
    public AudioSource lootSound;
    public GameObject Spawnvariant;
    [SerializeField] private GameplaySettings gameplaySettings;
    public float timeBtSpawn;
    public GameObject Dron;
    public GameObject dronSpawn;
    void Start()
    {
        if(cash < 0)
        {
            cash = 0;
        }
        cash = 250f;
        force = 25f;
        cash = PlayerPrefs.GetFloat("cash");
        tntCount = 0;
        diePannel.SetActive(false);
        MUC = FindObjectOfType<MainUiController>();
        rb = GetComponent<Rigidbody2D>();
        ch_animator = GetComponent<Animator>();
        ch_animator.SetFloat("Fast", 1f);
        health = maxHealth;
        tntDrop.OnDragEnd += Tnt;
        ch_sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void ArmorSetActive(bool active)
    {

        aura.SetActive(active);
    }
  
    public void MedButtonSetActive(bool active)
    {
        medButton.SetActive(active);
    }
   
    
    public void OffDarkness()
    {

        if(DarkNumber == 1)
        {
            darkness.SetActive(true);
        }
        if (DarkNumber == 2)
        {
            darkness.SetActive(false);
        }
    }
    public void Tnt(Vector3 pos)
    {
        if(tntCount > 0)
        {
            tntController _tntController = Instantiate(tnt, transform.position, Quaternion.identity).GetComponent<tntController>();
            _tntController.Throw(transform.position, Camera.main.ScreenToWorldPoint(pos));
            tntCount--;
        }
        
    }

    private void DamageEffect()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = new Color(1, 0.5f, 0.5f);
        }
        Invoke("SetWhiteColor", 0.05f);
    }

    private void SetWhiteColor()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = Color.white;
        }
    }
    
    public void HealButton()
    {

        if (cash >= 50 && med == false)
        {
            med = true;
            Heal(50);
            MUC.StartMed();
            cash -= 50;
        }
       
    }
    public void DronButton()
    {
        if(cash >= 100 && dron == false)
        {
            dron = true;
            Instantiate(Dron, dronSpawn.transform.position, Quaternion.identity);
            MUC.StartDron();
            cash -= 100;
        }
       
    }
    public void TakeDamage(float damage)
    {   // Если это не броня , то сносим жизни и делаем эффект от дамага , и ставим жизни на 0
        if(!armor)
        {
            health = Mathf.Max(health - damage, 0);
            OnHpChange?.Invoke(maxHealth, health);
            DamageEffect();
            if(health == 0)
            {
                Death();
            }
        }
    }

    public void Heal(float heal)
    {
        // это у нас аптека повышается жизни
        FindObjectOfType<Base>().Heal(50);
        health = Mathf.Min(health + heal, maxHealth);
        OnHpChange?.Invoke(maxHealth, health);
    }

    public void Death()
    {
        diePannel.SetActive(true);
        Time.timeScale = 0;
    }


    public void StopSpeedUp()
    {
        speedUp = false;
        ch_animator.SetFloat("Fast", 1);
    }
    public void StopBuffJump()
    {
        buffJump = false;
    }
    public void StopMed()
    {
        med = false;
    }
    public void StopDron()
    {
        dron = false;
    }
    public void StopArmor()
    {
        armor = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!lockControls)
        {
            if (!isWall)
            {
                rb.AddForce(moveVector);
                if (speedUp)
                {
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -gameplaySettings.playerMaxMoveSpeed - gameplaySettings.speedBonus, gameplaySettings.playerMaxMoveSpeed + gameplaySettings.speedBonus), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -gameplaySettings.playerMaxMoveSpeed, gameplaySettings.playerMaxMoveSpeed), rb.velocity.y);
                }

            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(-2, rb.velocity.y));
            }
        }
    }

    private void Update()
    {
        
       
        tntCountText.text = tntCount.ToString();
        moneyText.text = cash.ToString();
        //1.Задает направление и силу , которая действует на персонажа ,за счет чего увеличивая скорость.
        //2. Присваевает скорость, потом идет ограничивание тела в скорости, но скорость не меняется,будет только быстрее доходит до пределов , заданных с помощью Math.Clamp
        // Проверяется , если это не стена , то выполняем обычное передвижение, а если на стене , то идет заторможение по y

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Вычисляем направление движения
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * gameplaySettings.playerMaxMoveSpeed * Time.deltaTime;

        // Применяем движение к игроку
        transform.Translate(movement);

        OffDarkness();
        if (!lockControls)
        {
            //Проверяем черз if нажата ли кнопка пробел и персонаж на земле, потом задаем направление и умножаюм на силу прыжка
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                if (!buffJump)
                {
                    rb.velocity = Vector2.up * force;
                }
                else
                {
                    rb.velocity = Vector2.up * force * 2;
                }
            }


            // Проверяется через if нажат ли пробел, и,если это стена, то
            // Через if проверяется луч Physics2D.Raycast на правой ли стене персонаж,если да , то задаем направление вверх и минусуем вектор вправо, чтобы тело отскакивало в другую сторону,потом умножаем на jumpforce ,который в свою очередь увеличен в полтора раза.
            // И наоборот, если персонаж - правой стороне , то заадем направление вверх и плюсуем вектор вправо, потом умножаем уже увеличенную в 1,5 раза jumpforce
            if (Input.GetKeyDown(KeyCode.Space) && isWall)
            {
                if (Physics2D.Raycast(transform.position, transform.right, 1))
                {
                    rb.AddForce((transform.up - transform.right) * jumpforce * 1.5f);
                }
                else if (Physics2D.Raycast(transform.position, -transform.right, 1))
                {
                    rb.AddForce((transform.up + transform.right) * jumpforce * 1.5f);
                }
            }

            ch_animator.SetFloat("Speed", Input.GetAxisRaw("Horizontal") == 0 ? 0 : 1);

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                ch_animator.SetFloat("Speed", 0);
            }
            else
            {
                ch_animator.SetFloat("Speed", 1);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!lockControls)
        {
            //С помощью tag проверяет Ground , если да то правда
            if (collision.transform.tag == "Ground")
            {
                isGrounded = true;
                ch_animator.SetBool("OnGrounded", true);
            }
            // Проверяется tag, если это стена, то правда
            if (collision.transform.tag == "Wall")
            {
                isWall = true;

            }
           
        }
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!lockControls)
        {
            //Проверятеся , если не на земле , то неправда
            if (collision.transform.tag == "Ground")
            {
                isGrounded = false;
                ch_animator.SetBool("OnGrounded", false);

            }
            // Проверяется tag, если это не стена, то неправда
            if (collision.transform.tag == "Wall")
            {
                isWall = false;

            }
           
        }
        if (collision.gameObject.tag == "airDrop")
        {
            //lootAir.SetActive(false);
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpeedUp")
        {
            speedUp = true;
            Destroy(collision.gameObject);
            ch_animator.SetFloat("Fast", 2f);
            MUC.StartSpeedUp();
        }
        if (collision.tag == "Jump")
        {
            buffJump = true;
            Destroy(collision.gameObject);
            MUC.StartBuffJump();
        }
        if (collision.tag == "Armor")
        {
            armor = true;
            Destroy(collision.gameObject);
            MUC.StartArmor();
        }
       

        if (collision.tag == "Medicine")
        {
            
            Heal(50);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "money")
        {
            PlayerPrefs.SetFloat("cash", cash);
            cash += 20;
            moneyTake.Play();
            Destroy(collision.gameObject);
        }

      
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!lockControls)
        {
            if (collision.tag == "Door")
            {
                if (Input.GetKey(KeyCode.F))
                {
                    collision.GetComponent<DoorController>().In();
                }
            }
        }
    }

    public void LockControls()
    {
        lockControls = true;
        rb.velocity = Vector2.zero;
        ch_animator.SetFloat("Speed", 0);
    }

    public void UnlockControls()
    {
        lockControls = false;
    }


}


