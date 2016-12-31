using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

    private PlayerCharacter m_playerCharacter;

    [SerializeField]
    private Rigidbody2D m_RigidBody;
    [SerializeField]
    private float m_MaxSpeed = 10f;
    public float m_TurnSpeed = 180f;

    private float m_TurnInputValue;

    // Use this for initialization
    void Start () {
        m_playerCharacter = GetComponent<PlayerCharacter>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        m_RigidBody.transform.position = m_RigidBody.position + (new Vector2(0.0f, 0.0f)) * (Time.time - Time.deltaTime);
	}

    void FixedUpdate()
    {
        if (!m_playerCharacter.isFrozen)
        {
            Move();
        }
        Look();
    }

    public void stopMovement()
    {
        m_RigidBody.velocity = Vector2.zero;
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Move the character
        m_RigidBody.velocity = new Vector2(horizontalMovement * m_MaxSpeed, verticalMovement * m_MaxSpeed);
    }

    private void Look()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePoint - transform.position);
    }
}
