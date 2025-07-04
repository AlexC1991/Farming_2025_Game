using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Biostart.DayNight
{
    public class DayNightCycleWeather : MonoBehaviour
    {
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

            SetSunRotation(currentTimeOfDay);
            InvokeRepeating("UpdateCycle", 0f, 0.01f);
        }

        private void Update()
        {
            timeDisplay = ConvertRangeToTime(currentTimeOfDay);
            TriggerWeatherEffects(timeDisplay, currentDay);

            if (enableCloudCoverage)
            {
                UpdateRandomCloudCoverage();
            }
        }

        public void UpdateCycle()
        {
            UpdatePosition();
            UpdateFX();

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
            float sunIntensity = sunIntensityCurve.Evaluate(currentTimeOfDay / 24f);
            float moonIntensity = moonIntensityCurve.Evaluate(currentTimeOfDay / 24f);

            sun.intensity = sunIntensity;
            moon.intensity = moonIntensity;

            Color currentColor = dayColor.Evaluate(currentTimeOfDay / 24f);
            sun.color = currentColor;
            RenderSettings.ambientLight = currentColor;

            RenderSettings.fogColor = nightDayFogColor.Evaluate(currentTimeOfDay / 24f);
            RenderSettings.fogDensity = fogDensityCurve.Evaluate(currentTimeOfDay / 24f) * fogScale;

            moon.color = nightColor.Evaluate(currentTimeOfDay / 24f);
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
        }

        private void OnValidate()
        {
            if (currentTimeOfDay > 24f) currentTimeOfDay = 24f;
            if (currentTimeOfDay < 0f) currentTimeOfDay = 0f;

            if (currentDay < 1) currentDay = 1;
            if (currentDay > maxDay) currentDay = maxDay;

            timeDisplay = ConvertRangeToTime(currentTimeOfDay);
            UpdatePosition();
            UpdateFX();
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
