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
	
	}

    void FixedUpdate()
    {
        Move();
        Look();
    }

    public void Move()
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
