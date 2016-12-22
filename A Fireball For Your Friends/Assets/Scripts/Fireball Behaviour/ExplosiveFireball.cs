using UnityEngine;
using System.Collections;
using System;

public class ExplosiveFireball : ShootableFireball {
  new void OnCollisionEnter() {
    base.OnCollisionEnter();
    ParticleSystem explosion = GetComponentInChildren<Explosion>().GetComponent<ParticleSystem>();
    explosion.Play();
    GetComponent<MeshRenderer>().enabled = !shutDownMeshOnCollision;
  }

  internal override float timeToDestruction() {
    return GetComponentInChildren<Explosion>().GetComponent<ParticleSystem>().duration;
  }
}
