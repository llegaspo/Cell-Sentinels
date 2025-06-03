using UnityEngine;
using UnityEngine.UI;

public class CytokinesisBar : MonoBehaviour
{
    public static CytokinesisBar Instance { get; private set; }

    [Header("Reference to the GameManager")]
    [Tooltip("Drag your GameManager GameObject here, so we can read currentStormValue.")]
    public GameManager gameManager;

    [Header("UI References")]
    [Tooltip("The Canvas that holds this Storm bar (to hide/show if desired).")]
    [SerializeField] Canvas canvas;

    [Tooltip("The Image component used as the fill. Must be Image Type = Filled.")]
    [SerializeField] Image fillImage;

    [Header("Behavior Settings")]
    [Tooltip("If true, hides the bar entirely when stormValue = 0.")]
    [SerializeField] bool hideWhenEmpty = false;

    [Tooltip("If true, Storm bar will face the main camera.")]
    [SerializeField] bool alignWithCamera = false;

    [Tooltip("How quickly the fill animates toward the new Storm value (units per second).")]
    [SerializeField, Min(0.1f)] float changeSpeed = 100f;

    float currentValue; // smoothed 0..1 value

    void Reset()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("[CytokinesisBar] No GameManager assigned or found.");
            enabled = false;
            return;
        }
        if (fillImage == null)
        {
            Debug.LogError("[CytokinesisBar] No fillImage assigned.");
            enabled = false;
            return;
        }

        // Initialize at the current storm value
        currentValue = gameManager.GetStormNormalized();
        UpdateFill();
        UpdateVisibility();
    }

    void Update()
    {
        if (gameManager == null) return;

        if (alignWithCamera && Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        float target = gameManager.GetStormNormalized();
        currentValue = Mathf.MoveTowards(currentValue, target, Time.deltaTime * changeSpeed);

        UpdateFill();
        UpdateVisibility();
    }

    void UpdateFill()
    {
        fillImage.fillAmount = currentValue;
    }

    void UpdateVisibility()
    {
        if (canvas == null) return;

        bool isEmpty = Mathf.Approximately(fillImage.fillAmount, 0f);

        if (hideWhenEmpty)
            canvas.gameObject.SetActive(!isEmpty);
        else
            canvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// (Optional) You can call this from GameManager whenever storm changes too.
    /// Ensures the bar jumps immediately if needed.
    /// </summary>
    public void RefreshBar()
    {
        currentValue = gameManager.GetStormNormalized();
        UpdateFill();
        UpdateVisibility();
    }
}
