using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class InputControl : MonoBehaviour {

  private Vector3 movementDirection;
  // the world-relative desired move direction, calculated from the camForward and user input.

  private Vector3 camForward; // The current forward direction of the camera
  private bool jump; // whether the jump button is currently pressed

  public MovementInputData movementInputData;
  private Vector3 fireballTo;
  private Vector3 lookAt;
  private ShootableFireball firing;
  private float nextFireballTime = 0;
  private float turnSpeed = 0;
  private bool fireButtonPressed = false;
  private bool secondaryFireButtonPressed = false;

  public ExplosiveFireball explosiveFireballPrefab;
  public ChargeableMultiNormalFireball fireballPrefab;
  public NormalFireballInputManager normalFireballInputManager;
  public ExplosiveFireballInputManager explosiveFireballInputManager;

  public DebugText debugPosition;

  private void OnValidate() {
    if (Camera.main == null) {
      throw new Exception("You need to set a main camera");
    }
    if (explosiveFireballPrefab == null) {
      throw new Exception("You need to attach an explosive fireball prefab");
    }
    if (fireballPrefab == null) {
      throw new Exception("You need to attach a fireball prefab");
    }
    if (debugPosition == null) {
      throw new Exception("You need to attach a debug position");
    }
  }

  private void Update() {
    float h = CrossPlatformInputManager.GetAxis("Horizontal");
    float v = CrossPlatformInputManager.GetAxis("Vertical");
    jump = CrossPlatformInputManager.GetButton("Jump");
    bool fireButtonCheck = CrossPlatformInputManager.GetButton("Fire1");
    bool secondFireButtonCheck = CrossPlatformInputManager.GetButton("Fire2");

    camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    movementDirection = (v * camForward + h * Camera.main.transform.right).normalized;
    float distanceToHit;
    Ray mouseDirection = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);
    if (new Plane(Vector3.up, Vector3.zero).Raycast(mouseDirection, out distanceToHit)) {
      fireballTo = mouseDirection.GetPoint(distanceToHit);
      lookAt = fireballTo + Vector3.up;
      debugPosition.setInfo("Mouse", CrossPlatformInputManager.mousePosition.ToString());
      debugPosition.setInfo("Fireball", fireballTo.ToString());
      debugPosition.setInfo("Direction", mouseDirection.ToString());
      Debug.DrawRay(fireballTo, Vector3.up, Color.blue);
    }

    // Shooting control, primary fire takes precedence
    if (fireButtonCheck) {
      fireButtonPressed = true;
      normalFireballInputManager.fireButtonPressed(transform, fireballPrefab, fireballTo);
    } 
    
    if (secondFireButtonCheck) {
      secondaryFireButtonPressed = true;
      explosiveFireballInputManager.fireButtonPressed(transform, explosiveFireballPrefab);
    }

    // Handling of releasing fire buttons
    if (fireButtonPressed && !fireButtonCheck) {
      fireButtonPressed = false;
      normalFireballInputManager.fireButtonReleased(fireballTo);
    }

    if (secondaryFireButtonPressed && !secondFireButtonCheck) {
      secondaryFireButtonPressed = false;
      explosiveFireballInputManager.fireButtonReleased(fireballTo);
    }
  }


  private void FixedUpdate() {
    move();
  }

  public void move() {
    transform.position += movementDirection * movementInputData.movementSpeed * Time.deltaTime;

    Quaternion rotationToTurn = Quaternion.LookRotation(lookAt - transform.position);
    float angleToTurn = Quaternion.Angle(transform.rotation, rotationToTurn);
    //speed is in degrees/sec = angle, to pass angle in 1 seconds. our speed can be increased only 'turnSpeedChange' degrees/sec^2, but don't increase it if needn't
    turnSpeed = Mathf.Min(angleToTurn, turnSpeed + movementInputData.turnSpeedChange * Time.fixedDeltaTime);

    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    transform.rotation = Quaternion.Lerp(transform.rotation, rotationToTurn, angleToTurn > 0 ? turnSpeed * Time.fixedDeltaTime / angleToTurn : 0f);
    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    Debug.DrawRay(transform.position, transform.rotation * Vector3.forward, Color.red);
  }

  public Vector3 getLookAtPosition() {
    return lookAt;
  }
}

