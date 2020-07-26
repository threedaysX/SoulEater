using UnityEngine;

public class UIShake : MonoBehaviour
{
	public Vector3 originPosition;
	public Quaternion originRotation;
	public float shakeDecay = 0.002f;
	public float shakeIntensity = 0.3f;

	private float tempShakeIntensity = 0;
	private bool endShakedTrigger;

	public void Start()
	{
		originPosition = transform.position;
		originRotation = transform.rotation;
		endShakedTrigger = false;
	}

	public void Update()
	{
		if (tempShakeIntensity > 0)
		{
			transform.position = originPosition + Random.insideUnitSphere * tempShakeIntensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.y + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.z + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f,
				originRotation.w + Random.Range(-tempShakeIntensity, tempShakeIntensity) * 0.2f);
			tempShakeIntensity -= shakeDecay;
		}
		else if (!endShakedTrigger && tempShakeIntensity <= 0)
		{
			endShakedTrigger = true;
			tempShakeIntensity = 0;
			transform.position = originPosition;
			transform.rotation = originRotation;
		}
	}

	public void Shake()
	{
		tempShakeIntensity = shakeIntensity;
		endShakedTrigger = false;
	}

	public void Shake(float shakeIntensity, float shakeDecay = 0.002f)
	{
		tempShakeIntensity = shakeIntensity;
		this.shakeDecay = shakeDecay;
		endShakedTrigger = false;
	}
}
