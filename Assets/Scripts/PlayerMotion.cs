using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

    private PlayerCharacter m_playerCharacter;

	// Use this for initialization
	void Start () {
        m_playerCharacter = GetComponent<PlayerCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        m_playerCharacter.Move(horizontalMovement, verticalMovement);
    }
}
