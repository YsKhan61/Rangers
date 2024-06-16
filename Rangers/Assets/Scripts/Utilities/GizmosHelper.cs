using BTG.Utilities;
using UnityEngine;

public class GizmosHelper : Singleton<GizmosHelper>
{

    [SerializeField] private float m_Offset = 10f;
    [SerializeField] private float m_Radius = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + transform.forward * m_Offset, m_Radius);       
    }
}
