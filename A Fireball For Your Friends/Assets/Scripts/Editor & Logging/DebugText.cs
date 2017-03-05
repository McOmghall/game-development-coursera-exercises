using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Text))]
public class DebugText : MonoBehaviour {
  private Dictionary<string, string> infos = new Dictionary<string, string>();
  private Text localText;

  public void setInfo(string topic, string info) {
    if (!this.isActiveAndEnabled) {
      return;
    }
    //Debug.Log("SET INFO " + topic + " " + info);
    if (infos.ContainsKey(topic)) {
      infos.Remove(topic);
    }
    infos.Add(topic, info);

    string text = "";
    foreach (KeyValuePair<string, string> i in infos) {
      text += i.Key + ": " + i.Value + "\n";
    }

    localText.text = text;
    //Debug.Log("Infos: " + infos.Count);
  }

  void Start() {
    localText = GetComponent<Text>();
  }
}
