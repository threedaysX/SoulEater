using System.Collections;
using UnityEngine;

public class UIShake : MonoBehaviour
{
	public float shakeDecay = 0.002f;
	public float shakeIntensity = 0.3f;

	private float tempShakeIntensity = 0;
	private bool shaking;

	private Vector3 originPosition;
	private Quaternion originRotation;

	public void Start()
	{
		originRotation = transform.localRotation;
		shaking = false;
	}

	public void Update()
	{
		if (shaking)
		{
			transform.localPosition = originPosition + Random.insideUnitSphere * tempShakeIntensity;
			transform.localRotation = new Quaternion(
				originRotation.x + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.y + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.z + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.w + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f);
			tempShakeIntensity -= shakeDecay;
		}
		// Shake Finished
		if (tempShakeIntensity < 0)
		{
			shaking = false;
			tempShakeIntensity = 0;
			transform.localPosition = originPosition;
			transform.localRotation = originRotation;
		}
	}

	public void Shake()
	{
		originPosition = transform.localPosition;
		tempShakeIntensity = shakeIntensity;
		shaking = true;
	}

	public void Shake(float shakeIntensity, float shakeDecay = 0.002f)
	{
		originPosition = transform.localPosition;
		tempShakeIntensity = shakeIntensity;
		this.shakeDecay = shakeDecay;
		shaking = true;
	}
}
