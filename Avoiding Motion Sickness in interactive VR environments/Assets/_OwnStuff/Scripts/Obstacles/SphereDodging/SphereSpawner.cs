using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{

    public Transform[] m_SpawnPoints;
    public int m_MaxSphereAmount;
    public float m_SpawnInterval;

    public float m_ForceStrength = 1.0f;

    public bool m_UseRandomSpawnpoint = false;

    protected int m_CurrentIndex;

    public GameObject m_SpherePrefab;
    protected List<ObstacleSphere> m_InactiveSpheres;
    protected List<ObstacleSphere> m_ActiveSpheres;

    public void ActivateSpawner()
    {
        if (m_SpawnPoints.Length == 0)
        {
            m_SpawnPoints = new Transform[1];
            m_SpawnPoints[0] = transform;
        }

        m_InactiveSpheres = new List<ObstacleSphere>();
        for (int i = 0; i < m_MaxSphereAmount; i++)
        {
            ObstacleSphere obstacleSphere = Instantiate(m_SpherePrefab, m_SpawnPoints[0].position, Quaternion.identity, transform).GetComponent<ObstacleSphere>();

            if(!obstacleSphere)
            {
                Debug.LogError("[Sphere Spawner] Spawned sphere prefab does not have an \"ObstacleSphere\" script attached!");
                return;
            }

            obstacleSphere.gameObject.SetActive(false);
            obstacleSphere.m_Spawner = this;
            m_InactiveSpheres.Add(obstacleSphere);
        }

        m_ActiveSpheres = new List<ObstacleSphere>();
        StartCoroutine(SpawnSphereCoroutine());
    }

    public void DespawnSphere(ObstacleSphere sphere)
    {
        if (!sphere)
            return;

        sphere.m_CurrentLifetime = 0.0f;
        sphere.gameObject.SetActive(false);
        m_ActiveSpheres.Remove(sphere);
        m_InactiveSpheres.Remove(sphere);
        m_InactiveSpheres.Add(sphere);
        sphere.transform.position = m_SpawnPoints[0].position;
    }

    IEnumerator SpawnSphereCoroutine()
    {
        while(true)
        {
            if (!SpawnSphere())
                yield return new WaitForSeconds(1.0f);

            yield return new WaitForSeconds(m_SpawnInterval);
        }     
    }

    protected bool SpawnSphere()
    {
        if (m_InactiveSpheres.Count == 0)
            return false;


        ObstacleSphere sphere = m_InactiveSpheres[0];
        sphere.gameObject.SetActive(true);

        m_CurrentIndex = (m_CurrentIndex + 1) % m_SpawnPoints.Length;
        if (m_UseRandomSpawnpoint)
            m_CurrentIndex = Random.Range(0, m_SpawnPoints.Length);

        sphere.transform.position = m_SpawnPoints[m_CurrentIndex].position;
        m_InactiveSpheres.Remove(sphere);
        m_ActiveSpheres.Add(sphere);

        Rigidbody rb;
        if(sphere.gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.velocity = Vector3.zero;

            float x = Random.value * m_ForceStrength * Random.Range(-1.0f, 1.0f);
            float z = Random.value * m_ForceStrength;

            Vector3 forceVector = transform.TransformDirection(new Vector3(x, 0.0f, z));
            rb.velocity = forceVector;
        }


        return true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MovingObstacle"))
            DespawnSphere(other.gameObject.GetComponent<ObstacleSphere>());
    }
}
