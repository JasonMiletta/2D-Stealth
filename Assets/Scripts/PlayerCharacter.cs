using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

    public GameObject pauseMenu;
    private bool isEnabled = false;

    private void Awake()
    {
    }

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        // Enable pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !isEnabled)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            isEnabled = true;
        }

        // disable pause menu
        else if (Input.GetKeyDown(KeyCode.Escape) && isEnabled)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            isEnabled = false;
        }
    }
}

