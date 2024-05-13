using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaximumSlider : Slider
{
	public MinimumSlider other = null;

	protected override void Set(float input, bool sendCallback)
	{
		float newValue = input;
        if (other != null) {
			if (wholeNumbers)
			{
				newValue = Mathf.Round(newValue);
			}

			if (newValue <= other.value)
			{
				return;
			}
		}
		base.Set(input, sendCallback);
	}
}
