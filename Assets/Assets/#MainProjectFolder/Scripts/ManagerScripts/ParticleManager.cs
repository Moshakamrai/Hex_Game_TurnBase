using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region Singleton
    private static ParticleManager _instance;

    public static ParticleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ParticleManager");
                _instance = go.AddComponent<ParticleManager>();
            }
            return _instance;
        }
    }
    #endregion

    [System.Serializable]
    public class ParticleType
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }

    public List<ParticleType> particleTypes;

    private Dictionary<string, Queue<GameObject>> particlePools;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            InitializeParticlePools();
        }
        else
        {
            // If an instance already exists, destroy the new one
            Destroy(gameObject);
        }
    }

    void InitializeParticlePools()
    {
        particlePools = new Dictionary<string, Queue<GameObject>>();

        foreach (var particleType in particleTypes)
        {
            Queue<GameObject> particlePool = new Queue<GameObject>();

            for (int i = 0; i < particleType.poolSize; i++)
            {
                GameObject particle = Instantiate(particleType.prefab);
                particle.SetActive(false);
                particlePool.Enqueue(particle);
            }

            particlePools.Add(particleType.name, particlePool);
        }
    }

    public void PlayParticle(string particleType, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (particlePools.ContainsKey(particleType))
        {
            GameObject particle = GetPooledParticle(particleType);

            if (particle != null)
            {
                particle.transform.position = position;
                particle.transform.rotation = rotation;

                if (parent != null)
                {
                    particle.transform.SetParent(parent);
                }

                particle.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Particle type " + particleType + " not found!");
        }
    }

    GameObject GetPooledParticle(string particleType)
    {
        if (particlePools.ContainsKey(particleType))
        {
            if (particlePools[particleType].Count > 0)
            {
                return particlePools[particleType].Dequeue();
            }
            else
            {
                Debug.LogWarning("No available particles in the pool for type " + particleType);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Particle type " + particleType + " not found!");
            return null;
        }
    }

    public void ReturnToPool(string particleType, GameObject particle)
    {
        if (particlePools.ContainsKey(particleType))
        {
            particle.SetActive(false);
            particlePools[particleType].Enqueue(particle);
        }
        else
        {
            Debug.LogWarning("Particle type " + particleType + " not found!");
        }
    }

    // Function to play particle by name
    public void PlayParticleByName(string particleName, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        foreach (ParticleType particleType in particleTypes)
        {
            if (particleType.name == particleName)
            {
                PlayParticle(particleType.name, position, rotation, parent);
                return;
            }
        }
        Debug.LogWarning("Particle with name " + particleName + " not found!");
    }
}
