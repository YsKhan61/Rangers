using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class  TeslaBallPool : GenericObjectPool<TeslaBallView>
    {
        private TeslaBallView m_Prefab;

        public TeslaBallPool(TeslaBallView prefab) => m_Prefab = prefab;

        public virtual TeslaBallView GetTeslaBall() => GetItem();

        public virtual void ReturnTeslaBall(TeslaBallView teslaBall) => ReturnItem(teslaBall);

        protected override TeslaBallView CreateItem()
        {
            TeslaBallView view = Object.Instantiate(m_Prefab);
            view.SetPool(this);
            return view;
        }
    }
}

