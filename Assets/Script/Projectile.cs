using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 1f;
    public float Speed = 100f;
    public float PushForce = 50f;
    //public float LifeTime = 1f;
    public Cooldown LifeTime;

    public LayerMask TargetLayerMask;

    private DamageOnTouch _damageOnTouch;
    private Rigidbody2D _rigidbody;

    //public AudioSource GunfireSound;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
            return;

        _rigidbody.AddRelativeForce(new Vector2(x: 0f, y: Speed));


        _damageOnTouch = GetComponent<DamageOnTouch>();

        // subscribing
        if (_damageOnTouch != null)
            _damageOnTouch.OnHit += Die;

        LifeTime.StartCooldown();

        //GunfireSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        /*
        if (_timer < LifeTime)
        {
            _timer += Time.deltaTime;
            return;
        }
        */

        if (LifeTime.CurrentProgress != Cooldown.Progress.Finished)
            return;

        //PlayGunfireSound();
        Die();
    }

    protected void Die()
    {
        //unsubscribing
        if (_damageOnTouch != null)
            _damageOnTouch.OnHit -= Die;

        LifeTime.StopCoolDown();
        Destroy(this.gameObject);
    }

    //public void PlayGunfireSound()
    //{
    //    GunfireSound.Play();
    //}
}

