using UnityEngine;

namespace Biostart.DayNight
{
public class DayNightCycle : MonoBehaviour {

    public Light sun;
    public Light moon;
    [Range(0, 1)]
    public float currentTimeOfDay = 0.45f;
    [Space(10)]
    public Gradient dayColor;
    public Gradient nightColor;
    public AnimationCurve sunIntensityCurve;
    public AnimationCurve moonIntensityCurve;

    [Header("Fog Settings")]
    public AnimationCurve fogDensityCurve;
    public Gradient nightDayFogColor;
    public float fogScale = 1f;

    [Header("Sun Rotation Settings")]
    public float sunRotationY = 170f;
    public float sunRotationSeconds = 60f;

    // Speed multiplier for the sun's rotation
    public float rotationSpeedMultiplier = 1f;

#if UNITY_EDITOR
    [HideInInspector]
    public bool showSettings = false;

    public void UpdateEditor() {
        UpdatePosition();
        UpdateFX();
    }

    public void EditorSetup() {
        sun = GetComponent<Light>();
    }
#endif

    void Start() {
         if (sun == null) {
        sun = GetComponent<Light>();
    }

    if (sun == null) {
        Debug.LogError("Sun Light is not assigned and no Light component found on this GameObject!");
        return;
    }

    SetSunRotation(currentTimeOfDay);
    InvokeRepeating("UpdateCycle", 0f, 0.01f);
    }

    // Public method for updating the day-night cycle
    public void UpdateCycle() {
        UpdatePosition();
        UpdateFX();

        // Update currentTimeOfDay and clamp it between 0 and 1
        currentTimeOfDay += Time.deltaTime / sunRotationSeconds * rotationSpeedMultiplier; // Adjust for speed multiplier
        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }
    }

    // Method to set the initial sun position based on timeOfDay
    public void SetSunRotation(float timeOfDay) {
        float targetRotationX = (timeOfDay * 360f) - 90f; // Rotate on the X-axis from -90 to 270
        float targetRotationY = sunRotationY;

        // Apply the initial sun position
        sun.transform.localRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
    }

    // Update the sun's position
    public void UpdatePosition() {
        // Initial sun angle on the X-axis is -90, then updated based on currentTimeOfDay
        float targetRotationX = (currentTimeOfDay * 360f) - 90f; // Rotate on the X-axis from -90 to 270
        float targetRotationY = sunRotationY; // Use public Y value

        // Update the sun's rotation on both axes
        sun.transform.localRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
    }

    // Update effects (sun/moon intensity, fog settings)
    public void UpdateFX() {
        // Get sun and moon intensity using curves
        float sunIntensity = sunIntensityCurve.Evaluate(currentTimeOfDay); // Use the curve for sun intensity
        float moonIntensity = moonIntensityCurve.Evaluate(currentTimeOfDay); // Use the curve for moon intensity

        // Apply intensity to the sun and moon
        sun.intensity = sunIntensity;
        moon.intensity = moonIntensity;

        // Set the sun color based on the current time of day
        Color currentColor = dayColor.Evaluate(currentTimeOfDay);
        sun.color = currentColor;
        RenderSettings.ambientLight = currentColor;

        // Configure fog color and density for the current time of day
        RenderSettings.fogColor = nightDayFogColor.Evaluate(currentTimeOfDay); // Fog color depends on the time of day
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(currentTimeOfDay) * fogScale;

        // Set the moon color
        moon.color = nightColor.Evaluate(currentTimeOfDay);
    }

    // Triggered when values in the inspector are changed
    private void OnValidate() {
        // Clamp currentTimeOfDay between 0 and 1
        if (currentTimeOfDay > 1) currentTimeOfDay = 1;
        if (currentTimeOfDay < 0) currentTimeOfDay = 0;

        // Immediately update the sun position in the editor
        UpdatePosition();
        UpdateFX();
    }
}
}