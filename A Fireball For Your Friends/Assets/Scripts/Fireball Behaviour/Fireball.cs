using UnityEngine;

public abstract class Fireball : MonoBehaviour {
  public enum FireballStates {
    CREATED,
    SHOT,
    TO_BE_DESTROYED
  }
  protected FireballStates _status;
  protected virtual FireballStates status { get { return _status; } set { _status = status; } }

  protected virtual void setStatus(FireballStates value) {
    _status = value;
  }

  internal abstract void fireTo(Vector3 target);
}