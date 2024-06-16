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

        private void Awake()
        {
            m_NetworkAvatarGuidState = GetComponent<NetworkAvatarGuidState>();
        }

        public override void OnNetworkSpawn()
        {
            m_SpawnPose = new Pose(transform.position, transform.rotation);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(m_Graphics);
        }

        public void SpawnGraphics()
        {
            m_Graphics = Instantiate(m_NetworkAvatarGuidState.RegisteredEntityData.Graphics, transform);
            m_Graphics.transform.localPosition = Vector3.zero;
            m_Graphics.transform.localRotation = Quaternion.identity;
        }
    }

}