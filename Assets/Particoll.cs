using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Particoll : MonoBehaviour
{
    public string targetTag = "Enemy";
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private List<ParticleCollisionEvent> collisionEvents;
    private CompositeDisposable disposable = new CompositeDisposable();
    private void OnEnable()
    {
        if(TryGetComponent<ParticleSystem>(out ps))
        {
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
        }
        else
        {
            Debug.LogError("NoPart");
        }

        this.OnParticleCollisionAsObservable()//.Where(other => other.CompareTag(targetTag))
        .Subscribe(other => DisableParticles(other)).AddTo(disposable);
    }

    private void DisableParticles(GameObject other)
    {
        Debug.Log(other.name);
        int collisionCount = ParticlePhysicsExtensions.GetCollisionEvents(ps, other, collisionEvents); //store the collision info
        int numParticleAlive = ps.GetParticles(particles);

        for (int i = 0; i < collisionCount; i++)
        {
            Vector3 collisionPos = collisionEvents[i].intersection; //specify the location of contact point
            for (int j = 0; j < numParticleAlive; j++)
            {
                if (Vector3.Distance(particles[j].position, collisionPos) < 0.1f)
                {
            particles[i].remainingLifetime = 0;
                }
            }
            ps.SetParticles(particles, numParticleAlive);
        }
    }

    private void OnDisable()
    {
        disposable.Clear();
    }
}
