using BTG.Utilities;
using UnityEngine;


namespace BTG.AudioSystem
{
    internal class AudioPool : MonoBehaviourObjectPool<AudioView>
    {
        /// <summary>
        /// Get the audio view from the pool
        /// </summary>
        internal AudioView GetAudioView()
        {
            AudioView view = GetItem();
            view.Show();
            return view;
        }

        /// <summary>
        /// Return the audio view to the pool
        /// </summary>
        internal void ReturnAudio(AudioView audioView)
        {
            audioView.Hide();
            audioView.transform.SetParent(Container);
            ReturnItem(audioView);
        }

        protected override AudioView CreateItem()
        {
            AudioView view = new GameObject("AudioView").AddComponent<AudioView>();
            view.transform.SetParent(Container);
            view.SetPool(this);
            view.Show();
            return view;
        }
    }
}
