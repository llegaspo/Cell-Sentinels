using UnityEngine;
using UnityEngine.UI;

public class DNABar : MonoBehaviour
{
    [Header("Reference to the GameManager (DNA source)")]
    [Tooltip("Drag your GameManager GameObject here, so we can read currentDNA and startingDNA.")]
    public GameManager gameManager;

    [Header("UI References")]
    [Tooltip("The Canvas that holds this DNA bar (so we can hide/show when empty).")]
    [SerializeField] Canvas canvas;

    [Tooltip("The Image component used as the fill. Must be set to Image Type = Filled.")]
    [SerializeField] Image fillImage;

    [Header("Behavior Settings")]
    [Tooltip("If true, hides the bar entirely when DNA is zero.")]
    [SerializeField] bool hideWhenEmpty = false;

    [Tooltip("If true, DNA bar will always face the main camera.")]
    [SerializeField] bool alignWithCamera = false;

    [Tooltip("How quickly the fill animates toward the new DNA value (points per second).")]
    [SerializeField, Min(0.1f)] float changeSpeed = 100f;

    // Internally track a smoothed value so the bar animates instead of snapping.
    float currentValue;

    void Reset()
    {
        // If you forgot to assign GameManager in Inspector, try finding it at runtime.
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("[DNABar] No GameManager assigned or found in scene.");
            enabled = false;
            return;
        }

        if (fillImage == null)
        {
            Debug.LogError("[DNABar] No fillImage assigned in Inspector.");
            enabled = false;
            return;
        }

        // Initialize currentValue to the starting DNA (so the bar is full at start).
        currentValue = gameManager.GetCurrentDNA();
        UpdateFill();   // Set initial fill
        UpdateVisibility();
    }

    void Update()
    {
        if (alignWithCamera && Camera.main != null)
        {
            // Make the bar face the camera
            transform.forward = Camera.main.transform.forward;
        }

        // Smoothly move currentValue toward the actual DNA each frame
        float targetDNA = gameManager.GetCurrentDNA();
        currentValue = Mathf.MoveTowards(currentValue, targetDNA, Time.deltaTime * changeSpeed);

        UpdateFill();
        UpdateVisibility();
    }

    private void UpdateFill()
    {
        // We assume startingDNA > 0
        float maxDNA = gameManager.startingDNA;
        if (Mathf.Approximately(maxDNA, 0f))
        {
            fillImage.fillAmount = 0f;
            return;
        }

        // Remap currentValue (0..maxDNA) into 0..1 for the Image.fillAmount
        float normalized = Mathf.InverseLerp(0f, maxDNA, currentValue);
        fillImage.fillAmount = normalized;
    }

    private void UpdateVisibility()
    {
        if (canvas == null) return;

        bool isEmpty = Mathf.Approximately(fillImage.fillAmount, 0f);

        if (hideWhenEmpty)
        {
            canvas.gameObject.SetActive(!isEmpty);
        }
        else
        {
            // Always show
            canvas.gameObject.SetActive(true);
        }
    }
}
