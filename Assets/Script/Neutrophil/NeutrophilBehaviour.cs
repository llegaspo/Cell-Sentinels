using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NeutrophilBehaviour : MonoBehaviour
{
    public float speed = 3f;             // Units per second
    private Transform targetPathogen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindNearestPathogen();
        MoveTowardsTarget();
    }

    void FindNearestPathogen()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Pathogen");
        float closestDist = Mathf.Infinity;
        Transform best = null;

        foreach (GameObject go in all)
        {
            float dist = Vector2.Distance(transform.position, go.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                best = go.transform;
            }
        }

        targetPathogen = best;
    }
     void MoveTowardsTarget()
    {
        if (targetPathogen == null) return;

        Vector2 direction = (targetPathogen.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}
