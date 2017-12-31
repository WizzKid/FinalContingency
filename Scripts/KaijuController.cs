using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KaijuController : MonoBehaviour 
{

	private Animator colAnim;
	private Animator guardAnim;
	//access to player health
	//private PlayerHealth p_health;

	private bool attack;
	//Turn attacking:
	//true = guardian, false = colossus
	private bool turn;

	public float turnDelayTime = 10.0f;
	private int attackAnim;
	private int damagedPartAnim;
	private float temp;
	public float hitDelayTime = 5.0f;
	public float guardianDeathDelay = 4.0f;
	public float guardianDeathCheckDelay = 26.0f;
	public float winDelay = 5.0f;
	public float colossusHealth = 100.0f;
	public float colossusMaxHealth = 100.0f;
	public float guardDamage = 10.0f;
	private int intGrd_Dmg;
	public AudioSource colossusAttackSound;
	public AudioSource guardianAttackSound;
	public AudioSource hitSound;
	public AudioSource colYell;
	public AudioSource colHit;
	public AudioSource colDying;
	public ParticleSystem explosion;
	public GameObject colossusPos;

	//---------------UPDATED BY BRANDON------------------
	//for each body part I added a float value for health and a public object for the HUD piece
	//separated part sections for clarity, other updated sections are "void Update" and "IEnumerator ColosHitDelay"

	public bool damagedRightArm;
	public float rightArmHealth = 5;
	public GameObject RightArmHUD;

	public bool damagedRightLeg;
	public float rightLegHealth = 5;
	public GameObject RightLegHUD;

	public bool damagedLeftArm;
	public float leftArmHealth = 5;
	public GameObject LeftArmHUD;

	public bool damagedLeftLeg;
	public float leftLegHealth = 5;
	public GameObject LeftLegHUD;

	public bool damagedTorso;
	public float torsoHealth = 5;
	public GameObject TorsoHUD;
	public GameObject TorsoText;

	private GameObject RightArm;
	private GameObject RightLeg;
	private GameObject LeftArm;
	private GameObject LeftLeg;
	private GameObject Torso;

	public bool guardianDying;
	private ColossusHealth colosHealth;

	private bool healing;
	public float healDelay = 1.0f;

	public AudioSource Warning;

	public float transitionDelay = 8.0f;

	// Use this for initialization
	void Awake () {
		colAnim = GameObject.FindGameObjectWithTag ("colossus").GetComponent<Animator> ();
		guardAnim = GameObject.FindGameObjectWithTag ("guardian").GetComponent<Animator> ();
		//p_health = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		colosHealth = GetComponent<ColossusHealth>();

		RightArm = GameObject.FindGameObjectWithTag ("rightArmBase");
		RightLeg = GameObject.FindGameObjectWithTag ("rightLegBase");
		LeftArm = GameObject.FindGameObjectWithTag ("leftArmBase");
		LeftLeg = GameObject.FindGameObjectWithTag ("leftLegBase");
		Torso = GameObject.FindGameObjectWithTag ("torsoBase");

		attack = true;
		turn = true;
		damagedRightArm = false;
		RightArmHUD.SetActive(false);
		damagedRightLeg = false;
		RightLegHUD.SetActive(false);
		damagedLeftArm = false;
		LeftArmHUD.SetActive(false);
		damagedLeftLeg = false;
		LeftLegHUD.SetActive(false);
		damagedTorso = false;
		TorsoHUD.SetActive(false);
		guardianDying = false;
		guardAnim.SetInteger ("Guard_AnimState", 0);
		colAnim.SetInteger ("Colos_AnimState", 0);

		healing = false;

		if (SceneManager.GetActiveScene().name == "Tutorial_level") {
			foreach (Renderer r in Torso.GetComponentsInChildren<Renderer>()) {
				r.material.color = Color.red;
			}
			foreach (Renderer r in LeftArm.GetComponentsInChildren<Renderer>()) {
				r.material.color = Color.red;
			}
			foreach (Renderer r in RightArm.GetComponentsInChildren<Renderer>()) {
				r.material.color = Color.red;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if TurnDelay is over, alternate between attacks, each mech has a random of 4 attacks
		if (attack) {
			Debug.Log ("attack");
			//random attack chosen out of 4 attacks
			temp = Random.Range (1, 4);
			attackAnim = Mathf.RoundToInt (temp);
				//Guardian turn
				if (turn) {
					Debug.Log ("Guardian Turn");
					//Checking guardian damaged parts before starting anim
					if (damagedLeftArm && damagedLeftLeg || damagedRightArm && damagedRightLeg || damagedTorso) {
						turn = !turn;
						return;
					} else {
						guardAnim.SetInteger("Guard_AnimState", attackAnim);
						guardianAttackSound.Play ();
					}
					StartCoroutine (GuardHitDelay());
				}

				//Colossus turn
				else if (!turn) {
					Debug.Log ("Colossus Turn");
					colAnim.SetInteger ("Colos_AnimState", attackAnim);
					colossusAttackSound.Play ();
					//If torso is damaged, skip to death and return out of update.
					if (damagedTorso && !guardianDying) {
						guardianDying = true;
						StartCoroutine (GuardianDeathCheck ());
						return;
					}
					//Targeting Right
					if (attackAnim >= 1 && attackAnim <= 2) {
						if (damagedLeftLeg) {
							StartCoroutine (ColosHitDelay (3));
							//damagedTorso = true;
						}	
						else if (damagedRightArm) {
							StartCoroutine (ColosHitDelay (2));
							//damagedRightLeg = true;
						} else {
							StartCoroutine (ColosHitDelay (1));
							//damagedRightArm = true;
						}
					}
					//Targeting Left
					else if (attackAnim >= 3 && attackAnim <= 4) {
						if (damagedLeftLeg) {
							StartCoroutine (ColosHitDelay (6));
							//damagedTorso = true;
						}
						else if (damagedLeftArm) {
							StartCoroutine (ColosHitDelay (5));
							//damagedLeftLeg = true;
						} else {
							StartCoroutine (ColosHitDelay (4));
							//damagedLeftArm = true;
						}
					} 
				//end colossus turn
				}
			attack = false;
			StartCoroutine (TurnDelay ());
		}
		//end attack if statement----------------------------

		//if-then statements to check health value and update HUD
		if (rightArmHealth < 5) {
			RightArmHUD.SetActive (true);
		} else if (rightArmHealth >= 5) {
			RightArmHUD.SetActive (false);
			damagedRightArm = false;
			foreach(Renderer r in RightArm.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.white;
			}
		}

		if (rightLegHealth < 5) {
			RightLegHUD.SetActive (true);
		} else if (rightLegHealth >= 5) {
			RightLegHUD.SetActive (false);
			damagedRightLeg = false;
			foreach(Renderer r in RightLeg.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.white;
			}
		}

		if (leftArmHealth < 5) {
			LeftArmHUD.SetActive (true);
		} else if (leftArmHealth >= 5) {
			LeftArmHUD.SetActive (false);
			damagedLeftArm = false;
			foreach(Renderer r in LeftArm.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.white;
			}
		}

		if (leftLegHealth < 5) {
			LeftLegHUD.SetActive (true);
		} else if (leftLegHealth >= 5) {
			LeftLegHUD.SetActive (false);
			damagedLeftLeg = false;
			foreach(Renderer r in LeftLeg.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.white;
			}
		}

		if (torsoHealth < 5) {
			TorsoHUD.SetActive (true);
            if (TorsoText != null)
            {
                TorsoText.SetActive(true);
            }
		} else if (torsoHealth >= 5) {
			TorsoHUD.SetActive (false);
            if (TorsoText != null)
            {
                TorsoText.SetActive(false);
            }
			damagedTorso = false;
			foreach(Renderer r in Torso.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.white;
			}
		}

		if (SceneManager.GetActiveScene ().name == "Tutorial_level") {
			if (torsoHealth >= 5 && leftArmHealth >= 5 && rightArmHealth >= 5){
				StartCoroutine (VerticalSliceTransition ());
			}
		}
		//test input to heal
		/*if (Input.GetKeyDown (KeyCode.B)) {
			rightArmHealth += 1;
		}*/
	}
	//end update()

	IEnumerator VerticalSliceTransition () {
		yield return new WaitForSeconds (transitionDelay);
		SceneManager.LoadScene ("START_cutscene");
	}

	IEnumerator GuardHitDelay () {
		yield return new WaitForSeconds (hitDelayTime);
		colossusHealth -= guardDamage;
		intGrd_Dmg = Mathf.RoundToInt (guardDamage);
		colosHealth.doDamage (intGrd_Dmg);
		hitSound.Play ();
		Instantiate (explosion, colossusPos.transform.position, Quaternion.identity);
		if (colossusHealth <= colossusMaxHealth/2) {
			colYell.Play ();
		}
		if (colossusHealth <= 0) {
			colDying.Play (1);
			StartCoroutine (Win ());
		}
	}

	IEnumerator ColosHitDelay (int damagedPart) {
		yield return new WaitForSeconds (hitDelayTime);
		colHit.Play ();
		switch (damagedPart) {
		case (3):
			Warning.Play ();
			damagedTorso = true;
				torsoHealth = 0;
			Instantiate (explosion, Torso.transform.position, Quaternion.identity);
			foreach(Renderer r in Torso.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//Torso.SetActive(true);
			break;
		case (2):
			Warning.Play ();
			damagedRightLeg = true;
				rightLegHealth = 0;
			Instantiate (explosion, RightLeg.transform.position, Quaternion.identity);
			foreach(Renderer r in RightLeg.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//RightLeg.SetActive(true);
			break;
		case (1):
			Warning.Play ();
			damagedRightArm = true;
				rightArmHealth = 0;
			Instantiate (explosion, RightArm.transform.position, Quaternion.identity);
			foreach(Renderer r in RightArm.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//RightArm.SetActive(true);
			break;
		case (6):
			Warning.Play ();
			damagedTorso = true;
				torsoHealth = 0;
			Instantiate (explosion, Torso.transform.position, Quaternion.identity);
			foreach(Renderer r in Torso.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//Torso.SetActive(true);
			break;
		case (5):
			Warning.Play ();
			damagedLeftLeg = true;
				leftLegHealth = 0;
			Instantiate (explosion, LeftLeg.transform.position, Quaternion.identity);
			foreach(Renderer r in LeftLeg.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//LeftLeg.SetActive(true);
			break;
		case (4):
			Warning.Play ();
			damagedLeftArm = true;
				leftArmHealth = 0;
			Instantiate (explosion, LeftArm.transform.position, Quaternion.identity);
			foreach(Renderer r in LeftArm.GetComponentsInChildren<Renderer>()){
				r.material.color = Color.red;
			}
				//LeftArm.SetActive(true);
			break;
		default:
			Debug.Log ("Switch Broke");
			break;
		}
	}

	public void HealRobot (int part) {
		if (!healing) {
			StartCoroutine (HealPart (part));
			healing = !healing;
		}
	}

	IEnumerator HealPart (int part) {
		yield return new WaitForSeconds (healDelay);

		switch (part) {
		case (1):
			if (rightArmHealth <= 5) {
				rightArmHealth += 1;
			}
			break;
		case (2):
			if (rightLegHealth <= 5) {
				rightLegHealth += 1;
			}
			break;
		case (3):
			if (leftArmHealth <= 5) {
				leftArmHealth += 1;
			}
			break;
		case (4):
			if (leftLegHealth <= 5) {
				leftLegHealth += 1;
			}
			break;
		case (5):
			if (torsoHealth <= 5) {
				torsoHealth += 1;
			}
			break;
		default:
			Debug.Log ("Healing broked");
			break;
		}

		healing = !healing;
	}

	IEnumerator TurnDelay () {
		yield return new WaitForSeconds (turnDelayTime);
		colAnim.SetInteger ("Colos_AnimState", 0);
		guardAnim.SetInteger ("Guard_AnimState", 0);
		turn = !turn;
		attack = true;
	}

	IEnumerator GuardianDeathCheck () {
		yield return new WaitForSeconds (guardianDeathCheckDelay);
		if (damagedTorso) {
			//put UI here
			Debug.Log("Guardian dying");
			StartCoroutine(GuardianDeathDelay());
		}
		else {
			Debug.Log ("Guardian torso repaired");
			guardianDying = false;
		}
	}

	IEnumerator GuardianDeathDelay () {
		yield return new WaitForSeconds (guardianDeathDelay);
		SceneManager.LoadScene ("Lose_cutscene"/*SceneManager.GetActiveScene ().name*/);
	}

	IEnumerator Win () {
		yield return new WaitForSeconds (winDelay);
		//insert win screen code.
		SceneManager.LoadScene ("Win_cutscene"/*SceneManager.GetActiveScene ().name*/);
	}
}
