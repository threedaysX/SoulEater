using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour 
{
	public bool isStart;
	public Transform target;
	public float speed = 5f;
	public float delayAttractTimes = 0f;

	private ParticleSystem ps;
	private ParticleSystem.Particle[] m_Particles;
	private int numParticlesAlive;
	private float nextAttractTimes = 0f;

	private void Start () 
	{
		isStart = false;
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
	}

	private void Update () 
	{
		if (isStart && target != null)
		{
			if (Time.time >= nextAttractTimes)
			{
				m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
				numParticlesAlive = ps.GetParticles(m_Particles);
				float step = speed * Time.deltaTime;
				for (int i = 0; i < numParticlesAlive; i++) {
					m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, target.position, step);
				}
				ps.SetParticles(m_Particles, numParticlesAlive);
			}
		}
	}

	public void SetTargetMaster(Transform master)
	{
		nextAttractTimes = Time.time + delayAttractTimes;
		this.target = master;
		isStart = true;
	}
}
