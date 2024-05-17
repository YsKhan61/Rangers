using BTG.Utilities;
using UnityEngine;

namespace BTG.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerView : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        public Transform Transform => transform;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
    }
}