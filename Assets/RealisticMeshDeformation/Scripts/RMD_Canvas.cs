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
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Mesh Deformation/RMD Canvas")]
public class RMD_Canvas : MonoBehaviour {

    private RMD_Shooter shooter;

    public void SetRadius(Slider slider) {

        if (!shooter)
            shooter = FindObjectOfType<RMD_Shooter>();

        if (!shooter)
            return;

        shooter.SetRadius(slider.value);

    }

    public void SetMass(Slider slider) {

        if (!shooter)
            shooter = FindObjectOfType<RMD_Shooter>();

        if (!shooter)
            return;

        shooter.SetMass((int)slider.value);

    }

    public void Shoot() {

        if (!shooter)
            shooter = FindObjectOfType<RMD_Shooter>();

        if (!shooter)
            return;

        shooter.Fire();

    }

    public void RepairAll() {

        RMD_Deformation[] allDeformations = GameObject.FindObjectsOfType<RMD_Deformation>();

        foreach (RMD_Deformation item in allDeformations)
            item.Repair();

    }

}
