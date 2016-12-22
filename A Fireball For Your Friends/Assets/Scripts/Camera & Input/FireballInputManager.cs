using UnityEngine;
using System.Collections;

public abstract class FireballInputManager {
  public float timeBetweenShots = 2;

  private Fireball managedFireball;
  protected float timeOfNextShot = float.MinValue;

  public DebugText debugPosition;

  internal virtual void fireButtonPressed(Transform where, Fireball fireball, Vector3? fireTo = null) {
    if (managedFireball == null && timeOfNextShot < Time.time) {
      debugPosition.setInfo(this.GetType().ToString(), "Pressing button: instantiating");
      managedFireball = Object.Instantiate(fireball, where.position + where.up * 4, Quaternion.identity) as Fireball;
      managedFireball.transform.parent = where;
      timeOfNextShot = Time.time + timeBetweenShots;
    } 
    
    if (fireTo != null && managedFireball != null && managedFireball.GetType().Equals(typeof(ChargeableMultiNormalFireball))) {
      ChargeableMultiNormalFireball cast = (ChargeableMultiNormalFireball)managedFireball;
      cast.setTarget(fireTo);
    }
  }

  internal virtual void fireButtonReleased(Vector3 target) {
    if (managedFireball != null) {
      debugPosition.setInfo(this.GetType().ToString(), "Button released");
      managedFireball.fireTo(target);
      managedFireball = null;
    }
  }
}
