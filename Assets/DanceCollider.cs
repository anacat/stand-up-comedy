using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceCollider : MonoBehaviour
{

   private void OnTriggerEnter(Collider other)
   {

        PlayerController.instance.movesScore += 10;

   }
}
