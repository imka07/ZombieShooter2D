using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Test : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float jumpforce = 1f;
    public float health;
    public float maxHealth = 100;
    private bool isGrounded = false;
    public bool speedUp, buffJump, armor, med, dron = false;
    public float timeBtSpawn;

    [Header("Player Components")]
    [SerializeField] GameObject aura;
    Rigidbody2D rb;
    private Animator ch_animator;
    [SerializeField] private AudioSource moneyTake;
    private SpriteRenderer[] ch_sprites;
    public AudioSource lootSound;
    [SerializeField] private GameplaySettings gameplaySettings;

    private MainUiController MUC;
    //private bool isWall = false;
    public delegate void OnHpChangeHandler(float maxHp, float currentHp);
    public OnHpChangeHandler OnHpChange;
    public GameObject medButton;
    public int genAmmo { get; private set; } = 0;
    private Vector3 moveVector;
    [SerializeField] private GameObject tnt;
    [SerializeField] private Transform tntSpawn;
    public GameObject Dron;
    public GameObject dronSpawn;


    void Start()
    {
        MUC = FindObjectOfType<MainUiController>();
        rb = GetComponent<Rigidbody2D>();
        ch_animator = GetComponent<Animator>();
        ch_animator.SetFloat("Fast", 1f);
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
   
   
    private void Grenade()
    {
        Vector3 targetPositionScreen = Input.mousePosition;
        Vector3 targetPositionWorld = Camera.main.ScreenToWorldPoint(targetPositionScreen);
        targetPositionWorld.z = 0f;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameManager.instance.tntCount > 0)
            {
                tntController tntController =  Instantiate(tnt, transform.position, Quaternion.identity).GetComponent<tntController>();
                tntController.Throw(tntSpawn.position, targetPositionWorld);
                gameManager.instance.tntCount--;
            }
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

    public void FullHp()
    {
        health = Mathf.Max(health + maxHealth, 0);
        OnHpChange?.Invoke(maxHealth, health);
    }
    
    //public void HealButton()
    //{

    //    if (cash >= 50 && med == false)
    //    {
    //        med = true;
    //        Heal(50);
    //        MUC.StartMed();
    //        cash -= 50;
    //    }
       
    //}
    //public void DronButton()
    //{
    //    if(cash >= 100 && dron == false)
    //    {
    //        dron = true;
    //        Instantiate(Dron, dronSpawn.transform.position, Quaternion.identity);
    //        MUC.StartDron();
    //        cash -= 100;
    //    }
       
    //}
    public void TakeDamage(float damage)
    {

        if(!armor)
        {
            health = Mathf.Max(health - damage, 0);
            OnHpChange?.Invoke(maxHealth, health);
            DamageEffect();
            if(health == 0)
            {
                gameManager.instance.GameOver();
            }
        }
    }

    public void HealtChange(float ammount)
    {
        maxHealth = Mathf.Min(maxHealth + ammount);
        OnHpChange?.Invoke(maxHealth, health);
        health = maxHealth;
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

    void FixedUpdate()
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

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (gameManager.instance.isGameActive)
        {
            ch_animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * gameplaySettings.playerMaxMoveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
            ch_animator.SetFloat("Speed", 0);
        }

        if (gameManager.instance.isGameActive) Grenade();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!buffJump)
            {
                rb.velocity = Vector2.up * jumpforce;
            }
            else
            {
                rb.velocity = Vector2.up * jumpforce * 2;
            }
        }


        //if (Input.GetKeyDown(KeyCode.Space) && isWall)
        //{
        //    if (Physics2D.Raycast(transform.position, transform.right, 1))
        //    {
        //        rb.AddForce((transform.up - transform.right) * jumpforce * 1.5f);
        //    }
        //    else if (Physics2D.Raycast(transform.position, -transform.right, 1))
        //    {
        //        rb.AddForce((transform.up + transform.right) * jumpforce * 1.5f);
        //    }
        //}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

            //С помощью tag проверяет Ground , если да то правда
            if (collision.transform.tag == "Ground")
            {
                isGrounded = true;
                ch_animator.SetBool("OnGrounded", true);
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

            //Проверятеся , если не на земле , то неправда
            if (collision.transform.tag == "Ground")
            {
                isGrounded = false;
                ch_animator.SetBool("OnGrounded", false);

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
        
    }
}


