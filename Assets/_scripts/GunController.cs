using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class GunController : MonoBehaviour
{
    [SerializeField] ShakePreset shakePresetPistol;
    [SerializeField] GameObject bulletLauncher;
    [SerializeField] GameObject tracerPrefab;
    [SerializeField] GameObject LasertracerPrefab;
    [SerializeField] GameObject ricohetPrefab;
    [SerializeField] GameObject hitPrefab;
    [SerializeField] ParticleSystem fireFlash;
    [SerializeField] SpriteRenderer gunRenderer;
    public Test test;
    private float TimeShot;
    public float startTimeShoot;
    private float TimeShotofLaser;
    public float startTimeShootOfLaser;
    [SerializeField] AudioSource gunShoot;
    [SerializeField] AudioSource gunReload;
    public Transform barrel;
    public Rigidbody2D bullet;
    public event System.Action OnAmmoChange;
    public float rotateUp;
    public float rotateDown;
   
    public int genAmmo { get; private set; } = 0;
    public int maxAmmo { get; private set; }
    public int currentAmmo { get; private set; }
    public Transform player;
    [SerializeField] private GameplaySettings gameplaySettings;

    private bool canShoot = true;

    public int currentGunIndex { get; private set; } = 999;


    void Start()
    {
        gunShoot = GetComponent<AudioSource>();
        OnAmmoChange?.Invoke();
        ChangeGun(0);
    }

    void Update()
    {

        if ((Input.GetMouseButtonDown(0) && canShoot))
        {
            if (currentAmmo > 0)
            {
                switch (currentGunIndex)
                {
                    case 0:
                        PistolShoot();
                        break;
                    case 1:
                        AssaultRifleShoot();
                        break;
                    case 2:
                        GrenadeLauncgerShoot();
                        break;
                    case 3:
                        ShotGunShoot();
                        break;
                    case 5:
                        CrossBowShoot();
                        break;

                }

            }

        }
        LaserSetting();
        if (currentAmmo > 0)
        {
            if (currentGunIndex == 4)
            {
                if (TimeShot <= 0)
                {
                    if (Input.GetMouseButton(0))
                    {
                        UziShoot();
                        TimeShot = startTimeShoot;
                    }
                }
                else
                {
                    TimeShot -= Time.deltaTime;
                }

            }
           
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeGun(1);
        if (Input.GetKeyDown(KeyCode.R)) ReloadGun();
    }

    private void LaserSetting()
    {
        if (currentAmmo > 0)
        {
            if (currentGunIndex == 6)
            {
                if (TimeShotofLaser <= 0)
                {
                    if (Input.GetMouseButton(0))
                    {
                        LaserShoot();
                        TimeShotofLaser = startTimeShootOfLaser;
                    }
                }
                else
                {
                    TimeShotofLaser -= Time.deltaTime;
                }

            }

        }
    }

    private void Shoot()
    {
        CinemachineShaker.Instance.Shaker(1, 0.2f);
        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        fireFlash.Play(); //вспышка от выстрела
        RaycastHit2D hit = Physics2D.Raycast(bulletLauncher.transform.position, bulletLauncher.transform.right, gameplaySettings.weaponSettings.shootDistance); //запуск райкаста
        CheckHit(hit);
        DrawTraccer(hit); // отрисовка трассера
        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();
    }
    private void CrossBowShoot()
    {
        CinemachineShaker.Instance.Shaker(1, 0.2f);
        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        StartCoroutine(ShootDelay(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootDelay));
        Instantiate(gameplaySettings.weaponSettings.weapons[currentGunIndex].arrow, bulletLauncher.transform.position, transform.rotation);
        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();

    }
    private void InstanBulletForShotGun()
    {


        for (int i = 0; i <= 2; i++)
        {

            var spawnBullet = Instantiate(bullet, barrel.position, barrel.rotation);



            switch (i)
            {
                case 0:
                    spawnBullet.AddForce(new Vector3(0f, -120f, 0f));
                    break;
                case 1:
                    spawnBullet.AddForce(new Vector3(0f, 0f, 0f));
                    break;
                case 2:
                    spawnBullet.AddForce(new Vector3(0f, 120f, 0f));
                    break;
            }




        }


    }
    private void ShotGunShoot()
    {
        CinemachineShaker.Instance.Shaker(1, 0.2f);
        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        InstanBulletForShotGun();
        StartCoroutine(ShootDelay(gameplaySettings.weaponSettings.weapons[3].shootDelay));

        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();
    }



    #region Pistol
    private void PistolShoot()
    {
        Shoot();
        StartCoroutine(ShootDelay(gameplaySettings.weaponSettings.weapons[0].shootDelay));
    }
    #endregion

    #region Assault Rifle
    private void AssaultRifleShoot()
    {
        StartCoroutine(ARShoots(gameplaySettings.weaponSettings.weapons[1].shootDelay));

    }

    private void LaserShoot()
    {
        CinemachineShaker.Instance.Shaker(1, 0.2f);
        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        var shootDirection = bulletLauncher.transform.right;
        RaycastHit2D hit = Physics2D.Raycast(bulletLauncher.transform.position, shootDirection, gameplaySettings.weaponSettings.shootDistance); //запуск райкаста
        CheckHit(hit);
        DrawTraccer(hit); // отрисовка трассера
        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();
    }
    private void UziShoot()
    {

        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        fireFlash.Play(); //вспышка от выстрела
        var shootDirection = bulletLauncher.transform.right;
        shootDirection.y = Mathf.Clamp(shootDirection.y * Random.Range(0.001f, 10f), -180, 180);
        RaycastHit2D hit = Physics2D.Raycast(bulletLauncher.transform.position, shootDirection, gameplaySettings.weaponSettings.shootDistance); //запуск райкаста
        CheckHit(hit);
        DrawTraccer(hit); // отрисовка трассера
        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();
    }


    private void GrenadeLauncgerShoot()
    {
        CinemachineShaker.Instance.Shaker(1, 0.2f);
        PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
        Instantiate(gameplaySettings.weaponSettings.weapons[currentGunIndex].projectile, bulletLauncher.transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90));
        StartCoroutine(ShootDelay(gameplaySettings.weaponSettings.weapons[2].shootDelay));
        currentAmmo--;
        OnAmmoChange?.Invoke();

        CheckAmmo();
    }


    private IEnumerator ARShoots(float delay)
    {
        canShoot = false;
        var shootsCount = 3;
        while (shootsCount > 0)
        {
            Shoot();
            shootsCount--;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay * 2);
        canShoot = true;
    }

    #endregion

    private IEnumerator ShootDelay(float delay)
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private void PlayShootSound(AudioClip shootClip)
    {
        gunShoot.clip = shootClip; //выбор звука выстрела
        gunShoot.pitch = Random.Range(0.9f, 1.1f); //делает рандомную высоту звука
        gunShoot.Play(); //проигрывает звук
    }

    private void CheckHit(RaycastHit2D hit)
    {
        if (hit)
        {
            switch (hit.transform.tag)
            {
                case "Enemy":
                    hit.transform.GetComponent<EnemyBasic>().TakeDamage(gameplaySettings.weaponSettings.weapons[currentGunIndex].damage);
                    if (hit.transform.GetComponent<EnemyBasic>().GetArmor() > 0)
                    {
                        Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(hitPrefab, hit.point, Quaternion.identity);
                    }
                    break;
                case "zombie":
                    hit.transform.GetComponent<ZombieAI>().TakeDamage(gameplaySettings.weaponSettings.weapons[currentGunIndex].damage);
                    Instantiate(hitPrefab, hit.point, Quaternion.identity);
                    break;
                case "fly":
                    hit.transform.GetComponent<fly>().TakeDamage(gameplaySettings.weaponSettings.weapons[currentGunIndex].damage);
                    break;
                default:
                    Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    break;
            }
        }
    }

    private void DrawTraccer(RaycastHit2D hit)
    {
        if (currentGunIndex != 6)
        {
            if (hit)
            {
                var line = Instantiate(tracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, hit.point);
            }
            else
            {
                var line = Instantiate(tracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, bulletLauncher.transform.position + bulletLauncher.transform.right * gameplaySettings.weaponSettings.shootDistance);
            }
        }
        else if(currentGunIndex == 6)
        {
            if (hit)
            {
                var line = Instantiate(LasertracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, hit.point);
            }
            else
            {
                var line = Instantiate(LasertracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, bulletLauncher.transform.position + bulletLauncher.transform.right * gameplaySettings.weaponSettings.shootDistance);
            }
        }
    }

    private Coroutine reloadCoroutine = null;

    private void ReloadGun()
    {
        if (reloadCoroutine == null)
        {
            StopAllCoroutines();
            reloadCoroutine = StartCoroutine(ReloadGunCoroutine());
        }
    }

    private IEnumerator ReloadGunCoroutine()
    {
        if (currentAmmo < maxAmmo && (currentGunIndex >= 0 || genAmmo > 0))
        {

            canShoot = false;
            gunReload.clip = gameplaySettings.weaponSettings.weapons[currentGunIndex].reloadClip;
            gunReload.Play();
            yield return new WaitForSeconds(gameplaySettings.weaponSettings.weapons[currentGunIndex].reloadTime);
            var bulletsToAdd = Mathf.Min(maxAmmo - currentAmmo, genAmmo);
            currentAmmo = currentGunIndex >= 0 ? maxAmmo : currentAmmo + bulletsToAdd;
            genAmmo -= currentGunIndex >= 0 ? 0 : bulletsToAdd;
            canShoot = true;
            reloadCoroutine = null;
            OnAmmoChange?.Invoke();
        }
    }

    public void ChangeGun(int gunIndex)
    {
        if (gunIndex != currentGunIndex)
        {
            gunRenderer.sprite = gameplaySettings.weaponSettings.weapons[gunIndex].weaponSprite;
            bulletLauncher.transform.localPosition = gameplaySettings.weaponSettings.weapons[gunIndex].bulletLauncherPos;
            currentGunIndex = gunIndex;
            StopAllCoroutines();
            canShoot = true;
            maxAmmo = gameplaySettings.weaponSettings.weapons[gunIndex].maxAmmo;
            currentAmmo = maxAmmo;
            reloadCoroutine = null;
        }
        else
        {
            genAmmo += gameplaySettings.weaponSettings.weapons[gunIndex].maxAmmo;
        }

        OnAmmoChange?.Invoke();
    }
    public void BuyBullet(int gunIndex)
    {
        var test = FindObjectOfType<Test>();
        if (test.cash >= gameplaySettings.weaponSettings.weapons[gunIndex].bulletCost)
        {
            genAmmo += gameplaySettings.weaponSettings.weapons[gunIndex].maxAmmo;
            test.cash -= gameplaySettings.weaponSettings.weapons[gunIndex].bulletCost;
        }
      
    }
    
    
    private void CheckAmmo()
    {
        if (currentAmmo <= 0)
        {

            if (currentGunIndex >= 0)
            {
                ReloadGun();
            }
            else
            {
                if (genAmmo > 0)
                {
                    ReloadGun();
                }
                //else
                //{
                //    ChangeGun(0);
                //}

            }

        }
    }
}
