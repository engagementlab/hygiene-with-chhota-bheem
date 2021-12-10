using UnityEngine;

public class Particles : MonoBehaviour
{ 
	[HideInInspector]
	public ParticleSystem ParticleSystem;
	public ParticleSystem.MainModule Main;

	public bool OnAwake;
	
	[HideInInspector]
	public bool UniqueColor;
	[HideInInspector]
	public Color MyColor;

	public bool SetSize;

	public float Smallest;
	public float Largest;
	
	private ParticleSystem.ColorOverLifetimeModule _particleColor;
	private ParticleSystem.EmissionModule _emission;
	private ParticleSystem.EmitParams _emit;

	private void Awake()
	{
	
		ParticleSystem = GetComponent<ParticleSystem>();
		_emission = ParticleSystem.emission;
		Main = ParticleSystem.main;
		
	}

	public void PlayParticles(bool play)
	{
		
		_emission.enabled = play;
		_emit = new ParticleSystem.EmitParams();
		
		ParticleSystem.Play(true);

	}

	public void ParticleReduce(float rate)
	{
		
		var em = ParticleSystem.emission;
		em.rateOverTime = new ParticleSystem.MinMaxCurve(0, 4);
		
	}
}
