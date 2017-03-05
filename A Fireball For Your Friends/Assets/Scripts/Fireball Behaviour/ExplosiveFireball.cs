using UnityEngine;
using System.Collections;
using System;

public class ExplosiveFireball : ShootableFireball {
  public GameObject explosionOnImpact;

  new void OnCollisionEnter() {
    base.OnCollisionEnter();
    ParticleSystem explosion = GetComponentInChildren<Explosion>().GetComponent<ParticleSystem>();
    explosion.Play();
    GetComponent<MeshRenderer>().enabled = !shutDownMeshOnCollision;

    Instantiate(explosionOnImpact, gameObject.transform.position, Quaternion.identity, gameObject.transform);
  }

  internal override float timeToDestruction() {
    return GetComponentInChildren<Explosion>().GetComponent<ParticleSystem>().main.duration;
  }
}
