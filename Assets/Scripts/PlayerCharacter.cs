using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = 10f;
    private Rigidbody2D m_RigidBody;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(float horizontal, float vertical)
    {
        // Move the character
        m_RigidBody.velocity = new Vector2(horizontal*m_MaxSpeed, vertical*m_MaxSpeed);
    }
}

