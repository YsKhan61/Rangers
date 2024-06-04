using UnityEngine;

namespace BTG.UI
{
    public class ConnectionAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed = -50f;

        private void Update()
        {
            transform.Rotate(0, 0, _rotationSpeed * Mathf.PI * Time.deltaTime);
        }
    }
}