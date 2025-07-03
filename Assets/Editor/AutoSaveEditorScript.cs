using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class AutoSaveEditorScript : EditorWindow
{
    // --- Inspector Properties ---
    [Header("Auto-Save Settings")]
    [Tooltip("Enable or disable the auto-save functionality.")]
    public bool autoSaveEnabled = true;

    [Tooltip("Interval in seconds between auto-saves.")]
    public float saveIntervalSeconds = 120f; // Default to 2 minutes

    [Tooltip("Save the current scene(s) automatically.")]
    public bool saveScenes = true;

    [Tooltip("Save project assets automatically.")]
    public bool saveAssets = true;

    [Tooltip("Show notification when auto-save occurs.")]
    public bool showNotifications = true;

    // --- Internal Variables ---
    private double timeSinceLastSave = 0.0;
    private double lastSaveTime = 0.0;
    private double lastUpdateTime = 0.0;

    // --- Editor Window Initialization ---
    [MenuItem("Tools/Auto-Saver")]
    public static void ShowWindow()
    {
        GetWindow<AutoSaveEditorScript>("Auto-Saver");
    }

    // --- GUI ---
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Auto-Save Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        autoSaveEnabled = EditorGUILayout.Toggle("Auto-Save Enabled", autoSaveEnabled);
        
        EditorGUI.BeginDisabledGroup(!autoSaveEnabled);
        saveIntervalSeconds = EditorGUILayout.FloatField("Save Interval (seconds)", Mathf.Max(10f, saveIntervalSeconds));
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Save Options", EditorStyles.boldLabel);
        saveScenes = EditorGUILayout.Toggle("Save Scenes", saveScenes);
        saveAssets = EditorGUILayout.Toggle("Save Assets", saveAssets);
        showNotifications = EditorGUILayout.Toggle("Show Notifications", showNotifications);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        // Display time until next save
        if (autoSaveEnabled)
        {
            double timeUntilSave = saveIntervalSeconds - timeSinceLastSave;
            if (timeUntilSave > 0)
            {
                EditorGUILayout.LabelField($"Next auto-save in: {timeUntilSave:F1} seconds");
            }
            else
            {
                EditorGUILayout.LabelField("Auto-save pending...");
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Perform Manual Save"))
        {
            PerformSave(true);
        }

        EditorGUILayout.Space();
        
        // Status information
        if (lastSaveTime > 0)
        {
            string lastSaveTimeStr = System.DateTime.FromOADate(lastSaveTime).ToString("HH:mm:ss");
            EditorGUILayout.LabelField($"Last save: {lastSaveTimeStr}");
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("This script automatically saves your Unity project scenes and assets at the specified interval, just like using File > Save.", MessageType.Info);

        // Save preferences when GUI changes
        if (GUI.changed)
        {
            SavePreferences();
        }
    }

    // --- Unity Editor Callbacks ---
    private void OnEnable()
    {
        LoadPreferences();
        EditorApplication.update += EditorUpdate;
        
        // Initialize timing variables
        lastUpdateTime = EditorApplication.timeSinceStartup;
        timeSinceLastSave = 0.0;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
        SavePreferences();
    }

    // --- Core Logic ---
    private void EditorUpdate()
    {
        if (!autoSaveEnabled || saveIntervalSeconds <= 0)
        {
            return;
        }

        // Use EditorApplication.timeSinceStartup for more reliable timing
        double currentTime = EditorApplication.timeSinceStartup;
        double deltaTime = currentTime - lastUpdateTime;
        lastUpdateTime = currentTime;

        timeSinceLastSave += deltaTime;

        if (timeSinceLastSave >= saveIntervalSeconds)
        {
            PerformSave(false);
            timeSinceLastSave = 0.0;
        }
    }

    private void PerformSave(bool isManual = false)
    {
        if (!autoSaveEnabled && !isManual)
        {
            return;
        }

        bool savedSomething = false;
        string saveMessage = "";

        try
        {
            // Save scenes (equivalent to File > Save or Ctrl+S)
            if (saveScenes)
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.SaveOpenScenes();
                    savedSomething = true;
                    saveMessage += "Scenes ";
                }
            }

            // Save assets (equivalent to File > Save Project or Ctrl+Alt+S)
            if (saveAssets)
            {
                AssetDatabase.SaveAssets();
                savedSomething = true;
                saveMessage += "Assets ";
            }

            if (savedSomething)
            {
                lastSaveTime = System.DateTime.Now.ToOADate();
                string prefix = isManual ? "Manual save" : "Auto-save";
                string fullMessage = $"{prefix} completed: {saveMessage.Trim()}";
                
                Debug.Log($"[{System.DateTime.Now:HH:mm:ss}] {fullMessage}");
                
                if (showNotifications)
                {
                    ShowNotification(new GUIContent(fullMessage));
                }
            }
            else if (isManual)
            {
                string message = "No unsaved changes detected";
                Debug.Log(message);
                if (showNotifications)
                {
                    ShowNotification(new GUIContent(message));
                }
            }
        }
        catch (System.Exception e)
        {
            string errorMessage = $"Save failed: {e.Message}";
            Debug.LogError(errorMessage);
            
            if (showNotifications)
            {
                ShowNotification(new GUIContent(errorMessage));
            }
        }
    }

    // --- Preferences Management ---
    private void SavePreferences()
    {
        EditorPrefs.SetBool("AutoSave_Enabled", autoSaveEnabled);
        EditorPrefs.SetFloat("AutoSave_Interval", saveIntervalSeconds);
        EditorPrefs.SetBool("AutoSave_SaveScenes", saveScenes);
        EditorPrefs.SetBool("AutoSave_SaveAssets", saveAssets);
        EditorPrefs.SetBool("AutoSave_ShowNotifications", showNotifications);
    }

    private void LoadPreferences()
    {
        autoSaveEnabled = EditorPrefs.GetBool("AutoSave_Enabled", true);
        saveIntervalSeconds = EditorPrefs.GetFloat("AutoSave_Interval", 120f);
        saveScenes = EditorPrefs.GetBool("AutoSave_SaveScenes", true);
        saveAssets = EditorPrefs.GetBool("AutoSave_SaveAssets", true);
        showNotifications = EditorPrefs.GetBool("AutoSave_ShowNotifications", true);
    }
}