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

[AddComponentMenu("BoneCracker Games/Realistic Mesh Deformation/RMD Camera Shooter")]
public class RMD_Shooter : MonoBehaviour {

    public bool canFire = true;
    public bool mobileController = false;

    public Rigidbody projectile;
    public float force = 75f;
    public float lifeTime = 3f;
    public float radius = 1f;
    public int mass = 1000;

    void Update() {

        if (!mobileController) {

            if (Input.GetMouseButtonDown(0))
                Fire();

            if (Input.GetKeyDown(KeyCode.Space))
                Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked);

        }

    }

    public void Fire() {

        if (!canFire)
            return;

        Rigidbody rigid = Instantiate(projectile, transform.position, transform.rotation);
        rigid.AddRelativeForce(Vector3.forward * force, ForceMode.VelocityChange);
        rigid.transform.localScale *= radius;
        rigid.mass = mass;
        Destroy(rigid.gameObject, lifeTime);

    }

    public void SetRadius(float newRadius) {

        radius = newRadius;

    }

    public void SetMass(int newMass) {

        mass = newMass;

    }

}
