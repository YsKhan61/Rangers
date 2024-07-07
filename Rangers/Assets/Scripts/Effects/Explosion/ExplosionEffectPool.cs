using BTG.Utilities;
using UnityEngine;


namespace BTG.Effects
{
    public class ExplosionEffectPool : GenericObjectPool<ExplosionView>
    {
        private ExplosionDataSO m_Data;
        public ExplosionDataSO Data => m_Data;

        public ExplosionEffectPool(ExplosionDataSO data)
        {
            m_Data = data;
        }

        public ExplosionView GetExplosionEffect() => GetItem();

        public void ReturnExplosionEffect(ExplosionView explosion) => ReturnItem(explosion);

        protected override ExplosionView CreateItem()
        {
            ExplosionView view = new GameObject(m_Data.name).AddComponent<ExplosionView>();
            ParticleSystem ps = Object.Instantiate(m_Data.ParticleSystemPrefab, view.transform);
            view.Initialize(ps, this, m_Data);
            return view;
        }
    }

}
