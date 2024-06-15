using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class GunController : MonoBehaviour
{

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
    [SerializeField] AudioSource gunShoot;
    [SerializeField] AudioSource gunReload;
    public Transform barrel;
    public Rigidbody2D bullet;
    public event System.Action OnAmmoChange;
    public float rotateUp;
    public float rotateDown;
    MainUiController mainUiController;
   
    public int genAmmo { get; private set; } = 0;
    public int maxAmmo { get; private set; }
    public int currentAmmo { get; private set; }
    public Transform player;
    [SerializeField] private GameplaySettings gameplaySettings;

    private bool canShoot = true;

    public int currentGunIndex { get; private set; } = 999;


    void Start()
    {
        mainUiController = FindObjectOfType<MainUiController>();
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
                        ShotGunShoot();
                        break;
                    case 4:
                        GrenadeLauncgerShoot();
                        break;

                }

            }

        }

        Uzi();
        LaserSetting();

        for (int i = 0; i < 6; i++)
        {
            if (gameManager.instance.weaponsToUnlock[i] == true)
            {
                KeyCode keyCode = KeyCode.Alpha1 + i;
                if (Input.GetKeyDown(keyCode))
                {
                    ChangeGun(i);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) ReloadGun();
    }



    private void LaserSetting()
    {
        if (currentAmmo > 0)
        {
            if (currentGunIndex == 5)
            {
                if (Input.GetMouseButton(0) && canShoot)
                {
                    LaserShoot();
                }

            }

        }
    }

    public AudioSource laserSound;
    private void PlayLaserSound()
    {
        if (laserSound != null) // Убеждаемся, что ссылка на источник звука существует
        {
            laserSound.PlayOneShot(laserSound.clip); // Воспроизводим звук лазера один раз
        }
    }

    private void Uzi()
    {
        if (currentAmmo > 0)
        {
            if (currentGunIndex == 3)
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
    }


    public void CanShoot(bool active)
    {
        canShoot = active;
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
    //private void CrossBowShoot()
    //{
    //    CinemachineShaker.Instance.Shaker(1, 0.2f);
    //    PlayShootSound(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootClip);
    //    StartCoroutine(ShootDelay(gameplaySettings.weaponSettings.weapons[currentGunIndex].shootDelay));
    //    Instantiate(gameplaySettings.weaponSettings.weapons[currentGunIndex].arrow, bulletLauncher.transform.position, transform.rotation);
    //    currentAmmo--;
    //    OnAmmoChange?.Invoke();

    //    CheckAmmo();

    //}
    private void InstanBulletForShotGun()
    {
        Instantiate(bullet, barrel.position, barrel.rotation);
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
        var shootDirection = bulletLauncher.transform.right;
        RaycastHit2D hit = Physics2D.Raycast(bulletLauncher.transform.position, shootDirection, gameplaySettings.weaponSettings.shootDistance); //запуск райкаста
        CheckHit(hit);
        DrawTraccer(hit); 
        currentAmmo--;
        OnAmmoChange?.Invoke();
        CheckAmmo();
        PlayLaserSound();
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
                case "bunker":
                    break;
                default:
                    Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    break;
            }
        }
    }

    private void DrawTraccer(RaycastHit2D hit)
    {
        if (currentGunIndex != 5)
        {
            if (hit && hit.transform.tag != "bunker")
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
        else if(currentGunIndex == 5)
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
            mainUiController.ChangeWeaponIcon(gunIndex);
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

            }

        }
    }
}
