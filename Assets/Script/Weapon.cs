using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject[] Feedback;

    public GameObject Projectile;
    public Transform SpawnPos;
    public float Interval = 0.1f;
    public Cooldown AutofireShootInterval;

    private float _timer = 0f;
    private bool _canShoot = true;
    private bool _singleFireReset = true;

    public int BurstFireAmount = 3;
    public float BurstFireInterval = 0.1f;


    private bool _burstFiring = false;
    private float _lastShootRequestAt;
    private float Spread = 0f;

    public int ProjectileCount = 1;
    public GameObject[] ReloadFeedbacks;
    public Cooldown ReloadCoolDown;
    public int MaxBulletCount = 12;


    public int CurrentBulletCount
    {
        get { return currentBulletCount; }
    }

    protected int currentBulletCount;

    public enum FireModes
    {
        SingleFire, // =0
        Auto,   // =1
        BurstFire   // =2
    }

    public FireModes FireMode;

    private void Start()
    {
        currentBulletCount = MaxBulletCount;
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (_timer < Interval)
        {
            _timer += Time.deltaTime; 
            _canShoot = false;
            return;
        }

        _timer = 0f;
        _canShoot = true;
        */

        UpdateShootCooldown();
        UpdateReloadCooldown();
        Reload();
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            currentBulletCount = 0;
            ReloadCoolDown.StartCooldown();

            foreach (var feedback in ReloadFeedbacks)
            {
                GameObject ReloadFeedback = GameObject.Instantiate(feedback, transform.position, transform.rotation);
                Destroy(ReloadFeedback, 1f);
            }
        }

    }

    private void UpdateShootCooldown()
    {
        if (AutofireShootInterval.CurrentProgress != Cooldown.Progress.Finished)
            return;

        AutofireShootInterval.CurrentProgress = Cooldown.Progress.Ready;
    }

    private void UpdateReloadCooldown()
    {
        if (ReloadCoolDown.CurrentProgress != Cooldown.Progress.Finished)
            return;

        if (ReloadCoolDown.CurrentProgress == Cooldown.Progress.Finished)
        {
            currentBulletCount = MaxBulletCount;
        }

        ReloadCoolDown.CurrentProgress = Cooldown.Progress.Ready;
    }


    public void Shoot()
    {
        if (Projectile == null)
        {
            Debug.LogWarning("Missing Projectile prefeb");
            return;
        }

        if (SpawnPos == null)
        {
            Debug.LogWarning("Missing SpawnPosition transform");
            return;
        }

        if (ReloadCoolDown.IsOnCooldown || ReloadCoolDown.CurrentProgress != Cooldown.Progress.Ready)
            return;

        switch (FireMode)
        {
            case FireModes.Auto:
                {
                    AutofireShoot();
                    break;
                }
            case FireModes.SingleFire:
                {
                    SinglefireShoot();
                    break;
                }
            case FireModes.BurstFire:
                {
                    BurstfireShoot();
                    break;
                }
        }

        //if (!_canShoot)
        //    return;

        //if (AutofireShootInterval.CurrentProgress != Cooldown.Progress.Ready)
        //    return;

        //AutofireShootInterval.StartCooldown();

        //SpawnFeedback();
    }


    void SpawnFeedback()
    {
        foreach (var feedback in Feedback)
        {
            GameObject spawnfeedback = GameObject.Instantiate(feedback, SpawnPos.position, SpawnPos.rotation);
            Destroy(spawnfeedback, 1f);
        }
    }

    void ShootProjectile()
    {
        float randomRot = Random.Range(-Spread, Spread);

        GameObject bullet = GameObject.Instantiate(Projectile, SpawnPos.position, SpawnPos.rotation * Quaternion.Euler(0, 0, randomRot));
        SpawnFeedback();
    }



    void AutofireShoot()
    {
        Debug.Log("Auto Fire");

        if (!_canShoot)
            return;

        if (AutofireShootInterval.CurrentProgress != Cooldown.Progress.Ready)
            return;

        Debug.Log("Fire");
        ShootProjectile();
        AutofireShootInterval.StartCooldown();

        currentBulletCount--;
        StartReload();
    }


    void SinglefireShoot()
    {
        Debug.Log("Single Fire");

        if (!_canShoot)
            return;

        if (AutofireShootInterval.CurrentProgress != Cooldown.Progress.Ready)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire");
            ShootProjectile();
            AutofireShootInterval.StartCooldown();
            currentBulletCount--;
        }

        StartReload();
    }

    void BurstfireShoot()
    {
        Debug.Log("Burst Fire");

        if (!_canShoot)
            return;

        if (_burstFiring)
            return;

        if (AutofireShootInterval.CurrentProgress != Cooldown.Progress.Ready)
            return;

        //StartCoroutine("BurstfireCo");
        StartCoroutine(BurstfireCo(1f));
    }

    IEnumerator BurstfireCo(float time = 3f)
    {
        _burstFiring = true;
        _singleFireReset = false;
        Debug.Log("start");

        if (Time.time - _lastShootRequestAt < BurstFireInterval)
        {
            yield break;
        }

        int remainingShots = BurstFireAmount;

        while (remainingShots > 0)
        {
            ShootProjectile();
            currentBulletCount--;

            if (currentBulletCount <= 0)
                break;


            _lastShootRequestAt = Time.time;

            remainingShots--;
            yield return WaitFor(BurstFireInterval);
        }


        /*
        while (time > 0)
        {
            time -= Time.deltaTime;
            Debug.Log(time + " remaining");
            yield return null;
        }
        */

        Debug.Log("ended");

        _burstFiring = false;
        AutofireShootInterval.StartCooldown();

        StartReload();
    }
    public void StartReload()
    {
        if (currentBulletCount <= 0 && !ReloadCoolDown.IsOnCooldown)
        {
            foreach (var feedback in ReloadFeedbacks)
            {
                GameObject ReloadFeedback = GameObject.Instantiate(feedback, transform.position, transform.rotation);
                Destroy(ReloadFeedback, 1f);
            }

            ReloadCoolDown.StartCooldown();
        }

    }

    IEnumerator WaitFor(float seconds)
    {
        for (float timer = 0f; timer < seconds; timer += Time.deltaTime)
        {
            yield return null;
        }
    }

    public void StopShoot()
    {
        _singleFireReset = true;
    }

}

