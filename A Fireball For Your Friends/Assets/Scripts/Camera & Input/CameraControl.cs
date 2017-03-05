using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(InputControl))]
public class CameraControl : MonoBehaviour {
    public float turnSpeedChange = 30f;
    public float movementSpeedMax = 1;
    public float minimalMouseDifferenceToChangeCamera = 30;
    private float turnSpeed = 0f;
    [RangeAttribute(0, 100)]
    public float height = 50;

    public DebugText debugPosition;

    private void OnValidate() {
        if (debugPosition == null) {
            throw new Exception("You need to attach a debug position");
        }
    }

    void FixedUpdate () {
      Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, transform.position + Vector3.up * height, movementSpeedMax);
      Camera.main.transform.LookAt(transform.position);
    }

    public void notUsedCamera() {
      Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, transform.position + Vector3.up * height, movementSpeedMax);

      //angle we need to turn
      Quaternion rotationToPlayer = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
      Quaternion rotationToMouse = Quaternion.LookRotation(GetComponent<InputControl>().getLookAtPosition() - Camera.main.transform.position);
      float angleDiff = Quaternion.Angle(rotationToPlayer, rotationToMouse);
      while (angleDiff < 0) {
        // Put angles in absolute value
        angleDiff += 360;
      }
      Quaternion rotationToTurn = Quaternion.Slerp(rotationToPlayer, rotationToPlayer, (angleDiff < minimalMouseDifferenceToChangeCamera ? 0 : 0.8f));
      float angleToTurn = Quaternion.Angle(Camera.main.transform.rotation, rotationToTurn);

      debugPosition.setInfo("RotationToPlayer", rotationToPlayer.ToString());
      debugPosition.setInfo("RotationToMouse", rotationToMouse.ToString());
      debugPosition.setInfo("AngleDiff", angleDiff.ToString());

      //speed is in degrees/sec = angle, to pass angle in 1 seconds. our speed can be increased only 'turnSpeedChange' degrees/sec^2, but don't increase it if needn't
      turnSpeed = Mathf.Min(angleToTurn, turnSpeed + turnSpeedChange * Time.fixedDeltaTime);

      Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToTurn, angleToTurn > 0 ? turnSpeed * Time.fixedDeltaTime / angleToTurn : 0f);
      Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.rotation * Vector3.forward * 100, Color.red);
    }
}
