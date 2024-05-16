﻿using BTG.StateMachine;
using BTG.Utilities;
using BTG.Utilities.DI;
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
        Damaged,
        Dead
    }

    public class EnemyTankStateMachine : BaseStateMachine<EnemyTankState>, IUpdatable, IDestroyable
    {
        /// <summary>
        /// Get the patrol points of the entity
        /// </summary>
        internal Vector3[] PatrolPoints => m_Controller.Data.PatrolPoints;

        /// <summary>
        /// Get the agent of the entity
        /// </summary>
        internal NavMeshAgent Agent => m_Controller.Agent;

        /// <summary>
        /// Is the target in range
        /// </summary>
        internal bool IsTargetInRange => m_Controller.TargetView != null;

        /// <summary>
        /// Get the transform of the entity
        /// </summary>
        internal Transform Transform => m_Controller.Transform;

        /// <summary>
        /// Get the target transform
        /// </summary>
        internal Transform TargetTransform => m_Controller.TargetView.Transform;

        /// <summary>
        /// Is the ultimate ready
        /// </summary>
        internal bool IsUltimateReady => m_Controller.IsUltimateReady;

        /// <summary>
        /// Is the primary action executing
        /// </summary>
        internal bool IsPrimaryActionExecuting { get; private set; }

        [Inject]
        private EnemyTankUltimateStateFactoryContainerSO m_UltimateStateFactoryContainer;

        private EnemyTankController m_Controller;


        public EnemyTankStateMachine(EnemyTankController controller) => m_Controller = controller;

        /// <summary>
        /// Create the states of the state machine according to the entity
        /// </summary>
        public void CreateStates()
        {
            AddState(EnemyTankState.Idle, new EnemyTankIdleState(this));
            AddState(EnemyTankState.Patrol, new EnemyTankPatrolState(this));
            AddState(EnemyTankState.PrimaryAttack, new EnemyTankAttackState(this));
            AddState(EnemyTankState.Ultimate, m_UltimateStateFactoryContainer.GetFactory(m_Controller.UltimateTag).CreateState(this));
            AddState(EnemyTankState.Damaged, new EnemyTankDamagedState(this));
            AddState(EnemyTankState.Dead, new EnemyTankDeadState(this));
        }

        /// <summary>
        /// Initialize the state machine after all the states are created
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

        /// <summary>
        /// Inform the state machine that the primary action has been executed
        /// </summary>
        public void OnPrimaryActionExecuted() => IsPrimaryActionExecuting = false;

        /// <summary>
        /// Inform the state machine that the ultimate action has been executed
        /// </summary>
        public void OnUltimateExecuted() => ChangeState(EnemyTankState.PrimaryAttack);

        /// <summary>
        /// Inform the state machine that the entity has taken damage
        /// </summary>
        public void OnDamageTaken() => ChangeState(EnemyTankState.Damaged);

        /// <summary>
        /// Inform the state machine that the target is in range
        /// </summary>
        public void OnTargetInRange() => ChangeState(EnemyTankState.PrimaryAttack);

        /// <summary>
        /// Inform the state machine that the target is not in range
        /// </summary>
        public void OnTargetNotInRange() => ChangeState(EnemyTankState.Idle);

        /// <summary>
        /// Inform the state machine that the Idle state is complete
        /// </summary>
        internal void OnIdleStateComplete() => ChangeState(EnemyTankState.Patrol);

        /// <summary>
        /// Inform the state machine that the Patrol state is complete
        /// </summary>
        internal void OnPatrolStateComplete() => ChangeState(EnemyTankState.Idle);

        /// <summary>
        /// Inform the state machine that the Damaged state is complete
        /// </summary>
        internal void OnDamagedStateComplete() => ChangeState(EnemyTankState.Idle);

        /// <summary>
        /// Inform the state machine to execute the primary action of the owner
        /// </summary>
        internal void ExecutePrimaryAction(int stopTime)
        {
            IsPrimaryActionExecuting = true;
            m_Controller.ExecutePrimaryAction(stopTime);
        }

        /// <summary>
        /// Inform the state machine that the ultimate is ready
        /// </summary>
        internal void OnUltimateReady() => ChangeState(EnemyTankState.Ultimate);

        /// <summary>
        /// Inform the state machine to execute the ultimate action of the owner
        /// </summary>
        internal void ExecuteUltimateAction() => m_Controller.ExecuteUltimateAction();

        /// <summary>
        /// This method is called to reallign the entity brain's transform with the rigidbody's transform
        /// </summary>
        internal void ReAllign() => m_Controller.ReAllign();

        /// <summary>
        /// Inform the state machine that the entity has died
        /// </summary>
        internal void OnEntityDead()
        {
            ChangeState(EnemyTankState.Dead);
            DeInit();
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
