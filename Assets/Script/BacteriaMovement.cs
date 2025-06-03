using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BacteriaWiggle : MonoBehaviour
{
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(11.5f, 10f);
    public float speed = 3.5f;
    public float detectionRadius = 3f; // how far bacteria can "see" a cell

    private Vector2 targetPosition;
    private GameObject currentTargetCell;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        PickNewTargetPosition();
    }

    void FixedUpdate()
    {
        DetectNearbyCell();

        Vector2 currentPosition = rb.position;

        if (currentTargetCell != null)
        {
            Vector2 cellPos = currentTargetCell.transform.position;
            Vector2 newPos = Vector2.MoveTowards(currentPosition, cellPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                PickNewTargetPosition();
            }
        }
    }

    void DetectNearbyCell()
    {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        float closestDistance = detectionRadius;
        currentTargetCell = null;

        foreach (GameObject cell in cells)
        {
            float distance = Vector2.Distance(rb.position, cell.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTargetCell = cell;
            }
        }
    }

    void PickNewTargetPosition()
    {
        float minX = areaCenter.x - areaSize.x / 2;
        float maxX = areaCenter.x + areaSize.x / 2;
        float minY = areaCenter.y - areaSize.y / 2;
        float maxY = areaCenter.y + areaSize.y / 2;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        targetPosition = new Vector2(randomX, randomY);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
