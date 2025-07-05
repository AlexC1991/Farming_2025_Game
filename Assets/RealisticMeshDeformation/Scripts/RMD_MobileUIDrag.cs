//----------------------------------------------
//        Realistic Mesh Deformation
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
// Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Mobile UI Drag used for orbiting RCC Camera.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Mesh Deformation/RMD Mobile Drag")]
public class RMD_MobileUIDrag : MonoBehaviour, IDragHandler, IEndDragHandler {

    private RMD_CameraController cameraController;

    public void OnDrag(PointerEventData data) {

        if (!cameraController)
            cameraController = FindObjectOfType<RMD_CameraController>();

        if (!cameraController)
            return;

        cameraController.OnDrag(data);

    }

    public void OnEndDrag(PointerEventData data) {



    }

}
