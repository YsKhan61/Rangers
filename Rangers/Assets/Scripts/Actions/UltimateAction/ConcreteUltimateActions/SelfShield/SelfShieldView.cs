using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class SelfShieldView : MonoBehaviour, IDamageableView
    {
        public Transform Transform => transform;

        public bool IsPlayer { get; private set; }

        public Transform Owner { get; private set; }

        public bool IsVisible {get; private set; }

        public bool CanTakeDamage {get; private set; }

        public void SetOwner(Transform owner, bool isPlayer)
        {
            IsPlayer = isPlayer;
            Owner = owner;
        }

        public void SetVisible(bool isVisible) => IsVisible = isVisible;

        public void Damage(int damage)
        {
            // Do nothing for now.
        }

        public void Damage(ulong actorOwnerClientId, int damage)
        {
            // Do nothing for now
        }
    }

}
