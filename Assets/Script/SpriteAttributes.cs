using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpriteAttributes : MonoBehaviour
{
    [Header("Health & Attack Settings")]
    public float attackDamage = 5f;      // Damage per second dealt to the enemy
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Tag Settings")]
    public string enemyTag = "Enemy";    // e.g. "Pathogen" on the neutrophil; unused on the pathogen itself

    [Header("Cytokinesis Storm Settings")]
    [Tooltip("How much this cell adds to the Cytokinesis Storm bar when spawned.")]
    public int cytokinesisValue = 5;

    private SpriteRenderer spriteRenderer;
    private Collider2D thisCollider;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisCollider = GetComponent<Collider2D>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Only interact with objects tagged as our enemyTag
        if (other.CompareTag(enemyTag))
        {
            SpriteAttributes enemy = other.GetComponent<SpriteAttributes>();
            if (enemy != null)
            {
                // Deal damage over time to the enemy
                enemy.TakeDamage(attackDamage * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
{
    if (gameObject.CompareTag("Neutrophil") && GameManager.Instance != null)
    {
        GameManager.Instance.OnCellDeath(gameObject);
    }

    if (gameObject.CompareTag("Pathogen") && GameManager.Instance != null)
    {
        GameManager.Instance.OnPathogenDeath(gameObject);
    }

    if (thisCollider != null)
    {
        thisCollider.enabled = false;
    }

    if (spriteRenderer != null)
    {
        StartCoroutine(FadeOutAndDestroy());
    }
    else
    {
        Destroy(gameObject);
    }
}


    System.Collections.IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.5f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
