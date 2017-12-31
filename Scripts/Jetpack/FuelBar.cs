using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
	private float fillAmount;

	[SerializeField] //Allows private field to be public, can be removed later after testing
	private float lerpSpeed;

	//yellow
	public Color fullColor;

	//red
	public Color lowColor;

	//Image reference to FuelSlider, and subcomponent fillAmount
	[SerializeField]
	private Image content;

	public float MaxValue { get; set; }

	//Taken from FuelStat, formats float for conversion to 0 - 1 range
	public float Value
	{
		set
		{
			//MaxValue can be replaced by the max value intended to be used
			fillAmount = Map(value, 0, MaxValue, 0, 1);
		}
	}

	//Update fillAmount of content every frame
	void Update ()
	{
		HandleBar();
	}

	private void HandleBar()
	{
		//if the value is different than displayed, this function will adjust the bar to the
		//correct amount steadily (Lerp variable)
		if (fillAmount != content.fillAmount)
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}

		//adjusts color with bar percentage from yellow to red
		content.color = Color.Lerp(lowColor, fullColor, fillAmount);
	}

	private float Map(float value, float inMin, float inMax, float outMin, float outMax)
	{
		//Converts any range to corresponding proportion in 0 - 1 range
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		//EXAMPLE
		//(78 - 0) * (1 - 0) / (230 - 0) + 0
		// 80 * 1 / 100 = 0.8
	}
}
