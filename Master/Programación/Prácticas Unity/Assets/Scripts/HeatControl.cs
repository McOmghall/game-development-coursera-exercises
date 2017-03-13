using Editor;
using UnityEngine;

public class HeatControl : MonoBehaviour {
    public AnimationCurve temperatureValues = AnimationCurve.Linear (0, 100, 5, 0);
    public WrapMode postWrapMode = WrapMode.PingPong;
    public WrapMode preWrapMode = WrapMode.PingPong;
    public Color colorHeat = Color.red;
    public Color colorCold = Color.blue;

    [ReadOnly] public float temperature;
    [ReadOnly] public float minTemp = float.MaxValue;
    [ReadOnly] public float maxTemp = float.MinValue;

    void Start () {
        temperatureValues.postWrapMode = postWrapMode;
        temperatureValues.preWrapMode = preWrapMode;

        foreach (Keyframe k in temperatureValues.keys) {
            minTemp = (k.value < minTemp ? k.value : minTemp);
            maxTemp = (k.value > maxTemp ? k.value : maxTemp);
        }
    }

	void Update () {
        temperature = temperatureValues.Evaluate(Time.time);
        GetComponent<MeshRenderer>().material.color = Color.Lerp(colorCold, colorHeat, (temperature - minTemp) / (maxTemp - minTemp));
	}
}
