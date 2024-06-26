using BTG.Utilities;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class  TeslaBallPool : MonoBehaviourObjectPool<TeslaBallView>
    {
        private TeslaBallView m_Prefab;

        [Inject]
        protected IObjectResolver m_Resolver;

        public TeslaBallPool(TeslaBallView prefab) => m_Prefab = prefab;

        public virtual TeslaBallView GetTeslaBall() => GetItem();

        public virtual void ReturnTeslaBall(TeslaBallView teslaBall) => ReturnItem(teslaBall);

        protected override TeslaBallView CreateItem()
        {
            TeslaBallView view = Object.Instantiate(m_Prefab, Container);
            view.SetPool(this);
            m_Resolver.Inject(view);
            return view;
        }
    }
}

