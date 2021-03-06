using UnityEngine;
using System.Collections;

public class PlayerMovementCrashes : MonoBehaviour 
{
	public float speed = 2f;
	public float runSpeed = 5f;
	public float turnSmoothing = 15f;

	private Vector3 movement;
	private Rigidbody playerRigidBody;
	public Vector3 jump;
	public float jumpForce = 2.0f;
	public bool isGrounded;

	void Awake()
	{
		jump = new Vector3(0.0f, 2.0f, 0.0f);
		playerRigidBody = GetComponent<Rigidbody> ();
	}

	void OnCollisionStay()
	{
		isGrounded = true;
	}

	void FixedUpdate()
	{
		float lh = Input.GetAxisRaw ("Horizontal");
		float lv = Input.GetAxisRaw ("Vertical");

		Move (lh, lv);

		while(Input.GetKeyDown(KeyCode.Space)){

			playerRigidBody.AddForce(jump * jumpForce);
		}
	}


	void Move (float lh, float lv)
	{
		movement.Set (lh, 0f, lv);
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0f;

		if (Input.GetKey (KeyCode.LeftShift))
		{
			movement = movement.normalized * runSpeed * Time.deltaTime;
		} 
		else 
		{
			movement = movement.normalized * speed * Time.deltaTime;
		}

		playerRigidBody.MovePosition (transform.position + movement);


		if (lh != 0f || lv != 0f) 
		{
			Rotating(lh, lv);
		}
	}


	void Rotating (float lh, float lv)
	{

		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody> ().rotation, targetRotation, turnSmoothing * Time.deltaTime);

		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}

}