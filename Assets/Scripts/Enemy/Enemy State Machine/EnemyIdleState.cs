﻿using BTG.Utilities;
using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyIdleState : EnemyAliveState
    {
        private const int MAX_IDLE_TIME = 3;
        private float m_IdleTime = 0;
        private float m_TimeElapsed = 0;

        public EnemyIdleState(EnemyState state) : base(state)
        {

        }

        public override void Enter()
        {
            NextState = EnemyState.Idle;
            m_IdleTime = Random.Range(0, MAX_IDLE_TIME);
        }

        public override void Update()
        {
            m_TimeElapsed += Time.deltaTime;
            if (m_TimeElapsed >= m_IdleTime)
                NextState = EnemyState.Move;
        }

        public override void Exit()
        {
            // do nothing for now
        }
    }
}
