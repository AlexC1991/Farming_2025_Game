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
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Mesh Deformation/RMD Camera Controller")]
public class RMD_CameraController : MonoBehaviour {

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    public float translateSpeed = 10f;
    public float rotateSpeed = 5f;
    public float smoothFactor = 5f;

    private Vector2 orbit = new Vector2(0f, 0f);

    public bool mobileController = false;
    public RMD_UIJoystick joystick;

    private void OnEnable() {
        Application.targetFrameRate = 60;
        orbit.x = transform.eulerAngles.x;
        orbit.y = transform.eulerAngles.y;

    }

    void Update() {

        if (!mobileController) {

            horizontalInput = Mathf.Lerp(horizontalInput, Input.GetAxisRaw("Horizontal"), Time.deltaTime * smoothFactor);
            verticalInput = Mathf.Lerp(verticalInput, Input.GetAxisRaw("Vertical"), Time.deltaTime * smoothFactor);

            if (Input.GetMouseButton(1)) {

                orbit.x -= Input.GetAxisRaw("Mouse Y") * rotateSpeed;
                orbit.y += Input.GetAxisRaw("Mouse X") * rotateSpeed;

            }

        } else {

            horizontalInput = joystick.inputHorizontal;
            verticalInput = joystick.inputVertical;

        }

        transform.rotation = Quaternion.Euler(orbit.x, orbit.y, 0f);

        transform.Translate(Vector3.right * horizontalInput * translateSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * verticalInput * translateSpeed * Time.deltaTime);

    }

    public void OnDrag(PointerEventData pointerData) {

        // Receiving drag input from UI.
        orbit.y += pointerData.delta.x * 10f / 100f;
        orbit.x -= pointerData.delta.y * 10f / 100f;

    }

}
