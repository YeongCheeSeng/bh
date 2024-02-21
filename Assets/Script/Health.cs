using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public delegate void HitEvent(GameObject source);
    public HitEvent OnHit;

    public delegate void ResetEvent();
    public ResetEvent OnHitReset;

    public delegate void DeathEvent();
    public DeathEvent OnDeath;

    public delegate void HealEvent(GameObject source);
    public HealEvent OnHeal;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }

    public float MaxHealth = 10f;

    public Cooldown Invulnerable;

    private float _currentHealth = 10f;
    private bool _canDamage = true;

    public GameObject DeadEffect;

    // Update is called once per frame
    private void Update()
    {
        ResetInvulnerable();
        PlayerDeath();
    }

    private void ResetInvulnerable()
    {
        if (_canDamage)
            return;

        if (Invulnerable.IsOnCooldown && _canDamage == false)
            return;

        _canDamage = true;
        OnHitReset?.Invoke();
    }

    public void Damage(float damage, GameObject source)
    {
        if (!_canDamage)
            return;

        _currentHealth -= damage;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Die();
        }

        Invulnerable.StartCooldown();
        _canDamage = false;

        OnHit?.Invoke(source);
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(this.gameObject);
        GameObject DeadSpawn = GameObject.Instantiate(DeadEffect, transform.position, transform.rotation);
        Destroy(DeadSpawn, 1f);
    }

    public void Heal(float healAmount)
    {
        _currentHealth += healAmount;

        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        OnHeal?.Invoke(gameObject);
    }

    public void PlayerDeath()
    {
        GameObject Player = GameObject.FindWithTag("Player");

        if (Player == null)
        SceneManager.LoadScene("GameOver");
    }
}
