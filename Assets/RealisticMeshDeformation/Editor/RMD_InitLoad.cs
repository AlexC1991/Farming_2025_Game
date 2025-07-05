//----------------------------------------------
//        Realistic Mesh Deformation
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
// Bugra Ozdoganlar
//
//----------------------------------------------
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RMD_InitLoad : EditorWindow {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    private static void EditorUpdate() {

        bool hasKey = false;

#if BCG_RMD
        hasKey = true;
#endif

        if (!hasKey) {

            RMD_SetScriptingSymbol.SetEnabled("BCG_RMD", true);
            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using RMD! Please read the documentation before use. Have fun :) ", "Let's get started!");

        }

    }

}
#endif