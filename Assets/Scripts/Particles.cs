﻿using UnityEngine;

public class Particles : MonoBehaviour
{ 
	[HideInInspector]
	public ParticleSystem ParticleSystem;
	public ParticleSystem.MainModule Main;

	public bool OnAwake;

	public bool UniqueColor;
	public Color MyColor;

	public bool SetLifetime;

	public float Smallest;
	public float Largest;
	
	private ParticleSystem.ColorOverLifetimeModule _particleColor;
	private ParticleSystem.EmissionModule _emission;

	private void Awake()
	{
		ParticleSystem = GetComponent<ParticleSystem>();
		Main = ParticleSystem.main;
		
	}

	private void Start()
	{		

		_particleColor = ParticleSystem.colorOverLifetime;
				
		ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		
		_emission = ParticleSystem.emission;
		_emission.enabled = OnAwake;

		if (!OnAwake)
			ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		else
			ParticleSystem.Play(true);
	}

	public void PlayParticles(bool play)
	{
//		
//		if (UniqueColor)
//			Main.startColor = new ParticleSystem.MinMaxGradient(MyColor);

		if (SetLifetime)
			Main.startSize = Largest;
		
		_emission.enabled = play;
		
		if (!play)
			ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		else
			ParticleSystem.Play(true);

	}
	
	public void ParticleControl(bool on, Spells spell)
	{
		// Stop old particles
		ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	
		if (on) // Turn on new particles
		{
			Color myColor;

			if (UniqueColor)
				myColor = MyColor;
			else
			{
				// Show spell color particle system
				switch (spell)
				{
					case Spells.BigShoot:

						myColor = Color.cyan;
			
						break;
			
					case Spells.ScatterShoot:
				
						myColor = Color.yellow;
			
						break;
			
					case Spells.SpeedShoot:

						myColor = Color.red;
			
						break;
				
					default:
						myColor = Color.white;

						break;
				}
				
				_particleColor.color = new ParticleSystem.MinMaxGradient(Color.white, myColor);
				_emission.enabled = true;
				ParticleSystem.Play(true);
			}
		}
	
	}

	public void ParticleReduce(float rate)
	{
		if (Main.startSize.constant <= Smallest)
			return;
		
		var size = Main.startSize;
		size = Main.startSize.constant - rate;
		
		Main.startSize = size;
	}
}
