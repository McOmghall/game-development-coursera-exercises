using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour {
	
	public string LevelToLoad;
	
	public void loadLevel() {
        Debug.Log("Loading new level");
        //Load the level from LevelToLoad
        SceneManager.LoadScene(LevelToLoad);
	}
}
