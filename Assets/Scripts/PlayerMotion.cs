using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

    private PlayerCharacter m_playerCharacter;

    [SerializeField]
    private Rigidbody2D m_RigidBody;
    [SerializeField]
    private float m_MaxSpeed = 10f;

    // Use this for initialization
    void Start () {
        m_playerCharacter = GetComponent<PlayerCharacter>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Move(horizontalMovement, verticalMovement);
    }

    public void Move(float horizontal, float vertical)
    {
        // Move the character
        m_RigidBody.velocity = new Vector2(horizontal * m_MaxSpeed, vertical * m_MaxSpeed);
    }
}
