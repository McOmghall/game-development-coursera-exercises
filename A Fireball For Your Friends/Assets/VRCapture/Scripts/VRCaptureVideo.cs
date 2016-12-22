using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;

namespace VRCapture {

    [RequireComponent(typeof(Camera))]
    public class VRCaptureVideo : MonoBehaviour {
        [DllImport("vrcapture-lib")]
        static extern System.IntPtr LibVideoCaptureAPI_Get(int width, int height, int rate, string path, string ffpath);

        [DllImport("vrcapture-lib")]
        static extern void LibVideoCaptureAPI_SendFrame(System.IntPtr api, byte[] data);

        [DllImport("vrcapture-lib")]
        static extern void LibVideoCaptureAPI_Close(System.IntPtr api);

        /// <summary>
        /// To be notified when the video is complete, register a delegate 
        /// using this signature by calling VideoCaptureCompleteDelegate.
        /// </summary>
        public delegate void VideoCaptureCompleteDelegate();

        /// <summary>
        /// The video capturing complete delegate variable.
        /// </summary>
        VideoCaptureCompleteDelegate videoCaptureCompleteDelegate;

        /// <summary>
        /// Register a delegate to be invoked when the video is complete.
        /// </summary>
        /// <param name='del'>
        /// The delegate to be invoked when complete.
        /// </param>
        public void RegisterCaptureCompleteDelegate(VideoCaptureCompleteDelegate del) {
            videoCaptureCompleteDelegate += del;
        }

        /// <summary>
        /// Video capture configurations.
        /// TODO, check configuration correct.
        /// </summary>
        string ffmpegPath;
        string folderPath;
        string filePath;
        int frameWidth;
        int frameHeight;
        int antiAliasing;
        int targetFrameRate;

        /// <summary>
        /// Specifies whether or not the camera being used to capture video is 
        /// dedicated solely to video capture. When a dedicated camera is used,
        /// the camera's aspect ratio will automatically be set to the specified
        /// frame size.
        /// If a non-dedicated camera is specified it is assumed the camera will 
        /// also be used to render to the screen, and so the camera's aspect 
        /// ratio will not be adjusted.
        /// Use a dedicated camera to capture video at resolutions that have a 
        /// different aspect ratio than the device screen.
        /// </summary>
        public bool isDedicated = true;

        /// <summary>
        /// The camera that resides on the same game object as this script.
        /// It will be used for capturing video.
        /// </summary>
        Camera videoCamera;
        /// <summary>
        /// The texture holding the video frame data.
        /// </summary>
        Texture2D texture2d;
        RenderTexture renderTexture;
        /// <summary>
        /// Whether or not capturing from this camera is currently in progress.
        /// </summary>
        bool isCapturing;
        /// <summary>
        /// Whether or not there is a frame capturing now.
        /// </summary>
        bool isCapturingFrame;
        /// <summary>
        /// The time spent during capturing.
        /// </summary>
        float capturingTime;
        /// <summary>
        /// The delta time of each frame.
        /// </summary>
        float deltaFrameTime;
        /// <summary>
        /// Frame statistics.
        /// </summary>
        int capturedFrameCount;
        int encodedFrameCount;
        /// <summary>
        /// Reference to native lib API.
        /// </summary>
        System.IntPtr libAPI;

        /// <summary>
        /// Thread shared resources.
        /// </summary>
        Queue<byte[]> frameQueue;
        Object threadLock;

        /// <summary>
        /// Show debug message.
        /// </summary>
        bool showDebug;

        public VRCaptureVideo SetShowDebug(bool show) {
            showDebug = show;
            return this;
        }

        public VRCaptureVideo SetFFmpegPath(string path) {
            ffmpegPath = path;
            return this;
        }

        public VRCaptureVideo SetFolderPath(string path) {
            folderPath = path;
            return this;
        }

        public VRCaptureVideo SetVideoResolution(int width, int height) {
            frameWidth = width;
            frameHeight = height;
            return this;
        }

        public VRCaptureVideo SetAntiAliasing(int antiAliasing) {
            this.antiAliasing = antiAliasing;
            return this;
        }

        public VRCaptureVideo SetTargetFrameRate(int frameRate) {
            targetFrameRate = frameRate;
            return this;
        }

        public bool IsProcessing() {
            return isCapturing || (frameQueue != null && frameQueue.Count > 0);
        }

        public string GetFilePath() {
            return filePath;
        }

