using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Biostart.DayNight
{
    public class DayNightCycleWeather : MonoBehaviour
    {
        [Header("Time Mode Settings")]
        [SerializeField] private bool useSystemTime = true;
        [SerializeField] private bool useLocalTime = true; // If false, uses UTC
        [SerializeField] private float timeOffset = 0f; // Hours to offset from system time
        
        [Header("Manual Time Settings (Debug Mode)")]
        [Range(0, 24)]
        public float currentTimeOfDay = 1f;

        [SerializeField, Tooltip("HH:MM")]
        private string timeDisplay;

        [SerializeField, Tooltip("Day of the month (1-MaxDay)")]
        private int currentDay = 1;

        [Header("General Settings")]
        [Range(1, 365)]
        public int maxDay = 30; // Maximum number of days in a cycle
        
        [Header("Cloud Coverage Settings")]
        public bool enableCloudCoverage = true; // Enable/disable cloud coverage changes
        public float minCloudCoverage = 0f; // Minimum cloud coverage value
        public float maxCloudCoverage = 1f; // Maximum cloud coverage value
        public float randomChangeInterval = 10f; // Time interval (in seconds) to update cloud coverage randomly
        public float coverageSecondPeriod = 60f; // Time interval (in seconds) to update cloud coverage randomly

        private float currentRandomCoverage; // Current randomly generated cloud coverage
        private float timeSinceLastChange;   // Timer to track time since the last random change

        [Space(10)]
        public Light sun;
        public Light moon;
        public Gradient dayColor;
        public Gradient nightColor;
        public AnimationCurve sunIntensityCurve;
        public AnimationCurve moonIntensityCurve;

        [Header("Sun Intensity Settings")]
        public float maxSunIntensity = 8f; // Maximum sun intensity value

        [Header("Fog Settings")]
        public AnimationCurve fogDensityCurve;
        public Gradient nightDayFogColor;
        public float fogScale = 1f;

        [Header("Sun Rotation Settings")]
        public float sunRotationY = 170f;
        public float sunRotationSeconds = 60f;
        public float rotationSpeedMultiplier = 1f;

        [Header("Weather Effects")]
        public List<WeatherEffect> weatherEffects = new List<WeatherEffect>();

        [Header("Skybox Settings")]
        [SerializeField] private Material skyboxMaterial;
        public float cloudChangeSpeed = 0.1f;
        private float defaultCloudCoverage;

        [Header("Water Material Settings")]
        [SerializeField] private Material waterMaterial;
        public AnimationCurve waterTranslucencyCurve;
        public AnimationCurve waterReflectionCurve;
        
        [Header("Translucency Curvature Settings")]
        public float dayTranslucencyCurvature = 0.9f;   // Day translucency (0-1 range)
        public float nightTranslucencyCurvature = 0.0f; // Night translucency (0-1 range)
        
        [Header("Reflection Curvature Settings")]  
        public float dayReflectionCurvature = 0.8f;     // Day reflection curvature
        public float nightReflectionCurvature = 0.0f;   // Night reflection curvature
        
        private float defaultTranslucencyCurvature;
        private float defaultReflectionCurvature;

        private void Start()
        {
            if (sun == null)
            {
                sun = GetComponent<Light>();
            }

            if (sun == null)
            {
                Debug.LogError("Sun Light is not assigned and no Light component found on this GameObject!");
                return;
            }

            if (skyboxMaterial != null && skyboxMaterial.HasProperty("_CloudCoverage1"))
            {
                defaultCloudCoverage = skyboxMaterial.GetFloat("_CloudCoverage1");
            }
            else
            {
                Debug.LogError("Skybox Material or _CloudCoverage1 property not found!");
            }

            // Initialize water material settings
            if (waterMaterial != null)
            {
                if (waterMaterial.HasProperty("_TranslucencyCurvatureMask"))
                {
                    defaultTranslucencyCurvature = waterMaterial.GetFloat("_TranslucencyCurvatureMask");
                }
                
                if (waterMaterial.HasProperty("_ReflectionFresnel"))
                {
                    defaultReflectionCurvature = waterMaterial.GetFloat("_ReflectionFresnel");
                }
                
                if (!waterMaterial.HasProperty("_TranslucencyCurvatureMask") && !waterMaterial.HasProperty("_ReflectionFresnel"))
                {
                    Debug.LogWarning("Water Material found but neither _TranslucencyCurvatureMask nor _ReflectionFresnel properties found!");
                }
            }

            // Initialize curves
            InitializeSunIntensityCurve();
            InitializeWaterCurvatureCurve();

            // Set initial time
            if (useSystemTime)
            {
                UpdateFromSystemTime();
            }
            
            SetSunRotation(currentTimeOfDay);
            InvokeRepeating("UpdateCycle", 0f, 0.01f);
        }

        private void UpdateFromSystemTime()
        {
            DateTime now = useLocalTime ? DateTime.Now : DateTime.UtcNow;
            
            // Add offset
            now = now.AddHours(timeOffset);
            
            // Convert to 24-hour format
            currentTimeOfDay = now.Hour + (now.Minute / 60f) + (now.Second / 3600f);
            
            // Set current day based on day of month
            currentDay = Mathf.Clamp(now.Day, 1, maxDay);
        }

        // Public methods to switch between modes (can be called from UI buttons)
        public void SetManualTimeMode()
        {
            useSystemTime = false;
            Debug.Log("Switched to Manual Time Mode");
        }

        public void SetSystemTimeMode()
        {
            useSystemTime = true;
            UpdateFromSystemTime();
            Debug.Log("Switched to System Time Mode");
        }

        public void ToggleTimeMode()
        {
            useSystemTime = !useSystemTime;
            if (useSystemTime)
            {
                UpdateFromSystemTime();
                Debug.Log("Switched to System Time Mode");
            }
            else
            {
                Debug.Log("Switched to Manual Time Mode");
            }
        }

        // Method to set time offset (useful for testing different time zones)
        public void SetTimeOffset(float offsetHours)
        {
            timeOffset = offsetHours;
            if (useSystemTime)
            {
                UpdateFromSystemTime();
            }
        }

        private void InitializeWaterCurvatureCurve()
        {
            // Initialize translucency curvature curve if not set
            if (waterTranslucencyCurve == null || waterTranslucencyCurve.length == 0)
            {
                waterTranslucencyCurve = new AnimationCurve();
                waterTranslucencyCurve.AddKey(0f, 0f);    // Midnight - night (0 = use night value)
                waterTranslucencyCurve.AddKey(0.2f, 0f);  // 4:48 AM - still night
                waterTranslucencyCurve.AddKey(0.3f, 0.5f); // 7:12 AM - dawn transition
                waterTranslucencyCurve.AddKey(0.5f, 1f);  // Noon - day (1 = use day value)
                waterTranslucencyCurve.AddKey(0.7f, 0.5f); // 4:48 PM - dusk transition
                waterTranslucencyCurve.AddKey(0.8f, 0f);  // 7:12 PM - night begins
                waterTranslucencyCurve.AddKey(1f, 0f);    // Midnight - night
                
                for (int i = 0; i < waterTranslucencyCurve.length; i++)
                {
                    waterTranslucencyCurve.SmoothTangents(i, 0.3f);
                }
            }
            
            // Initialize reflection curvature curve if not set
            if (waterReflectionCurve == null || waterReflectionCurve.length == 0)
            {
                waterReflectionCurve = new AnimationCurve();
                waterReflectionCurve.AddKey(0f, 0f);    // Midnight - night (0 = use night value)
                waterReflectionCurve.AddKey(0.2f, 0f);  // 4:48 AM - still night
                waterReflectionCurve.AddKey(0.3f, 0.5f); // 7:12 AM - dawn transition
                waterReflectionCurve.AddKey(0.5f, 1f);  // Noon - day (1 = use day value)
                waterReflectionCurve.AddKey(0.7f, 0.5f); // 4:48 PM - dusk transition
                waterReflectionCurve.AddKey(0.8f, 0f);  // 7:12 PM - night begins
                waterReflectionCurve.AddKey(1f, 0f);    // Midnight - night
                
                for (int i = 0; i < waterReflectionCurve.length; i++)
                {
                    waterReflectionCurve.SmoothTangents(i, 0.3f);
                }
            }
        }

        private void InitializeSunIntensityCurve()
        {
            // Check if curve is empty or has very low values
            if (sunIntensityCurve == null || sunIntensityCurve.length == 0)
            {
                sunIntensityCurve = new AnimationCurve();
                sunIntensityCurve.AddKey(0f, 0f);      // Midnight - no sun
                sunIntensityCurve.AddKey(0.2f, 0f);    // 4:48 AM - sunrise starts
                sunIntensityCurve.AddKey(0.3f, 0.5f);  // 7:12 AM - morning light
                sunIntensityCurve.AddKey(0.5f, 1f);    // Noon - maximum intensity
                sunIntensityCurve.AddKey(0.7f, 0.5f);  // 4:48 PM - afternoon light
                sunIntensityCurve.AddKey(0.8f, 0f);    // 7:12 PM - sunset ends
                sunIntensityCurve.AddKey(1f, 0f);      // Midnight - no sun
                
                // Set smooth tangents
                for (int i = 0; i < sunIntensityCurve.length; i++)
                {
                    sunIntensityCurve.SmoothTangents(i, 0.3f);
                }
            }
            else
            {
                // Check if the curve has proper peak values
                float maxValue = 0f;
                for (int i = 0; i < sunIntensityCurve.length; i++)
                {
                    if (sunIntensityCurve[i].value > maxValue)
                        maxValue = sunIntensityCurve[i].value;
                }
                
                // If curve max is too low, normalize it to 1.0
                if (maxValue < 0.9f && maxValue > 0.1f)
                {
                    Debug.Log($"Sun intensity curve maximum was {maxValue}, normalizing to 1.0");
                    for (int i = 0; i < sunIntensityCurve.length; i++)
                    {
                        Keyframe key = sunIntensityCurve[i];
                        key.value = key.value / maxValue;
                        sunIntensityCurve.MoveKey(i, key);
                    }
                }
            }
        }

        private void Update()
        {
            timeDisplay = ConvertRangeToTime(currentTimeOfDay);
            TriggerWeatherEffects(timeDisplay, currentDay);

            if (enableCloudCoverage)
            {
                UpdateRandomCloudCoverage();
            }

            // Update from system time if enabled
            if (useSystemTime)
            {
                UpdateFromSystemTime();
            }
        }

        public void UpdateCycle()
        {
            UpdatePosition();
            UpdateFX();

            // Only update time automatically if not using system time
            if (!useSystemTime)
            {
                currentTimeOfDay += (Time.deltaTime / sunRotationSeconds * rotationSpeedMultiplier) * 24f;

                if (currentTimeOfDay >= 24f)
                {
                    currentTimeOfDay = 0f;
                    currentDay++;

                    if (currentDay > maxDay)
                    {
                        currentDay = 1;
                    }
                }
            }
        }
        
        private void UpdateRandomCloudCoverage()
        {
            timeSinceLastChange += Time.deltaTime;

            // Рассчитываем плавное изменение облачности через синусоидальную функцию
            currentRandomCoverage = Mathf.Lerp(minCloudCoverage, maxCloudCoverage, 
                (Mathf.Sin(2 * Mathf.PI * timeSinceLastChange / coverageSecondPeriod) + 1) / 2);

            // Применяем новое значение облачности к skybox
            if (skyboxMaterial != null && skyboxMaterial.HasProperty("_CloudCoverage1"))
            {
                float currentCoverage = skyboxMaterial.GetFloat("_CloudCoverage1");
                skyboxMaterial.SetFloat("_CloudCoverage1",
                    Mathf.Lerp(currentCoverage, currentRandomCoverage, Time.deltaTime * cloudChangeSpeed));
            }

            // Сбрасываем таймер при достижении полного периода
            if (timeSinceLastChange >= coverageSecondPeriod)
            {
                timeSinceLastChange = 0f;
            }
        }

        public void SetSunRotation(float timeOfDay)
        {
            float targetRotationX = (timeOfDay / 24f) * 360f - 90f;
            float targetRotationY = sunRotationY;

            sun.transform.localRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
        }

        public void UpdatePosition()
        {
            float targetRotationX = (currentTimeOfDay / 24f) * 360f - 90f;
            float targetRotationY = sunRotationY;

            sun.transform.localRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
        }

        public void UpdateFX()
        {
            // Calculate sun intensity using the curve (0-1) multiplied by max intensity
            float normalizedSunIntensity = sunIntensityCurve.Evaluate(currentTimeOfDay / 24f);
            float sunIntensity = normalizedSunIntensity * maxSunIntensity;
            
            float moonIntensity = moonIntensityCurve.Evaluate(currentTimeOfDay / 24f);

            sun.intensity = sunIntensity;
            moon.intensity = moonIntensity;

            Color currentColor = dayColor.Evaluate(currentTimeOfDay / 24f);
            sun.color = currentColor;
            RenderSettings.ambientLight = currentColor;

            RenderSettings.fogColor = nightDayFogColor.Evaluate(currentTimeOfDay / 24f);
            RenderSettings.fogDensity = fogDensityCurve.Evaluate(currentTimeOfDay / 24f) * fogScale;

            moon.color = nightColor.Evaluate(currentTimeOfDay / 24f);

            // Update water curvature mask based on time of day
            UpdateWaterCurvature();
        }

        private void UpdateWaterCurvature()
        {
            if (waterMaterial == null) return;
            
            float timeNormalized = currentTimeOfDay / 24f;
            
            // Update Translucency Curvature Mask
            if (waterMaterial.HasProperty("_TranslucencyCurvatureMask"))
            {
                // Get curve value (0-1) and map it directly to YOUR specified day/night range
                float curveValue = waterTranslucencyCurve.Evaluate(timeNormalized);
                float targetTranslucency = Mathf.Lerp(nightTranslucencyCurvature, dayTranslucencyCurvature, curveValue);
                
                // Clamp to ensure it stays within your specified range
                targetTranslucency = Mathf.Clamp(targetTranslucency, nightTranslucencyCurvature, dayTranslucencyCurvature);
                
                // Set directly to your calculated value
                waterMaterial.SetFloat("_TranslucencyCurvatureMask", targetTranslucency);
            }
            
            // Update Reflection Curvature Mask (Environment Reflections)
            if (waterMaterial.HasProperty("_ReflectionFresnel"))
            {
                // Get curve value (0-1) and map it directly to YOUR specified day/night range
                float curveValue = waterReflectionCurve.Evaluate(timeNormalized);
                float targetReflection = Mathf.Lerp(nightReflectionCurvature, dayReflectionCurvature, curveValue);
                
                // Clamp to ensure it stays within your specified range
                targetReflection = Mathf.Clamp(targetReflection, nightReflectionCurvature, dayReflectionCurvature);
                
                // Set directly to your calculated value
                waterMaterial.SetFloat("_ReflectionFresnel", targetReflection);
            }
        }

        public void SetCurrentDay(int day)
        {
            if (day < 1 || day > maxDay)
            {
                Debug.LogError($"Day must be between 1 and {maxDay}.");
                return;
            }

            currentDay = day;
            Debug.Log($"Day set to: {currentDay}");
            TriggerWeatherEffects(timeDisplay, currentDay);
        }

        private string ConvertRangeToTime(float timeValue)
        {
            int hours = Mathf.FloorToInt(timeValue);
            int minutes = Mathf.FloorToInt((timeValue - hours) * 60f);
            return $"{hours:D2}:{minutes:D2}";
        }

        private void TriggerWeatherEffects(string currentTime, int currentDay)
        {
            foreach (var effect in weatherEffects)
            {
                if (effect == null || effect.effectObject == null || effect.dailySchedules == null)
                {
                    Debug.LogWarning($"Weather effect is not properly configured: {effect?.effectName ?? "Unnamed Effect"}");
                    continue;
                }

                bool shouldActivateEffect = false;

                foreach (var schedule in effect.dailySchedules)
                {
                    if (!schedule.days.Contains(currentDay))
                        continue;

                    // Проверка активации эффекта по времени
                    if (IsTimeInRange(currentTime, schedule.startTriggerTime, schedule.endTriggerTime))
                    {
                        shouldActivateEffect = true;
                    }

                    // Проверка и изменение облачности
                    if (schedule.enableCloudCoverage)
                    {
                        if (skyboxMaterial != null && skyboxMaterial.HasProperty("_CloudCoverage1"))
                        {
                            float currentCoverage = skyboxMaterial.GetFloat("_CloudCoverage1");

                            if (IsTimeInRange(currentTime, schedule.startCloudCoverageTime, schedule.endCloudCoverageTime))
                            {
                                // Плавное изменение облачности к заданному значению
                                float targetCoverage = schedule.cloudCoverage;
                                skyboxMaterial.SetFloat("_CloudCoverage1",
                                    Mathf.Lerp(currentCoverage, targetCoverage, Time.deltaTime * cloudChangeSpeed));
                            }
                            else
                            {
                                // Возврат облачности к исходному значению за пределами заданного времени
                                skyboxMaterial.SetFloat("_CloudCoverage1",
                                    Mathf.Lerp(currentCoverage, defaultCloudCoverage, Time.deltaTime * cloudChangeSpeed));
                            }
                        }
                    }
                }

                // Активация или деактивация объекта эффекта
                if (shouldActivateEffect)
                {
                    if (!effect.effectObject.activeSelf && !effect.IsStopping())
                    {
                        Debug.Log($"Activating weather effect: {effect.effectName} at {currentTime} on day {currentDay}");
                        StartCoroutine(effect.ActivateEffect(skyboxMaterial, cloudChangeSpeed));
                    }
                }
                else
                {
                    if (effect.effectObject.activeSelf && !effect.IsStopping())
                    {
                        Debug.Log($"Deactivating weather effect: {effect.effectName} at {currentTime} on day {currentDay}");
                        StartCoroutine(effect.DeactivateEffect(skyboxMaterial, cloudChangeSpeed, defaultCloudCoverage));
                    }
                }
            }
        }

        private bool IsTimeInRange(string currentTime, string startTime, string endTime)
        {
            System.TimeSpan current = System.TimeSpan.Parse(currentTime);
            System.TimeSpan start = System.TimeSpan.Parse(startTime);
            System.TimeSpan end = System.TimeSpan.Parse(endTime);

            if (start <= end)
            {
                return current >= start && current <= end;
            }
            else
            {
                return current >= start || current <= end;
            }
        }

        private void OnApplicationQuit()
        {
            if (skyboxMaterial != null && skyboxMaterial.HasProperty("_CloudCoverage1"))
            {
                skyboxMaterial.SetFloat("_CloudCoverage1", defaultCloudCoverage);
            }
            
            // Reset water material to default values
            if (waterMaterial != null)
            {
                if (waterMaterial.HasProperty("_TranslucencyCurvatureMask"))
                {
                    waterMaterial.SetFloat("_TranslucencyCurvatureMask", defaultTranslucencyCurvature);
                }
                
                if (waterMaterial.HasProperty("_ReflectionFresnel"))
                {
                    waterMaterial.SetFloat("_ReflectionFresnel", defaultReflectionCurvature);
                }
            }
        }

        private void OnValidate()
        {
            if (currentTimeOfDay > 24f) currentTimeOfDay = 24f;
            if (currentTimeOfDay < 0f) currentTimeOfDay = 0f;

            if (currentDay < 1) currentDay = 1;
            if (currentDay > maxDay) currentDay = maxDay;

            timeDisplay = ConvertRangeToTime(currentTimeOfDay);
            
            // Only update position and FX if not using system time or if in editor
            if (!useSystemTime || !Application.isPlaying)
            {
                UpdatePosition();
                UpdateFX();
            }
        }
    }

    [System.Serializable]
    public class DailyEffectSchedule
    {
        [Tooltip("Comma-separated days to activate the effect, e.g., 1,2,5")]
        public List<int> days = new List<int>(); // Days of the month
        public string startTriggerTime; // Time to activate the effect
        public string endTriggerTime;   // Time to deactivate the effect

        public bool enableCloudCoverage = false;   // Enable/disable cloud coverage changes
        public string startCloudCoverageTime;      // Time to start cloud coverage adjustment
        public string endCloudCoverageTime;        // Time to end cloud coverage adjustment
        public float cloudCoverage = 0f;           // Desired cloud coverage value
    }

    [System.Serializable]
    public class WeatherEffect
    {
        public string effectName;
        public GameObject effectObject;
        public List<DailyEffectSchedule> dailySchedules;

        private bool isStopping = false;

        public IEnumerator ActivateEffect(Material skyboxMaterial, float changeSpeed)
        {
            if (effectObject == null || skyboxMaterial == null) yield break;

            isStopping = false;

            var particleSystems = effectObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                var mainModule = ps.main;
                mainModule.loop = true;
            }

            effectObject.SetActive(true);
        }

        public IEnumerator DeactivateEffect(Material skyboxMaterial, float changeSpeed, float defaultCloudCoverage)
        {
            if (effectObject == null || skyboxMaterial == null) yield break;

            isStopping = true;

            var particleSystems = effectObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                var mainModule = ps.main;
                mainModule.loop = false;
            }

            yield return new WaitUntil(CheckEffectComplete);
            effectObject.SetActive(false);

            isStopping = false;
        }

        public bool CheckEffectComplete()
        {
            if (effectObject == null) return true;

            var particleSystems = effectObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                if (ps.IsAlive(true)) return false;
            }

            return true;
        }

        public bool IsStopping()
        {
            return isStopping;
        }
    }
}