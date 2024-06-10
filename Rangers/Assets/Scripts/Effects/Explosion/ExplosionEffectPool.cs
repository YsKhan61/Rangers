using BTG.Utilities;
using UnityEngine;


namespace BTG.Effects
{
    public class ExplosionEffectPool : GenericObjectPool<ExplosionView>
    {
        private ExplosionDataSO m_Data;
        private Transform m_Container;

        public ExplosionEffectPool(ExplosionDataSO data)
        {
            m_Data = data;
            m_Container = new GameObject("ExplosionContainer of " + data.name).transform;
        }

        public ExplosionView GetExplosionEffect() => GetItem();

        public void ReturnExplosionEffect(ExplosionView explosion) => ReturnItem(explosion);
        protected override ExplosionView CreateItem()
        {
            ExplosionView view = new GameObject(m_Data.name).AddComponent<ExplosionView>();
            view.transform.SetParent(m_Container);
            ParticleSystem ps = Object.Instantiate(m_Data.ParticleSystemPrefab, view.transform);
            AudioSource audioSource = view.gameObject.AddComponent<AudioSource>();
            audioSource.clip = m_Data.AudioClip;
            audioSource.loop = false;
            audioSource.spatialBlend = 1;
            audioSource.playOnAwake = false;
            view.Initialize(ps, audioSource, this);
            return view;
        }
    }

}
