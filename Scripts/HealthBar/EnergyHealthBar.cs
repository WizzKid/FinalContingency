using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnergyHealthBar : MonoBehaviour
{
	private float fillAmount;

	[SerializeField] //Allows private field to be public, can be removed later after testing
	private float lerpSpeed;

	//green
	public Color fullColor;

	//red
	public Color lowColor;

	//Reference to the ColossusHealthBar, where its fillAmount can be edited
	[SerializeField]
	private Image content;

	public float MaxValue { get; set; }


	//EnergyHealthStat sends value update, which is formatted here for conversion to 0 - 1 range for fillAmount slider
	public float Value
	{
		set
		{
			//MaxValue can be replaced by the max value intended to be used
			fillAmount = Map(value, 0, MaxValue, 0, 1);
		}
	}
	
	//EnergyHealthBar updates every frame
	void Update ()
	{
		HandleBar();
	}

	private void HandleBar()
	{
		//if the energy value is different than displayed, this function will adjust the bar to the
		//correct amount steadily (Lerp variable)
		if (fillAmount != content.fillAmount)
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
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


}
