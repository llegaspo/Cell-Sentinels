using UnityEngine;
public class PathogenRandomMovement : MonoBehaviour
{
    [Tooltip("How fast the pathogen drifts around.")]
    public float moveSpeed = 1f;

    private Vector2 randomDirection;

    void Start()
    {
        // On Start, pick a random normalized direction once.
        SetRandomDirection();
    }

    void Update()
    {
        // Move in that direction every frame.
        transform.position += (Vector3)randomDirection * moveSpeed * Time.deltaTime;
    }

    
    public void SetRandomDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized;
    }
}