        public void Cleanup() {
            texture2d = null;
            renderTexture = null;
            frameQueue = null;
            threadLock = null;
            videoCaptureCompleteDelegate = null;
            capturedFrameCount = 0;
            encodedFrameCount = 0;
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        public void StartCapture() {
            if (IsProcessing()) {
                Debug.LogWarning("VRCaptureVideo: capture still processing!");
                return;
            }
            string videoPath = System.DateTime.Now.ToString("yyyy-MMM-d-HH-mm-ss") + ".mp4";
            filePath = VRUtils.EscapePath(folderPath + "/" + videoPath);
            ffmpegPath = VRUtils.EscapePath(ffmpegPath);
            libAPI = LibVideoCaptureAPI_Get(
                frameWidth,
                frameHeight,
                targetFrameRate,
                filePath,
                ffmpegPath);
            if (libAPI == System.IntPtr.Zero) {
                Debug.LogWarning("VRCaptureVideo: get native LibVideoCaptureAPI failed!");
                return;
            }
            InitCapture();
            isCapturing = true;
            deltaFrameTime = 1f / targetFrameRate;
            capturingTime = 0f;
            texture2d = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);
            frameQueue = new Queue<byte[]>();
            threadLock = new Object();
            // Start encoding thread.
            Thread encodingThread = new Thread(EncodingThreadFunction);
            encodingThread.Priority = System.Threading.ThreadPriority.Lowest;
            encodingThread.IsBackground = true;
            encodingThread.Start();
        }

        public void FinishCapture() {
            if (!isCapturing) {
                Debug.LogWarning("VRCaptureVideo: capture not start yet!");
            }
            isCapturing = false;
        }

        void InitCapture() {
            if (videoCamera.targetTexture != null) {
                // Use binded rendertexture will ignore antiAliasing config.
                renderTexture = videoCamera.targetTexture;
            }
            else {
                // Create a rendertexture for video capture.    
                // Size it according to the desired video frame size.
                renderTexture = new RenderTexture(frameWidth, frameHeight, 24);
                renderTexture.antiAliasing = antiAliasing;
                // Make sure the rendertexture is created.
                renderTexture.Create();
            }

            if (isDedicated) {
                // Set the aspect ratio of the camera to match the rendertexture.
                videoCamera.aspect = frameWidth / ((float)frameHeight);
                videoCamera.targetTexture = renderTexture;
            }
        }

        void Awake() {
            videoCamera = GetComponent<Camera>();
        }

        void LateUpdate() {
            if (isCapturing) {
                capturingTime += Time.deltaTime;
            }
            if (!isCapturingFrame && isCapturing) {
                int totalRequiredFrameCount =
                    (int)(capturingTime / deltaFrameTime);
                // Skip frames if we already got enough.
                if (totalRequiredFrameCount > capturedFrameCount) {
                    videoCamera.Render();
                    StartCoroutine(CaptureFrame());
                }
            }
        }

        IEnumerator CaptureFrame() {
            // Wait few frames for rendering finish.
            yield return null;
            isCapturingFrame = true;
            if (isCapturing) {
                RenderTexture.active = renderTexture;
                // TODO, remove the step of copying pixel data from GPU to CPU.
                texture2d.ReadPixels(new Rect(0, 0, frameWidth, frameHeight), 0, 0, false);
                RenderTexture.active = null;
                texture2d.Apply();
            }
            yield return null;
            // User may terminate the capture process during caoturing frame.
            if (isCapturing) {
                byte[] pixels = texture2d.GetRawTextureData();
                int totalRequiredFrameCount = (int)(capturingTime / deltaFrameTime);
                int requiredFrameCount = totalRequiredFrameCount - capturedFrameCount;
                lock (threadLock) {
                    while (requiredFrameCount-- > 0) {
                        frameQueue.Enqueue(pixels);
                    }
                }
                capturedFrameCount = totalRequiredFrameCount;
            }
            isCapturingFrame = false;
        }

        void EncodingThreadFunction() {
            while (isCapturing || frameQueue.Count > 0) {
                if (frameQueue.Count > 0) {
                    lock (threadLock) {
                        byte[] frame = frameQueue.Dequeue();
                        LibVideoCaptureAPI_SendFrame(libAPI, frame);
                    }
                    encodedFrameCount++;
                    if (showDebug) {
                        Debug.Log("VRCaptureVideo: Encoded " +
                                  encodedFrameCount + " frames. " +
                                  frameQueue.Count + " frames remaining.");
                    }
                }
                else {
                    Thread.Sleep(1);
                }
            }
            if (showDebug) {
                Debug.Log("VRCaptureVideo: Encod process finish!");
            }
            // Noify native encoding process finish.
            LibVideoCaptureAPI_Close(libAPI);
            // Notif caller video capture complete.
            if (videoCaptureCompleteDelegate != null) {
                videoCaptureCompleteDelegate();
            }
        }
    }
}
