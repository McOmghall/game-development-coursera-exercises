using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
internal class Health : MonoBehaviour {
  public float healthAmount = 100f;
  public bool destroyOnDeath = true;

  private float startingHealth;

  void Start () {
    startingHealth = healthAmount;
  }

  public void damage(float damageAmount) {
    healthAmount -= damageAmount;
  }

  void Update () {
    if (destroyOnDeath && healthAmount < 0) {
      Destroy(gameObject);
    }
  }

  public float healthPercentage () {
    return healthAmount / startingHealth;
  }

  public float healthLost () {
    return startingHealth - healthAmount;
  }
}