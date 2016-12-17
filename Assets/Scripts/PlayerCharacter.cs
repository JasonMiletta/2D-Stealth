using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

    public GameObject pauseMenu;
    public PlayerMotion motion;
    public int health = 1;

    private bool isPaused = false;
    public bool isFrozen{ get; protected set; }

    private void Awake()
    {
    }

    // Use this for initialization
    void Start() {
        motion = GetComponent<PlayerMotion>();
        isFrozen = false;
    }

    void Update()
    {
        // Enable pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            isPaused = true;
        }

        // disable pause menu
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            isPaused = false;
        }
    }

    public void freezePlayer()
    {
        isFrozen = true;
        motion.stopMovement();
    }

    private void ApplyDamage(int damageValue)
    {
        health -= damageValue;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

