using BTG.Utilities;
using UnityEngine;


namespace BTG.Test
{
    public class TestDamager : MonoBehaviour
    {
        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(1);
            }
        }
    }

}
