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

public class RMD {

    public static void Repair(RMD_Deformation deformedObject) {

        deformedObject.Repair();

    }

    public static void RepairAll() {

        RMD_Deformation[] allDeformations = GameObject.FindObjectsOfType<RMD_Deformation>();

        foreach (RMD_Deformation item in allDeformations)
            item.Repair();

    }

}
