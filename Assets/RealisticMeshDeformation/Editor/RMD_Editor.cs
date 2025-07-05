//----------------------------------------------
//        Realistic Mesh Deformation
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
// Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects()]
[CustomEditor(typeof(RMD_Deformation))]
public class RMD_Editor : Editor {

    RMD_Deformation prop;
    bool expandMeshes = false;
    Color defColor;

    private void Awake() {

        defColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (RMD_Deformation)target;
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deformationMode"), new GUIContent("Deformation Mode", "Fast = Instant bending. Smooth = Smooth bending."));
        prop.deformResolution = EditorGUILayout.IntSlider(new GUIContent("Deformation Resolution", "Determines how many vertices will be processed."), prop.deformResolution, 1, 100);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("deformRadius"), new GUIContent("Deformation Radius", "Deformation radius size."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deformMultiplier"), new GUIContent("Deformation Multiplier", "Deformation multiplier factor. Higher deformation on higher values."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("randomizeVertices"), new GUIContent("Randomize Vertices", "Randomizes positions of the vertices."));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumDeformation"), new GUIContent("Maximum Deformation Limit", "Maximum deformation limit."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumDeformationImpulse"), new GUIContent("Minimum Deformation Impulse", "Minimum deformation impulse."));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("recalculateNormals"), new GUIContent("Recalculate Normals", "Recalculate normals of the deformed meshes. May reduce performance on higher poly models."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("recalculateBounds"), new GUIContent("Recalculate Bounds", "Recalculate bounds of the deformed meshes. Not much has any effects on the performance."));
        EditorGUILayout.Space();

        if (Selection.gameObjects.Length > 1) {

            for (int i = 0; i < Selection.gameObjects.Length; i++)
                Selection.gameObjects[i].GetComponent<RMD_Deformation>().deformResolution = prop.deformResolution;

        }

        if (Selection.gameObjects.Length <= 1) {

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.Space();

            expandMeshes = EditorGUILayout.Foldout(expandMeshes, "Collected Meshes");

            if (expandMeshes) {

                EditorGUI.indentLevel++;

                for (int i = 0; i < prop.meshFilters.Length; i++) {

                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.BeginHorizontal();

                    if (prop.meshFilters[i]) {

                        if (prop.meshFilters[i].sharedMesh) {

                            EditorGUILayout.LabelField(prop.meshFilters[i].name);

                        } else {

                            GUI.color = Color.red;
                            EditorGUILayout.LabelField("Missing mesh of the " + prop.meshFilters[i].name + "!");

                        }

                    } else {

                        GUI.color = Color.red;
                        EditorGUILayout.LabelField("Missing meshfilter!");

                    }

                    GUI.color = Color.red;

                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                        RemoveMeshAtIndex(i);

                    GUI.color = defColor;

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                }

                EditorGUI.indentLevel--;

            }

            if (!EditorApplication.isPlaying) {

                CheckAllMeshes();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Add All Meshes"))
                    AddAllMeshes();

                EditorGUILayout.EndHorizontal();

            } else {

                if (GUILayout.Button("Repair"))
                    prop.Repair();

            }

            EditorGUILayout.EndVertical();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("repairState"), new GUIContent("Repair State", "Currently repairing the mesh or not?"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("deformState"), new GUIContent("Deform State", "Currently deforming the mesh or not?"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("latestProcessedVertexCount"), new GUIContent("Latest Processed Vertex Count", "Latest Processed Vertex Count"));
            GUI.enabled = true;

        }

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();


    }

    private void RemoveMeshAtIndex(int index) {

        List<MeshFilter> currentMeshFilters = new List<MeshFilter>();

        for (int i = 0; i < prop.meshFilters.Length; i++)
            currentMeshFilters.Add(prop.meshFilters[i]);

        if (currentMeshFilters[index] && currentMeshFilters[index].gameObject.GetComponent<RMD_CollisionInvoker>())
            DestroyImmediate(currentMeshFilters[index].gameObject.GetComponent<RMD_CollisionInvoker>());

        currentMeshFilters.RemoveAt(index);
        prop.meshFilters = currentMeshFilters.ToArray();

    }

    public void AddAllMeshes() {

        MeshFilter[] allMeshFilters = prop.gameObject.GetComponentsInChildren<MeshFilter>(true);
        List<MeshFilter> properMeshFilters = new List<MeshFilter>();

        foreach (MeshFilter mf in allMeshFilters) {

            if (mf.sharedMesh != null)
                properMeshFilters.Add(mf);

        }

        allMeshFilters = properMeshFilters.ToArray();
        prop.meshFilters = allMeshFilters;
        expandMeshes = true;

    }

    private void CheckAllMeshes() {

        for (int i = 0; i < prop.meshFilters.Length; i++) {

            if (prop.meshFilters[i] && prop.meshFilters[i].sharedMesh && !prop.meshFilters[i].sharedMesh.isReadable)
                EditorGUILayout.HelpBox("Mesh of the " + prop.meshFilters[i].sharedMesh.name + " is not readable. Go to your model settings and enable ''Read/Write'' option.", MessageType.Error);

            if (!prop.meshFilters[i])
                EditorGUILayout.HelpBox("Meshfilter is missing! Remove it from the list.", MessageType.Error);

            if (prop.meshFilters[i] && !prop.meshFilters[i].sharedMesh)
                EditorGUILayout.HelpBox("Mesh of the " + prop.meshFilters[i].name + " is missing! Remove it from the list, or assign a mesh for this meshfilter.", MessageType.Error);

            if (prop.meshFilters[i] && prop.meshFilters[i].gameObject.isStatic)
                EditorGUILayout.HelpBox("Meshfilter is static! Disable batching static of the mesh named " + prop.meshFilters[i].name + "!", MessageType.Error);

        }

    }

}
