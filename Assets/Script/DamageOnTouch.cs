using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public delegate void OnHitSomething();
    public OnHitSomething OnHit;

    public float Damage = 1f;
    public float PushForce = 10f;

    public GameObject[] DamagableFeedback;
    public GameObject[] AnythingFeedback;


    public LayerMask TargetLayerMask;
    public LayerMask IgnoreLayerMask;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((IgnoreLayerMask.value & (1 << col.gameObject.layer)) > 0))
            return;

        if (((TargetLayerMask.value & (1 << col.gameObject.layer)) > 0))
        {
            Damagable(col);
        }
        else
        {
            HitAnything(col);
        }
    }

    private void Damagable(Collider2D col)
    {
        Debug.Log("target hit");
        Health targetHealth = col.gameObject.GetComponent<Health>();

        Debug.Log(targetHealth);

        if (targetHealth == null)
            return;

        Rigidbody2D targetRigidbody = col.gameObject.GetComponent<Rigidbody2D>();

        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce((col.transform.position - transform.position).normalized * PushForce);
        }

        TryDamage(targetHealth);
        SpawnFeedback(DamagableFeedback);
        Debug.Log("Hit Damagable");
    }

    private void HitAnything(Collider2D col)
    {
        Rigidbody2D targetRigidbody = col.gameObject.GetComponent<Rigidbody2D>();

        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce((col.transform.position - transform.position).normalized * PushForce);
        }

        OnHit?.Invoke();
        SpawnFeedback(AnythingFeedback);
        Debug.Log("Hit Anything");
    }

    private void TryDamage(Health targetHealth)
    {
        targetHealth.Damage(Damage, transform.gameObject);
        Debug.Log("Hit " + targetHealth.CurrentHealth);
        OnHit?.Invoke();
    }


    void SpawnFeedback(GameObject[] Feedbacks)
    {
        foreach (var feedback in Feedbacks)
        {
            GameObject FeedbackClone = GameObject.Instantiate(feedback, transform.position, transform.rotation);
            Destroy(FeedbackClone, 1f);
        }
    }
}
