using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ObstacleRunner;
using ObstacleRunner.Events;

namespace ObstacleRunner.Objstacles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class HelixObstacle : Obstacle
    {
        private Vector3 angularVelocity = new Vector3(1, 1, 1);

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            BeginMove(true);    //just for visual effect
        }

        //remove!
        protected  void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                BeginMove(false);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                BeginMove(true);
            }
        }

        #endregion

        #region Private Methods

        #endregion
        protected override IEnumerator Move()
        {
            while (true)
            {
                yield return null;
                rigidbody.maxAngularVelocity = angularVelocity.y * GameSpeed * baseSpeed;
                rigidbody.angularVelocity = angularVelocity * GameSpeed * baseSpeed;

            }
        }

        protected override void ResetState()
        {
            base.ResetState();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        protected override void StopMove()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
            if(MoveRoutine != null)
                StopCoroutine(MoveRoutine);
        }
        #region Overrides
        

        #endregion

        #region Event-Subscriber
        #endregion
    }
}