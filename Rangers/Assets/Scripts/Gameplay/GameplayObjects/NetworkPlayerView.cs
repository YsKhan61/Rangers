using BTG.Player;
using BTG.Utilities;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    [RequireComponent(typeof(NetworkAvatarGuidState))]
    public class NetworkPlayerView : NetworkBehaviour
    {
        private NetworkAvatarGuidState m_NetworkAvatarGuidState;

        private GameObject m_Graphics;
        private Pose m_SpawnPose;
        private PlayerTankController m_Controller;
        private Transform m_Target;

        public TagSO Tag => m_NetworkAvatarGuidState.RegisteredEntityData.Tag;

        private void Awake()
        {
            m_NetworkAvatarGuidState = GetComponent<NetworkAvatarGuidState>();
        }

        private void Update()
        {
            FollowTarget();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(m_Graphics);
        }

        public void SetFollowTarget(Transform target) => m_Target = target;

        public void SpawnGraphics()
        {
            m_Graphics = Instantiate(m_NetworkAvatarGuidState.RegisteredEntityData.Graphics, transform);
            m_Graphics.transform.localPosition = Vector3.zero;
            m_Graphics.transform.localRotation = Quaternion.identity;
        }

        private void FollowTarget()
        {
            if (m_Target == null)
                return;

            m_Target.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            transform.SetPose(new Pose(pos, rot));
        }
    }

}