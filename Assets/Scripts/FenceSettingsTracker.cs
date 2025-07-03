using System.Collections.Generic;
using UnityEngine;

namespace farming2025
{
    [CreateAssetMenu(fileName = "FenceSettingsTracker", menuName = "Farming2025/Fence Settings Tracker")]
    public class FenceSettingsTracker : ScriptableObject
    {
        [System.Serializable]
        public class FenceSettings
        {
            public int countPositiveX = 1;
            public int countNegativeX = 1;
            public int countPositiveZ = 1;
            public int countNegativeZ = 1;
            
            public FenceSettings() { }
            
            public FenceSettings(int posX, int negX, int posZ, int negZ)
            {
                countPositiveX = posX;
                countNegativeX = negX;
                countPositiveZ = posZ;
                countNegativeZ = negZ;
            }
            
            public override string ToString()
            {
                return $"PosX:{countPositiveX}, NegX:{countNegativeX}, PosZ:{countPositiveZ}, NegZ:{countNegativeZ}";
            }
        }
        
        [System.Serializable]
        public class PositionSettingsPair
        {
            public Vector3 position;
            public FenceSettings settings;
            public string debugInfo;
            
            public PositionSettingsPair(Vector3 pos, FenceSettings set)
            {
                position = pos;
                settings = set;
                debugInfo = $"Pos: {pos} | Settings: {set}";
            }
        }
        
        [Header("Debug - Current Tracked Settings")]
        [SerializeField] private List<PositionSettingsPair> debugTrackedSettings = new List<PositionSettingsPair>();
        
        // Runtime dictionary for fast lookups
        private Dictionary<Vector3, FenceSettings> runtimeSettings = new Dictionary<Vector3, FenceSettings>();
        
        private Vector3 RoundPosition(Vector3 position)
        {
            return new Vector3(
                Mathf.Round(position.x * 4) / 4, 
                Mathf.Round(position.y * 4) / 4, 
                Mathf.Round(position.z * 4) / 4
            );
        }
        
        public void RecordSettings(Vector3 position, int posX, int negX, int posZ, int negZ)
        {
            Vector3 roundedPos = RoundPosition(position);
            FenceSettings newSettings = new FenceSettings(posX, negX, posZ, negZ);
            
            runtimeSettings[roundedPos] = newSettings;
            
            // Update debug list
            UpdateDebugList();
            
            Debug.Log($"[FenceSettingsTracker] Recorded settings at {roundedPos}: {newSettings}");
        }
        
        public FenceSettings GetSettings(Vector3 position)
        {
            Vector3 roundedPos = RoundPosition(position);
            if (runtimeSettings.ContainsKey(roundedPos))
            {
                FenceSettings found = runtimeSettings[roundedPos];
                Debug.Log($"[FenceSettingsTracker] Found settings at {roundedPos}: {found}");
                return found;
            }
            
            Debug.Log($"[FenceSettingsTracker] No settings found at {roundedPos}");
            return null;
        }
        
        public void RemoveSettings(Vector3 position)
        {
            Vector3 roundedPos = RoundPosition(position);
            if (runtimeSettings.Remove(roundedPos))
            {
                Debug.Log($"[FenceSettingsTracker] Removed settings at {roundedPos}");
                UpdateDebugList();
            }
        }
        
        public void ClearAll()
        {
            runtimeSettings.Clear();
            debugTrackedSettings.Clear();
            Debug.Log("[FenceSettingsTracker] Cleared all settings");
        }
        
        private void UpdateDebugList()
        {
            debugTrackedSettings.Clear();
            foreach (var kvp in runtimeSettings)
            {
                debugTrackedSettings.Add(new PositionSettingsPair(kvp.Key, kvp.Value));
            }
        }
        
        [ContextMenu("Print All Settings")]
        public void PrintAllSettings()
        {
            Debug.Log($"[FenceSettingsTracker] Total tracked positions: {runtimeSettings.Count}");
            foreach (var kvp in runtimeSettings)
            {
                Debug.Log($"Position: {kvp.Key} | Settings: {kvp.Value}");
            }
        }
    }
}