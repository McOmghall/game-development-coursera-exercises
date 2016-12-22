using UnityEngine;
namespace VRCapture {
    public class VRUtils : MonoBehaviour {
        public static string EscapePath(string path) {
            return '"' + path + '"';
        }
    }
}
