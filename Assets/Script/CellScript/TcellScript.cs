using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TCellAI : MonoBehaviour
{
    public float speed = 2f;
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
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // Calculate target position but keep physics collision
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
            float distance = Vector2.Distance(transform.position, b.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = b;
            }
        }

        return closest;
    }
}
