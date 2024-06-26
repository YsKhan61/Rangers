using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    public class RagdollPool : MonoBehaviourObjectPool<RagdollView>
    {
        private RagdollDataSO m_Data;

        public RagdollPool(RagdollDataSO data)
        {
            m_Data = data;
        }

        public RagdollView GetRagdoll() => GetItem();

        public void ReturnRagdoll(RagdollView ragdoll) => ReturnItem(ragdoll);

        protected override RagdollView CreateItem()
        {
            RagdollView view = Object.Instantiate(m_Data.RagdollPrefab, Container);
            view.Initialize();
            view.SetPool(this);
            return view;
        }
    }
}