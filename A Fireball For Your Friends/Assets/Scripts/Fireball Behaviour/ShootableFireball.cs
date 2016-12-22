using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Light))]
public abstract class ShootableFireball : Fireball {
  public bool shutDownMeshOnCollision = true;
  public float shootTime = 1;

  public Color colorOnExplosion;
  public bool changeFlickerColorOnDestruction = true;

  public float maxLightRange = 20;
  public float lightFlicker = 20;

  public bool modifyVelocityOnImpact = true;
  [HideInInspectorIf("showVelocityOnImpact")]
  public Vector3 velocityOnImpact = new Vector3(0, 0, 0);

  public bool showVelocityOnImpact() {
    return this.modifyVelocityOnImpact;
  }
  private Vector3 target;

  public bool usesGravity = true;
  public float maxExistenceTime = 5;

  protected float timeToBeDestroyed;
  protected override void setStatus(FireballStates value) {
    switch (value) {
      case FireballStates.CREATED:
        timeToBeDestroyed = float.MaxValue;
        if (GetComponent<Collider>() != null) {
          GetComponent<Collider>().enabled = false;
        }
        break;
      case FireballStates.SHOT:
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = usesGravity;
        GetComponentInChildren<SmokeTrail>().GetComponent<ParticleSystem>().Play();
        timeToBeDestroyed = Time.fixedTime + maxExistenceTime;
        break;
    }
    _status = value;
  }

  internal abstract float timeToDestruction();

  void Start() {
    setStatus(FireballStates.CREATED);
  }

  protected void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("Player")) {
      Debug.Log("Collided with non-enemy");
      return;
    }
    else {
      Debug.Log("Normal Collision");
      OnCollisionEnter();
    }
  }

  protected void OnCollisionEnter() {
    Debug.Log("Destroying fireball");
    Destroy(gameObject, timeToDestruction());
    GetComponent<MeshRenderer>().enabled = !shutDownMeshOnCollision;
    GetComponent<Rigidbody>().velocity = (modifyVelocityOnImpact ? velocityOnImpact : GetComponent<Rigidbody>().velocity);
    setStatus(FireballStates.TO_BE_DESTROYED);
  }

  protected void Update() {
    flicker();
    if (timeToBeDestroyed < Time.fixedTime) {
      OnCollisionEnter();
    }
  }

  protected void flicker() {
    Light light = GetComponent<Light>();
    if (changeFlickerColorOnDestruction && status.Equals(FireballStates.TO_BE_DESTROYED)) {
      light.range = maxLightRange;
      light.color = colorOnExplosion;
      light.intensity = 8;
    }
    else {
      light.range = Mathf.Sin(Time.deltaTime * lightFlicker) * (maxLightRange / 2) + maxLightRange;
      light.intensity = Mathf.Sin(Time.deltaTime * lightFlicker * 4) * 4 + 8;
    }
  }
  internal void setTarget(Vector3 target) {
    this.target = target;
    transform.LookAt(target);
  }

  internal override void fireTo(Vector3 target) {
    setTarget(target);
    GetComponent<Rigidbody>().velocity = calculateBestThrowSpeed(transform.position, target, shootTime);
    setStatus(FireballStates.SHOT);
  }

  private Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget) {
    // calculate vectors
    Vector3 toTarget = target - origin;
    Vector3 toTargetXZ = toTarget;
    toTargetXZ.y = 0;

    // calculate xz and y
    float y = toTarget.y;
    float xz = toTargetXZ.magnitude;

    // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
    // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
    // so xz = v0xz * t => v0xz = xz / t
    // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
    float t = timeToTarget;
    float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
    float v0xz = xz / t;

    // create result vector for calculated starting speeds
    Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
    result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
    result.y = v0y;                                // set y to v0y (starting speed of y plane)

    return result;
  }
}