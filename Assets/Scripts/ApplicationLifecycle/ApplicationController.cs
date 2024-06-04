using BTG.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;


namespace BTG.ApplicationLifecycle
{
    public class ApplicationController : LifetimeScope
    {
        [SerializeField]
        private string _nextScene;

        private IDisposable _subscriptionsDisposable;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            Application.targetFrameRate = 60;
            Application.wantsToQuit += OnApplicationWantsToQuit;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }

        private void Start()
        {
            ISubscriber<QuitApplicationMessage> quitApplicationSubscriber = Container.Resolve<ISubscriber<QuitApplicationMessage>>();
            DisposableGroup disposableGroup = new();
            disposableGroup.Add(quitApplicationSubscriber.Subscribe(QuitGame));
            _subscriptionsDisposable = disposableGroup;

            SceneManager.LoadScene(_nextScene);
        }

        private bool OnApplicationWantsToQuit()
        {
            return true;
        }

        private void QuitGame(QuitApplicationMessage message)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}