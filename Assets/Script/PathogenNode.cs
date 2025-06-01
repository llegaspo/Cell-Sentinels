using UnityEngine;

public class PathogenNode : MonoBehaviour
{
    void Start()
    {
        // Move pathogen towards the cell node for demonstration
        GameObject target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().linearVelocity = direction * 2f; // speed = 2 units/sec
        }
    }
}
