using UnityEngine;
using UnityEngine.SceneManagement;


namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactorySO")]
    public class RagdollFactorySO : EffectFactorySO
    {
        [SerializeField]
        private RagdollDataSO m_Data;
        public override EffectDataSO Data => m_Data;

        private RagdollPool m_Pool;
        private RagdollPool Pool => m_Pool ??= InitializePool();

        public override EffectView GetItem() => Pool.GetRagdoll();

        RagdollPool InitializePool()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var pool = new RagdollPool(m_Data);
            return pool;
        }

        void OnActiveSceneChanged(Scene current, Scene next)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            m_Pool?.ClearPool();
            m_Pool = null;
        }
    }
}