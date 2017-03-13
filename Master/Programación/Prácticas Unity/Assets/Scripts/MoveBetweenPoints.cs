using UnityEngine;
using System;
using System.Collections;

public class MoveBetweenPoints : MonoBehaviour {
    public float movementSpeedPerSecond = 10f;
    public float growScaleTotal = 2;
    public float rotateSpeedDegreesPerSecond = 90;
    public Vector3[] stopPoints = { new Vector3(-20.0f, 0f, 0f), new Vector3(-20f, 0f, 20f), new Vector3(20f, 0f, 20f), new Vector3(20f, 0f, 0f) };
    public float precisionThreshold = 0.1f;
    private MovementStates state = MovementStates.STARTING;
    private Vector3 initialScale;

    void Start () {
	    if (stopPoints == null || stopPoints.Length != 4) {
            throw new Exception("Stop Points array needs to have 4 elements");
        }

        initialScale = transform.localScale;
	}

	void Update () {
        Vector3 currentTarget;
        MovementStates nextState;
	    switch (state) {
            case MovementStates.STARTING:
                currentTarget = stopPoints[0];
                nextState = MovementStates.GROWING;
                break;
            case MovementStates.GROWING:
                currentTarget = stopPoints[1];
                transform.localScale = initialScale * Mathf.Lerp(growScaleTotal, 1, (Vector3.Distance(transform.position, stopPoints[1]) / Vector3.Distance(stopPoints[0], stopPoints[1])));
                nextState = MovementStates.SHRINKING;
                break;
            case MovementStates.SHRINKING:
                currentTarget = stopPoints[2];
                transform.localScale = initialScale * Mathf.Lerp(1, growScaleTotal, (Vector3.Distance(transform.position, stopPoints[2]) / Vector3.Distance(stopPoints[1], stopPoints[2])));
                nextState = MovementStates.ROTATING;
                break;
            case MovementStates.ROTATING:
                currentTarget = stopPoints[3];
                transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotateSpeedDegreesPerSecond * Time.deltaTime);
                nextState = MovementStates.ROTATING_REVERSE;
                break;
            case MovementStates.ROTATING_REVERSE:
                currentTarget = stopPoints[0];
                transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f), rotateSpeedDegreesPerSecond * Time.deltaTime);
                nextState = MovementStates.GROWING;
                break;
            default:
                throw new Exception("WTF");
        }

        if (Vector3.Distance(transform.position, currentTarget) < precisionThreshold) {
            state = nextState;
        } else {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, movementSpeedPerSecond * Time.deltaTime);
        }
    }

    enum MovementStates {
        STARTING,
        GROWING,
        SHRINKING,
        ROTATING,
        ROTATING_REVERSE
    }
}
