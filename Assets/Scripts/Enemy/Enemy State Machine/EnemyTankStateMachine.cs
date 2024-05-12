using BTG.StateMachine;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace BTG.Enemy
{
    public enum EnemyTankState
    {
        Idle,
        Patrol,
        PrimaryAttack,
        Ultimate,
        Dead
    }

    public class EnemyTankStateMachine : BaseStateMachine<EnemyTankState>, IUpdatable, IDestroyable
    {
        private EnemyTankController m_Controller;

        internal Vector3[] PatrolPoints => m_Controller.Data.PatrolPoints;
        internal NavMeshAgent Agent => m_Controller.Agent;
        internal bool IsTargetInRange => m_Controller.IsTargetInRange;
        internal Transform Transform => m_Controller.Transform;
        internal Transform TargetTransform => m_Controller.TargetView.Transform;
        internal bool IsUltimateReady => m_Controller.IsUltimateReady;


        public EnemyTankStateMachine(EnemyTankController controller)
        {
            m_Controller = controller;

            InitializeStates();
        }

        /// <summary>
        /// Initialize the state machine
        /// it will register to update and destroy callbacks
        /// </summary>
        public void Init(EnemyTankState stateToStart)
        {
            EditorInit();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);

            ChangeState(stateToStart);
        }

        /// <summary>
        /// Deinitialize the state machine
        /// It will unregister from update and destroy callbacks
        /// </summary>
        public void DeInit()
        {
            EditorDeInit();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        /// <summary>
        /// This method will change the current state of the state machine
        /// It is called only by the UnityMonoBehaviourCallbacks class 
        /// when the state machine is registered to update
        /// </summary>
        public void Update()
        {
            currentState?.Update();
        }

        /// <summary>
        /// This method is called by the UnityMonoBehaviourCallbacks class
        /// when the state machine is registered to destroy
        /// </summary>
        public void Destroy()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void OnUltimateExecuted()
        {
            ChangeState(EnemyTankState.PrimaryAttack);
        }

        /// <summary>
        /// This method is called by the Idle state when it is complete
        /// </summary>
        internal void OnIdleStateComplete()
        {
            ChangeState(EnemyTankState.Patrol);
        }

        /// <summary>
        /// This method is called by the Patrol state when it is complete
        /// </summary>
        internal void OnPatrolStateComplete()
        {
            ChangeState(EnemyTankState.Idle);
        }

        /// <summary>
        /// This method is called by states when the target is in range
        /// </summary>
        internal void OnTargetInRange()
        {
            ChangeState(EnemyTankState.PrimaryAttack);
        }

        /// <summary>
        /// This method is called by states when the target is not in range
        /// </summary>
        internal void OnTargetNotInRange()
        {
            ChangeState(EnemyTankState.Idle);
        }

        /// <summary>
        /// This method is called to execute the primary action of the owner
        /// </summary>
        internal void ExecutePrimaryAction() => m_Controller.ExecutePrimaryAction();

        /// <summary>
        /// This method is called when the ultimate of the entity is fully charged
        /// </summary>
        internal void OnUltimateReady() => ChangeState(EnemyTankState.Ultimate);

        /// <summary>
        /// This method is called to execute the ultimate action of the owner
        /// </summary>
        internal void ExecuteUltimateAction() => m_Controller.ExecuteUltimateAction();

        internal void OnDeath()
        {
            ChangeState(EnemyTankState.Dead);
            DeInit();
        }

        private void InitializeStates()
        {
            AddState(EnemyTankState.Idle, new EnemyTankIdleState(this));
            AddState(EnemyTankState.Patrol, new EnemyTankPatrolState(this));
            AddState(EnemyTankState.PrimaryAttack, new EnemyTankAttackState(this));
            AddState(EnemyTankState.Ultimate, new EnemyTankUltimateState(this));
            AddState(EnemyTankState.Dead, new EnemyTankDeadState(this));
        }

#if UNITY_EDITOR
        /// <summary>
        /// Only for debugging purposes
        /// </summary>
        /// <param name="state"></param>
        public void ChangeStateForTesting(EnemyTankState state)
        {
            ChangeState(state);
        }

        /// <summary>
        /// This method is called when the state machine is initialized
        /// </summary>
        private void EditorInit()
        {
            OnStateChanged += LogStateChanged;
        }

        /// <summary>
        /// This method is called when the state machine is deinitialized
        /// </summary>
        private void EditorDeInit()
        {
            OnStateChanged -= LogStateChanged;
        }

        private void LogStateChanged(EnemyTankState state) =>
            m_Controller.LogDescription($"State Changed: {state}");

#endif
    }
}
