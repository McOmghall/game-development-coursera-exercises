using UnityEngine;
using System;

public class RotateClockHands : MonoBehaviour {
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    // Update is called once per frame
    void Update () {
        DateTime time = DateTime.Now;
        //Debug.Log(time + "->" + (time.Hour % 12) * (360 / 12) + "," + (time.Minute % 60) * (360 / 60) + "," + (time.Second % 60) * (360 / 60));
        hourHand.localRotation = Quaternion.Euler(0, (time.Hour % 12) * (360 / 12), 0);
        minuteHand.localRotation = Quaternion.Euler(0, (time.Minute % 60) * (360 / 60), 0);
        secondHand.localRotation = Quaternion.Euler(0, (time.Second % 60) * (360 / 60), 0);
    }
}
