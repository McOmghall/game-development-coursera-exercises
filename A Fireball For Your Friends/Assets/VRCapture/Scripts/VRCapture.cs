using UnityEngine;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System;

namespace VRCapture {

    /// <summary>
    /// VRCapture is a plugin helping VR player to record and share
    /// their gameplay easily and nicely.
    /// </summary>
    public class VRCapture : MonoBehaviour {
        public static VRCapture instance;

        public enum VideoQuality {
            /// <summary>
            /// Lower quality will decrease filesize on disk.
            /// Low = 1000 bitrate.
            /// </summary>
            Low,

            /// <summary>
            /// Medium = 2500 bitrate.
            /// </summary>
            Medium,

            /// <summary>
            /// High = 5000 bitrate.
            /// </summary>
            High
        }

        /// <summary>
        /// Indicates the current status of the capturing session.
        /// </summary>
        public enum SessionStatusCode {

            /// <summary>
            /// The capturing session has encountered no errors.
            /// </summary>
            Success = 1,

            /// <summary>
            /// No camera was found to perform video recording and no custom 
            /// rendertexture was specified.
            /// You must specify one or the other before calling BeginCaptureSession().
            /// One or more cameras may be specified by setting the VideoCameras property.
            /// </summary>
            CameraNotFound = -1,

            /// <summary>
            /// The ffmpeg executable file not found, this plugin current is depend
            /// on this to generate the videos.
            /// </summary>
            FFmpegNotFound = -2,

            /// <summary>
            /// The capture process is interrupted by user or unexcept quit.
            /// </summary>
            Interrupted = -3,
        }

        /// <summary>
        /// To be notified when the capture is complete, register a delegate 
        /// using this signature by calling RegisterSessionCompleteDelegate.
        /// </summary>
        public delegate void SessionCompleteDelegate();

        /// <summary>
        /// Register a delegate to be invoked when the capture is complete.
        /// </summary>
        /// <param name='del'>
        /// The delegate to be invoked when capture complete.
        /// </param>
        public void RegisterSessionCompleteDelegate(SessionCompleteDelegate del) {
            sessionCompleteDelegate += del;
        }

        // The video recording complete delegate variable.
        SessionCompleteDelegate sessionCompleteDelegate;

        /// <summary>
        /// Reference to the VRCaptureVideo capture objects (i.e. cameras) from
        /// which video will be recorded.
        /// Generally you will want to specify at least one.
        /// </summary>

        [Tooltip("Video camera with video recording")]
        public VRCaptureVideo[] vrCaptureVideos;

        /// <summary>
        /// Reference to the VRCaptureAudio object for writing audio files.
        /// This needs to be set when you are recording a video with audio.
        /// </summary>
        [Tooltip("Object with recording audio script")]
        public VRCaptureAudio vrCaptureAudio;

        /// <summary>
        /// Capturing session status.
        /// </summary>
        SessionStatusCode sessionStatus;

        /// <summary>
        /// Capture configurations.
        /// TODO, check configuration correct.
        /// </summary>

        [Tooltip("Pixel width for video recording")]
        public int frameWidth = 1280;
        [Tooltip("Pixel Height for video recording")]
        public int frameHeight = 720;
        [Tooltip("Anti aliasing parameter of video")]
        public int antiAliasing = 2;
        [Tooltip("The target frame rate recording")]
        public int targetFrameRate = 30;
        [Tooltip("Quality of recorded video")]
        public VideoQuality videoQuality = VideoQuality.High;

        /// <summary>
        /// The ffmpeg executable file.
        /// </summary>
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string ffmpagFile = "/VRCapture/FFmpeg/Win/ffmpeg.exe";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        string ffmpagFile = "/VRCapture/FFmpeg/Mac/ffmpeg";
#endif
        string ffmpegPath;
        [NonSerialized]
        public string folderPath =
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/VRCapture";

        /// <summary>
        /// Check how many video capture is complete currently.
        /// </summary>
        int captureFinishCount;

