using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

namespace VRCapture {

    [RequireComponent(typeof(AudioListener))]
    public class VRCaptureAudio : MonoBehaviour {

        [DllImport("vrcapture-lib")]
        static extern System.IntPtr LibAudioCaptureAPI_Get(int rate, string path, string ffpath);

        [DllImport("vrcapture-lib")]
        static extern void LibAudioCaptureAPI_SendFrame(System.IntPtr api, byte[] data);

        [DllImport("vrcapture-lib")]
        static extern void LibAudioCaptureAPI_Close(System.IntPtr api);

        /// <summary>
        /// Audio capture configurations.
        /// TODO, check configuration correct.
        /// </summary>
        string folderPath;
        string filePath;

        /// <summary>
        /// Whether or not capturing from this audio listener is currently in progress.
        /// </summary>
        bool isCapturing;
        /// <summary>
        /// Reference to native lib API.
        /// </summary>
        System.IntPtr libAPI;
        /// <summary>
        /// The audio capture prepare.
        /// </summary>
        System.IntPtr audioPointer;
        System.Byte[] audioByteBuffer;

        /// <summary>
        /// The ffmpeg path.
        /// </summary>
        string ffmpegPath;

        /// <summary>
        /// To be notified when the audio is complete, register a delegate 
        /// using this signature by calling RegisterSessionCompleteDelegate.
        /// </summary>
        public delegate void AudioCaptureCompleteDelegate();

        /// <summary>
        /// The audio capturing complete delegate variable.
        /// </summary>
        AudioCaptureCompleteDelegate audioCaptureCompleteDelegate;

        /// <summary>
        /// Register a delegate to be invoked when the audio is complete.
        /// </summary>
        /// <param name='del'>
        /// The delegate to be invoked when complete.
        /// </param>
        public void RegisterCaptureCompleteDelegate(AudioCaptureCompleteDelegate del) {
            audioCaptureCompleteDelegate += del;
        }

        /// <summary>
        /// Show debug message.
        /// </summary>
        bool showDebug;

        public VRCaptureAudio SetShowDebug(bool show) {
            showDebug = show;
            return this;
        }

        public VRCaptureAudio SetFFmpegPath(string path) {
            ffmpegPath = path;
            return this;
        }

        public VRCaptureAudio SetFolderPath(string path) {
            folderPath = path;
            return this;
        }

        public bool IsProcessing() {
            return isCapturing;
        }

        public string GetFilePath() {
            return filePath;
        }

        public void Cleanup() {
            audioCaptureCompleteDelegate = null;
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        public void StartCapture() {
            if (IsProcessing()) {
                Debug.LogWarning("VRCaptureAudio: capture still processing!");
                return;
            }
            string audioPath = System.DateTime.Now.ToString("yyyy-MMM-d-HH-mm-ss") + ".wav";
            filePath = VRUtils.EscapePath(folderPath + "/" + audioPath);
            ffmpegPath = VRUtils.EscapePath(ffmpegPath);
            libAPI = LibAudioCaptureAPI_Get(
                AudioSettings.outputSampleRate,
                filePath,
                ffmpegPath);
            if (libAPI == System.IntPtr.Zero) {
                Debug.LogWarning("VRCaptureAudio: get native LibAudioCaptureAPI failed!");
                return;
            }
            InitCapture();
            isCapturing = true;
        }

        public void FinishCapture() {
            if (!isCapturing) {
                Debug.LogWarning("VRCaptureVideo: capture not start yet!");
            }
            isCapturing = false;
            LibAudioCaptureAPI_Close(libAPI);

            // Notif caller audio capture complete.
            if (audioCaptureCompleteDelegate != null) {
                audioCaptureCompleteDelegate();
            }

            if (showDebug) {
                Debug.Log("VRCaptureAudio: Encod process finish!");
            }
        }

        void InitCapture() {
            audioByteBuffer = new System.Byte[8192];
            GCHandle audioHandle = GCHandle.Alloc(audioByteBuffer, GCHandleType.Pinned);
            audioPointer = audioHandle.AddrOfPinnedObject();
        }

        void OnAudioFilterRead(float[] data, int channels) {
            if (isCapturing) {
                Marshal.Copy(data, 0, audioPointer, 2048);
                LibAudioCaptureAPI_SendFrame(libAPI, audioByteBuffer);
            }
        }
    }
}
