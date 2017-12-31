using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
	private float fillAmount;

	[SerializeField] //Allows private field to be public, can be removed later after testing
	private float lerpSpeed;

	//green
	public Color fullColor;

	//red
	public Color lowColor;

	//Reference to the PlayerHealthBar, where its fillAmount can be edited
	[SerializeField]
	private Image content;

	public float MaxValue { get; set; }

	public float deathTime;

    public Text deathConveyance;

    public PlayerController playerControls;

	public GameObject PlayerDeath;

	//HealthStat sends value update, which is formatted here for conversion to 0 - 1 range for fillAmount slider
	public float Value
	{
		set
		{
			//MaxValue can be replaced by the max value intended to be used
			fillAmount = Map(value, 0, MaxValue, 0, 1);
		}
	}
	
    void Awake()
    {
        playerControls.enabled = true;
    }


	//PlayerHealthBar updates every frame
	void Update ()
	{
		HandleBar();
	}

	private void HandleBar()
	{
		//if the health value is different than displayed, this function will adjust the bar to the
		//correct amount steadily (Lerp variable)
		if (fillAmount != content.fillAmount)
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}

		if (fillAmount <= 0) {
            //deathConveyance.text = "You Have Died";
            //playerControls.enabled = !playerControls;
            if (deathConveyance != null && playerControls != null && playerControls != null)
            {
                deathConveyance.text = "YOU HAVE DIED";
                playerControls.enabled = false;
                playerControls.anim.enabled = false;
                StartCoroutine(DeathDelay());
            }
		}

		//adjusts color with bar percentage from green to red
		content.color = Color.Lerp(lowColor, fullColor, fillAmount);
	}

	private float Map(float value, float inMin, float inMax, float outMin, float outMax)
	{
		//Converts input range to proportional range in 0-1 scale
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		//EXAMPLE
		//(78 - 0) * (1 - 0) / (230 - 0) + 0
		// 80 * 1 / 100 = 0.8
	}

	//Waits to reload the scene to give player time to realize they are dead
	IEnumerator DeathDelay ()
	{
		PlayerDeath.SetActive(true);
		yield return new WaitForSeconds (deathTime);
		SceneManager.LoadScene ("Lose_cutscene"/*SceneManager.GetActiveScene ().name*/);
        
    }
}
