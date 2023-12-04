using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D c0){

    Destroy(gameObject);
    
  }
}
