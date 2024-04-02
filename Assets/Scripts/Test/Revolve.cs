using UnityEngine;

public class Revolve : MonoBehaviour
{

    [SerializeField] private Rigidbody m_SunRigidbody;
    [SerializeField] private Rigidbody m_PlanetRigidbody;

    double GRAVITATIONAL_CONSTANT = 6.674f * Mathf.Pow(10, -11);

    private void FixedUpdate()
    {
        Vector3 force = CalculateGravitationalForce();
        m_PlanetRigidbody.AddForce(force);
    }

    private Vector3 CalculateGravitationalForce()
    {
        Vector3 direction = m_SunRigidbody.position - m_PlanetRigidbody.position;
        float distance = direction.magnitude;
        float forceMagnitude = (float)(GRAVITATIONAL_CONSTANT * m_SunRigidbody.mass * m_PlanetRigidbody.mass / Mathf.Pow(distance, 2));
        Vector3 force = direction.normalized * forceMagnitude;
        return force;
    }
}
