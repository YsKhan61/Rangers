using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    public class RagdollPool : GenericObjectPool<RagdollView>
    {
        private RagdollDataSO m_Data;
        private Transform m_Container;

        public RagdollPool(RagdollDataSO data)
        {
            this.m_Data = data;
            m_Container = new GameObject("RagdollContainer of " + m_Data.name).transform;
        }

        public RagdollView GetRagdoll() => GetItem();

        public void ReturnRagdoll(RagdollView ragdoll) => ReturnItem(ragdoll);

        protected override RagdollView CreateItem()
        {
            RagdollView view = Object.Instantiate(m_Data.RagdollPrefab, m_Container);
            view.Initialize();
            view.SetPool(this);
            return view;
        }
    }
}