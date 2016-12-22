
using System;
using UnityEngine;

[System.Serializable]
public class MovementInputData {
  public float movementSpeed = 3;
  public float turnSpeedChange = 30;
  public AnimationCurve accelerationExtra = MovementInputData.DefaultAccelerationDecay();

  private static AnimationCurve DefaultAccelerationDecay() {
    AnimationCurve defaultCurve = AnimationCurve.EaseInOut(0, 10, 10, 0);
    defaultCurve.postWrapMode = WrapMode.ClampForever;
    defaultCurve.preWrapMode = WrapMode.ClampForever;
    return defaultCurve;
  }
}