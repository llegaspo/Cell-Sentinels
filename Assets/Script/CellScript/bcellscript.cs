using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BCellAI : MonoBehaviour
{
    public float speed = 1.5f; // Slightly slower than TCell for variation
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GameObject target = FindClosestBacteria();
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;
            Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    GameObject FindClosestBacteria()
    {
        GameObject[] bacteria = GameObject.FindGameObjectsWithTag("Bacteria");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject b in bacteria)
        {
            float distance = Vector2.Distance(rb.position, b.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = b;
            }
        }

        return closest;
    }
}
