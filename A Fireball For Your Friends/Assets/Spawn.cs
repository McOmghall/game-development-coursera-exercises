using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
  public GameObject spawn;
  public float spawnPeriodSeconds = 2;

  private float nextSpawn = float.MinValue;
  
	void Update () {
	  if (Time.time > nextSpawn) {
      Instantiate(spawn, transform.position, transform.rotation, null);
      nextSpawn = Time.time + spawnPeriodSeconds;
    }	
	}
}