        /// <summary>
        /// Check wether the video and audio is still in merging process.
        /// </summary>
        bool isMerging;
        /// <summary>
        /// Check wether the capture process is interrupted.
        /// </summary>
        bool isInterrupted;

        /// <summary>
        /// Show debug message.
        /// </summary>
        bool showDebug = false;

        void Awake() {
            instance = this;
        }

        void OnApplicationQuit() {
            if (IsProcessing()) {
                InterruptCaptureSession();
            }
        }

        public bool IsProcessing() {
            bool isPorcessing = false;
            foreach (VRCaptureVideo vrCaptureVideo in vrCaptureVideos) {
                if (vrCaptureVideo.IsProcessing()) {
                    isPorcessing = true;
                    break;
                }
            }
            return isPorcessing || isMerging;
        }

        /// <summary>
        /// Initialize the attributes of the capture session and and start capturing. 
        /// </summary>
        /// <returns>
        /// The status of the capturing session. This may be Success or a failure code.
        /// See SessionStatusCode for more information.
        /// </returns>
        public SessionStatusCode BeginCaptureSession() {
            if (vrCaptureVideos.Length < 1) {
                Debug.LogError("VRCapture: BeginCaptureSession called but no attached " +
                                 "camera was found!");
                sessionStatus = SessionStatusCode.CameraNotFound;
                return sessionStatus;
            }

            if (IsProcessing()) {
                Debug.LogWarning("VRCapture: BeginCaptureSession called before, and " +
                                 "capture still processing!");
                return sessionStatus;
            }
            ffmpegPath = Application.dataPath + ffmpagFile;
            if (!File.Exists(ffmpegPath)) {
                Debug.LogError("VRCapture: FFmpeg not found, please fix this " +
                              "before capture!");
                sessionStatus = SessionStatusCode.FFmpegNotFound;
                return sessionStatus;
            }
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            // Loop through each of the video capture objects, initialize them 
            // and start them recording.
            foreach (VRCaptureVideo vrCaptureVideo in vrCaptureVideos) {
                vrCaptureVideo
                    .SetFFmpegPath(ffmpegPath)
                    .SetFolderPath(folderPath)
                    .SetVideoResolution(frameWidth, frameHeight)
                    .SetAntiAliasing(antiAliasing)
                    .SetTargetFrameRate(targetFrameRate)
                    .SetShowDebug(showDebug);
                vrCaptureVideo.StartCapture();
                vrCaptureVideo.RegisterCaptureCompleteDelegate(
                    HandleVideoCaptureComplete);
            }

            // Check if we capture audio.
            if (vrCaptureAudio != null) {
                vrCaptureAudio
                    .SetFFmpegPath(ffmpegPath)
                    .SetFolderPath(folderPath)
                    .SetShowDebug(showDebug);
                vrCaptureAudio.StartCapture();
                vrCaptureAudio.RegisterCaptureCompleteDelegate(
                    HandleAudioCaptureComplete);
            }
            captureFinishCount = 0;

            sessionStatus = SessionStatusCode.Success;
            return sessionStatus;
        }

        /// <summary>
        /// Stop capturing and produce the finalized video. Note that the video file
        /// may not be completely written when this method returns. In order to know
        /// when the video file is complete, register a SessionCompleteDelegate.
        /// </summary>
        /// <returns>The capture session.</returns>
        public SessionStatusCode EndCaptureSession() {
            // If the client calls EndRecordingSession for a failed session, do nothing.
            if (sessionStatus != SessionStatusCode.Success)
                return sessionStatus;

            foreach (VRCaptureVideo vrCaptureVideo in vrCaptureVideos) {
                vrCaptureVideo.FinishCapture();
            }

            if (vrCaptureAudio != null) {
                vrCaptureAudio.FinishCapture();
            }

            sessionStatus = SessionStatusCode.Success;
            return sessionStatus;
        }

        public SessionStatusCode InterruptCaptureSession() {
            isInterrupted = true;
            EndCaptureSession();
            sessionStatus = SessionStatusCode.Interrupted;
            return sessionStatus;
        }

        void HandleVideoCaptureComplete() {
            captureFinishCount++;
            if (captureFinishCount == vrCaptureVideos.Length &&
                vrCaptureAudio == null) {
            }
        }

