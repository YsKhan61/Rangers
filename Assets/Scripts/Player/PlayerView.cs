using UnityEngine;

namespace BTG.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerView : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
    }
}