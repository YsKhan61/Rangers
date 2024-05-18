using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;


namespace BTG.AudioSystem
{
    public class AudioPool : GenericObjectPool<AudioView>, ISelfDependencyRegister
    {
        private Transform m_Container;

        public AudioPool()
        {
            m_Container = new GameObject("AudioContainer").transform;
        }

        public AudioView GetAudio() => GetItem();

        public void ReturnAudio(AudioView audioView)
        {
            audioView.Hide();
            audioView.transform.SetParent(m_Container);
            ReturnItem(audioView);
        }

        protected override AudioView CreateItem()
        {
            AudioView view = new GameObject("AudioView").AddComponent<AudioView>();
            view.transform.SetParent(m_Container);
            view.Show();
            return view;
        }
    }
}