        void HandleAudioCaptureComplete() {
            if (isInterrupted) {
                isInterrupted = false;
                Cleanup();
                return;
            }
            // Start merging thread.
            Thread mergingThread = new Thread(MergingThreadFunction);
            mergingThread.Priority = System.Threading.ThreadPriority.Lowest;
            mergingThread.IsBackground = true;
            mergingThread.Start();
            isMerging = true;
        }

        void MergingThreadFunction() {

            while (captureFinishCount < vrCaptureVideos.Length) {
                Thread.Sleep(1000);
                if (isInterrupted) {
                    return;
                }
            }

            int bitrate = 5000;
            if (videoQuality == VideoQuality.Medium) {
                bitrate = 2500;
            }
            else if (videoQuality == VideoQuality.Low) {
                bitrate = 1000;
            }
            VRCaptureMerger merger = new VRCaptureMerger();
            string audioPath = (vrCaptureAudio != null) ? vrCaptureAudio.GetFilePath() : null;
            merger
                .SetBitrate(bitrate)
                .SetFolderPath(folderPath)
                .SetFFmpegPath(ffmpegPath)
                .SetAudioPath(audioPath);
            foreach (VRCaptureVideo vrCaptureVideo in vrCaptureVideos) {
                string videoPath = vrCaptureVideo.GetFilePath();
                merger.SetVideoPath(videoPath);
                merger.Merge();
            }
            isMerging = false;
            Cleanup();
            if (sessionCompleteDelegate != null) {
                sessionCompleteDelegate();
            }
        }

        void Cleanup() {
            captureFinishCount = 0;
            foreach (VRCaptureVideo vrCaptureVideo in vrCaptureVideos) {
                vrCaptureVideo.Cleanup();
            }
            if (vrCaptureAudio != null) {
                vrCaptureAudio.Cleanup();
            }
        }
    }

    /// <summary>
    /// VRCaptureMerger is processed after temp video captured, with or without
    /// temp audio captured. If audio captured, it will merge the video and audio
    /// within same file.
    /// </summary>
    class VRCaptureMerger {

        [DllImport("vrcapture-lib")]
        static extern System.IntPtr LibVideoMergeAPI_Get(int rate, string path, string vpath, string apath, string ffpath);

        [DllImport("vrcapture-lib")]
        static extern void LibVideoMergeAPI_Merge(System.IntPtr api);

        int bitrate;
        string ffmpegPath;
        string folderPath;
        string tempVideoPath;
        string tempAudioPath;

        public VRCaptureMerger SetFFmpegPath(string path) {
            ffmpegPath = path;
            return this;
        }

        public VRCaptureMerger SetFolderPath(string path) {
            folderPath = path;
            return this;
        }

        public VRCaptureMerger SetBitrate(int rate) {
            bitrate = rate;
            return this;
        }

        public VRCaptureMerger SetVideoPath(string path) {
            tempVideoPath = path;
            return this;
        }

        public VRCaptureMerger SetAudioPath(string path) {
            tempAudioPath = path;
            return this;
        }

        public void Merge() {
            string videoPath = System.DateTime.Now.ToString("yyyy-MMM-d-HH-mm-ss") + ".mp4";
            string filePath = VRUtils.EscapePath(folderPath + "/" + videoPath);
            tempVideoPath = VRUtils.EscapePath(tempVideoPath);
            tempAudioPath = VRUtils.EscapePath(tempAudioPath);
            ffmpegPath = VRUtils.EscapePath(ffmpegPath);
            System.IntPtr libAPI =
                LibVideoMergeAPI_Get(bitrate, filePath, tempVideoPath, tempAudioPath, ffmpegPath);
            if (libAPI == System.IntPtr.Zero) {
                Debug.LogWarning("VRCapture: get native LibVideoMergeAPI failed!");
                return;
            }
            LibVideoMergeAPI_Merge(libAPI);
            // Make sure generated the merge file.
            while (!File.Exists(filePath)) {
                Thread.Sleep(500);
            }
        }
    }
}