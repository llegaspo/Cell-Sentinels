using UnityEngine;

public class CellNode : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pathogen"))
        {
            Debug.Log("Enemy Pathogen Detected! Engaging...");
            // Here you could reduce pathogen health or destroy it
            Destroy(other.gameObject);
        }
    }
}
