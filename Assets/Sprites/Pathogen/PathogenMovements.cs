using UnityEngine;
using System.Collections.Generic;

public class BacteriaManager : MonoBehaviour
{
    public List<GameObject> bacteriaList;
    public float speed = 2f;
    public float changeDirectionTime = 2f;
    public float turnSpeed = 2f; // Higher = faster turning

    private Dictionary<GameObject, Vector2> currentDirections = new Dictionary<GameObject, Vector2>();
    private Dictionary<GameObject, Vector2> targetDirections = new Dictionary<GameObject, Vector2>();
    private Dictionary<GameObject, float> timers = new Dictionary<GameObject, float>();

    void Start()
    {
        foreach (var bacteria in bacteriaList)
        {
            Vector2 initialDir = GenerateLeftBiasedDirection();
            currentDirections[bacteria] = initialDir;
            targetDirections[bacteria] = initialDir;
            timers[bacteria] = changeDirectionTime;
        }
    }

    void Update()
    {
        foreach (var bacteria in bacteriaList)
        {
            // Smoothly rotate current direction towards target direction
            currentDirections[bacteria] = Vector2.Lerp(
                currentDirections[bacteria],
                targetDirections[bacteria],
                turnSpeed * Time.deltaTime
            ).normalized;

            // Move bacteria
            bacteria.transform.Translate(currentDirections[bacteria] * speed * Time.deltaTime);

            // Flip sprite based on X direction
            if (currentDirections[bacteria].x != 0)
            {
                Vector3 scale = bacteria.transform.localScale;
                scale.x = currentDirections[bacteria].x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                bacteria.transform.localScale = scale;
            }

            // Timer for changing target direction
            timers[bacteria] -= Time.deltaTime;
            if (timers[bacteria] <= 0)
            {
                targetDirections[bacteria] = GenerateLeftBiasedDirection();
                timers[bacteria] = changeDirectionTime;
            }
        }
    }

    Vector2 GenerateLeftBiasedDirection()
    {
        // X is biased to negative (left), with some random variation
        float x = Random.Range(-1f, -0.2f); // mostly negative, slight randomness
        float y = Random.Range(-1f, 1f);    // full random up/down

        return new Vector2(x, y).normalized;
    }
}
