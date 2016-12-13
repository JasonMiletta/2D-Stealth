using UnityEngine;
using System.Collections;

public class ExitGoal : MonoBehaviour {

    public GameManager gameManager;

    bool goalReached = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("trigger enter");
        gameManager.setExitGoalReached();
    }
}
