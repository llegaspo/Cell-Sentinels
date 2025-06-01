using UnityEngine;

public class UIPupilFollow : MonoBehaviour
{
    public RectTransform eyeRect;       // Reference to the eye
    public float maxDistance = 20f;     // How far the pupil can move
    public float smoothSpeed = 10f;     // Higher = faster movement

    private RectTransform pupilRect;
    private Vector2 targetPosition;

    void Start()
    {
        pupilRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 localMousePos;

        // Convert mouse to local space relative to eye
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            eyeRect, Input.mousePosition, null, out localMousePos))
        {
            // Clamp position within max distance circle
            targetPosition = Vector2.ClampMagnitude(localMousePos, maxDistance);
        }

        // Smoothly move toward target
        pupilRect.anchoredPosition = Vector2.Lerp(
            pupilRect.anchoredPosition, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
