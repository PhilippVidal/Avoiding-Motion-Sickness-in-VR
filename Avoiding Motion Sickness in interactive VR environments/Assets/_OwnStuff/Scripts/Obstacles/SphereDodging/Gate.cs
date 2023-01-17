using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform m_OpenPosition;
    public GameObject m_Railings;
    public float m_TimeToOpen = 3.0f;
    public bool m_IsOpen;

    protected Vector3 m_ClosedPosition;

    protected void Start()
    {
        m_ClosedPosition = m_Railings.transform.position;
    }

    public void Open()
    {
        m_IsOpen = true;

        StartCoroutine(OpenGate());
    }

    protected IEnumerator OpenGate()
    {
        float t = 0.0f;
        float elapsedTime = 0.0f;
        while (t < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / m_TimeToOpen;
            m_Railings.transform.position = Vector3.Lerp(m_ClosedPosition, m_OpenPosition.position, t);
            yield return null;
        }


        gameObject.active = false;
        yield return null;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (m_IsOpen)
            return;

        if (other.gameObject.CompareTag("MovingObstacle"))
            Open();
    }
}
