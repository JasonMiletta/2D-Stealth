using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public string startSceneName;

	// Use this for initialization
	void Start () {
	    if(startSceneName == null)
        {
            startSceneName = "Level 1";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startGame()
    {
        SceneManager.LoadScene(startSceneName, LoadSceneMode.Single);
    }

    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
