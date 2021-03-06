using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BonFireFlicker : MonoBehaviour
{
    Light2D getLightScript;
    float flickerValue = 1f, topTarget, floorTarget;
	public float timeLeft = 10f;
    bool flick = false;

    void Start()
    {
        getLightScript = GetComponent<Light2D>();
        topTarget = Random.Range(1.25f, 1.5f);
        floorTarget = Random.Range(0.75f, 1f);
    }

    void Update()
    {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 1.25f)
		{
			flickerValue -= Time.deltaTime;
		} else {
			if(flick)
			{
				flickerValue -= Time.deltaTime;
			} else {
				flickerValue += Time.deltaTime;
			}

			if(flickerValue >= topTarget)
			{
				floorTarget = Random.Range(0.75f, 1f);
				flick = true;
			} else if(flickerValue <= floorTarget) {
				topTarget = Random.Range(1.25f, 1.5f);
				flick = false;
			}
		}

        getLightScript.intensity = flickerValue;
    }
}
