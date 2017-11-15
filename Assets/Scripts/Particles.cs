using UnityEngine;

namespace DefaultNamespace
{
    public class Particles : MonoBehaviour
    { 
	    public ParticleSystem ParticleSystem;
	    private ParticleSystem.ColorOverLifetimeModule _particleColor;
	    private ParticleSystem.EmissionModule _emission;

	    public Color MyColor;
	    public bool OnAwake;

	    private void Awake()
	    {
		    ParticleSystem = GetComponent<ParticleSystem>();

		    if (!OnAwake)
		    {
			    _emission = ParticleSystem.emission;
			    _emission.enabled = false;

			    ParticleSystem.Stop();
		    }

		    _particleColor = ParticleSystem.colorOverLifetime;
	    }
        
        public void ParticleControl(bool on, Spells spell)
        {
		
            // Stop old particles
            ParticleSystem.Stop();
		
            if (on) // Turn on new particles
            {
                Color myColor;
			
                // Show Player spell color particle system
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
                ParticleSystem.Play();
            }
		
        }
    }
}