using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Text))]
public class DebugText : MonoBehaviour {
  private Dictionary<string, string> infos = new Dictionary<string, string>();
  private Text localText;
  private float nextUpdate = float.MinValue;

  public void setInfo(string topic, string info) {
    if (infos.ContainsKey(topic)) {
      infos.Remove(topic);
    }
    infos.Add(topic, info);
  }

  void Start() {
    localText = GetComponent<Text>();
  }

  void Update() {
    if (Time.time > nextUpdate) {
      string text = "";
      foreach (KeyValuePair<string, string> i in infos) {
        text += i.Key + ": " + i.Value + "\n";
      }

      localText.text = text;

      nextUpdate = Time.time + 2;
    }
  }
}
