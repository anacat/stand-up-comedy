using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceCollider : MonoBehaviour
{

   private void OnTriggerEnter(Collider other)
   {
      Debug.Log("owie", other.gameObject);

   }
}
