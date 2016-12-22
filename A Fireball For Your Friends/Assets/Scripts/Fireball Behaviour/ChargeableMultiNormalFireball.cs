using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ChargeableMultiNormalFireball : Fireball {
  public ShootableFireball managedFireballPrefab = new NormalFireball();
  public float rotationDegreesPerSecond = 45;
  public float fireCircleRadius = 3;
  public float maxChargedFireballs = 5;
  public AnimationCurve timeBetweeenFireballsOverTime = AnimationCurve.EaseInOut(0, 1, 100, 0);
  public float timeBetweenFireballReleases = 0.2f;

  private float nextFireball = float.MinValue;
  private Vector3 target;

  void FixedUpdate() {
    transform.Rotate(transform.up, rotationDegreesPerSecond * Time.deltaTime);
    switch (status) {
      case FireballStates.CREATED:
        if (Time.time > nextFireball && transform.childCount < maxChargedFireballs) {
          nextFireball = Time.time + timeBetweeenFireballsOverTime.Evaluate(transform.childCount * (MaxDefinedFireball() / maxChargedFireballs));
          Quaternion rotation = Quaternion.Euler(0, transform.childCount * (360 / maxChargedFireballs), 0);
          ShootableFireball managedFireball = UnityEngine.Object.Instantiate(managedFireballPrefab, transform.position + rotation * transform.forward * fireCircleRadius, transform.rotation) as ShootableFireball;
          managedFireball.transform.parent = transform;
        } else if (transform.childCount == maxChargedFireballs) {
          Transform shooting = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount + 1));
          shooting.GetComponent<Fireball>().fireTo(target);
          shooting.parent = null;
        }
        break;
      case FireballStates.SHOT:
        if (transform.childCount == 0) {
          Destroy(gameObject);
        } else if (Time.time > nextFireball) {
          List<Fireball> fireballs = GetComponentsInChildren<Fireball>().Where((Fireball b) => b.transform.GetInstanceID() != transform.GetInstanceID()).ToList();
          Fireball f = fireballs[Mathf.CeilToInt(UnityEngine.Random.Range(0, fireballs.Count))];
          f.fireTo(target);
          f.transform.parent = null;
          nextFireball = Time.time + timeBetweenFireballReleases;
        }
        break;
    }
  }

  internal void setTarget(Vector3? fireTo) {
    if (fireTo != null) {
      this.target = fireTo.GetValueOrDefault();
    }
  }

  private float MaxDefinedFireball() {
    return new List<Keyframe>(timeBetweeenFireballsOverTime.keys).Select<Keyframe, float>((Keyframe k) => k.time).Max();
  }

  internal override void fireTo(Vector3 target) {
    setStatus(FireballStates.SHOT);
    this.target = target;
    nextFireball = float.MinValue;
  }
}