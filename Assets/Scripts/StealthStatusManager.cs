using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StealthStatusManager : MonoBehaviour
{
    public Text m_StealthStatusText;            // Reference to the Status text on the HUD

    private void OnEnable()
    {
        EventManager.StartListening("playerSpotted", playerSpotted);
        EventManager.StartListening("playerLost", playerLost);
    }

    private void OnDisable()
    {
        EventManager.StopListening("playerSpotted", playerSpotted);
        EventManager.StopListening("playerLost", playerLost);
    }
    // Use this for initialization
    void Start () {
        playerLost();
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void playerSpotted()
    {
        Debug.Log("player spotted");
        m_StealthStatusText.text = "Spotted!";
        m_StealthStatusText.color = Color.red;
    }

    private void playerLost()
    {
        Debug.Log("player lost");
        m_StealthStatusText.text = "Hidden...";
        m_StealthStatusText.color = Color.green;
    }
}
