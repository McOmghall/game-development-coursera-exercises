using UnityEngine;

namespace VRCapture.Demo {

    public class DemoManager : MonoBehaviour {

        VRCapture vrCapture;

        void Start() {
            if (vrCapture == null) {
                vrCapture = VRCapture.instance;
            }
            vrCapture.RegisterSessionCompleteDelegate(HandleCaptureFinish);
            Application.runInBackground = true;
        }

        void OnGUI() {
            if (GUI.Button(new Rect(50, 50, 100, 50), "Capture Start")) {
                print("Capture Start");
                vrCapture.BeginCaptureSession();
            }

            if (GUI.Button(new Rect(50, 150, 100, 50), "Capture Stop")) {
                print("Capture Stop");
                vrCapture.EndCaptureSession();
            }
            if (GUI.Button(new Rect(50, 250, 400, 50), "Video Save File :" + vrCapture.folderPath)) {
                System.Diagnostics.Process.Start(vrCapture.folderPath);
            }
        }

        void HandleCaptureFinish() {
            print("Capture Finish");
        }
    }
}