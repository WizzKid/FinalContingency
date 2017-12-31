using System.Collections;
using UnityEngine;

public class JetScript : MonoBehaviour
{
    public float fuel = 20.0f;
    public float fuelUsing = 20.0f;
    public float maxFuel = 20.0f;
	private float ratio;
    public float JetForce = 5.0f;
	public float boost = 10.0f;
	public float boostTime = 0.2f;
	public bool jetPacking;
	public bool smallBoost;
	public float rayDist = 1f;
	public float velocityLag = 0.2f;
	public float velocityLag_fallSpeed;
	public bool landed;
    public Camera _camera;
	private Rigidbody rb;
	public Animator anim;

	private FuelBar fuelBar;
	[SerializeField]
	private FuelStat fuelStat;

	private PlayerHealth healthScript;
	private float fallSpeed;
	public float fallSpeedLimit;
	private int fallSpeedDamage;
	public AudioSource FuelGone;
	private bool reloadingFuel = true;

	//Awake over Start for reloading scene
	//Initialize variables
    private void Awake()
    {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		healthScript = GetComponentInParent<PlayerHealth> ();
        rb.freezeRotation = true;
		landed = true;
		smallBoost = true;
		jetPacking = false;
		fuelStat.Initialize ();
    }

	//Input code resides in Update because it checks all frames, whereas FixedUpdate only checks fixed frames and will miss input not on those frames
	void Update () {
		//Resets small boost and denotes the player is not jetpacking
		if (Input.GetKeyUp (KeyCode.Space) || Input.GetButtonUp("X360_A"))  {
			jetPacking = false;
		}

		//Denotes player is inputting jetpack command
		if ((fuel >= 0) && (Input.GetKey(KeyCode.Space) || Input.GetButton("X360_A") && (fuel >= 0)))
		{
			jetPacking = true;
		}
		if (fuel <= maxFuel/4 && reloadingFuel){
			FuelGone.Play();
			reloadingFuel = false;
		}

		/*if (Input.GetKeyDown (KeyCode.G)) {
			ratio = fuel / maxFuel;
			Debug.Log (ratio);
		}*/
	}

	//Handle physics in FixedUpdate()
    void FixedUpdate()
    {
		if (fuel > 0 && jetPacking)
        {
			anim.Play ("Player_Fly", -1, 0f);
			//Used to keep gravity from overpowering burst jetpack playstyle
			if (smallBoost) {
				rb.AddForce(new Vector3(0.0f, JetForce * boost, 0.0f), ForceMode.Impulse);
				smallBoost = false;
				StartCoroutine (BoostDelay());
			}
			landed = false;
			//Actual flight enacted by vector 3 add force
			rb.AddForce(new Vector3(0.0f, JetForce, 0.0f), ForceMode.Impulse);
			fuel -= fuelUsing * Time.deltaTime;
			//Update fuel bar according to fuel used over time
			fuelStat.CurrentVal = fuel/maxFuel;
        }

		RaycastHit hit;

		//Raycasts down to the floor to see if the player is landed
		if (Physics.Raycast(transform.position, new Vector3(0,-rayDist,0), out hit, rayDist)) {
			if (hit.collider.CompareTag ("landable") || hit.collider.CompareTag ("robot") || hit.collider.CompareTag ("torso") || hit.collider.CompareTag ("rightArm") || hit.collider.CompareTag ("leftArm") || hit.collider.CompareTag ("leftLeg") || hit.collider.CompareTag ("rightLeg")) {
				landed = true;
				if (hit.collider.CompareTag ("platform")) {
					fuel += fuelUsing * Time.deltaTime;
				}
			}
		}

		if (landed && fuel <= maxFuel) {
			fuel += fuelUsing * Time.deltaTime;
			fuelStat.CurrentVal = fuel/maxFuel;
			reloadingFuel = true;
		}

		//Debug ray to show in PIE
		Debug.DrawRay (transform.position, new Vector3 (0, -rayDist, 0), Color.green, rayDist);

		//Manual clamp to keep fuel in range
		if (fuel > maxFuel) {
			fuel = maxFuel;
			fuelStat.CurrentVal = fuelStat.MaxVal;
		} else if (fuel < 0) {
			fuel = 0;
			fuelStat.CurrentVal = 0;
		}

		fallSpeed = rb.velocity.y;

		if (fallSpeed >= fallSpeedLimit) {
			velocityLag_fallSpeed = fallSpeed;
			StartCoroutine (FallSpeed (velocityLag_fallSpeed));
		}
    }

	void OnCollisionEnter (Collision other) {
		if (velocityLag_fallSpeed <= fallSpeedLimit) {
			velocityLag_fallSpeed -= fallSpeedLimit;
			fallSpeedDamage = Mathf.RoundToInt (velocityLag_fallSpeed);
			if (velocityLag_fallSpeed >= fallSpeedLimit * 2.5f) {
				healthScript.doDamage (-fallSpeedDamage*5);
			} else if (velocityLag_fallSpeed >= fallSpeedLimit * 2.0f) {
				healthScript.doDamage (-fallSpeedDamage*2);
			} else {
				healthScript.doDamage (-fallSpeedDamage);
			}
		}
	}

	IEnumerator BoostDelay() {
		yield return new WaitForSeconds (boostTime);
		smallBoost = true;
	}

	IEnumerator FallSpeed(float s) {
		yield return new WaitForSeconds (velocityLag);
		velocityLag_fallSpeed = fallSpeed;
	}
}