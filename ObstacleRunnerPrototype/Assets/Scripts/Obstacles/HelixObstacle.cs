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
        private Rigidbody rigidBody;
        private Vector3 angularVelocity = new Vector3(1, 1, 1);

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();

            BeginMove(true);    //just for visual effect
        }

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
                rigidBody.maxAngularVelocity = angularVelocity.y * GameSpeed * baseSpeed;
                rigidBody.angularVelocity = angularVelocity * GameSpeed * baseSpeed;

            }
        }

        protected override void ResetState()
        {
            base.ResetState();
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        protected override void StopMove()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            
            if(MoveRoutine != null)
                StopCoroutine(MoveRoutine);
        }
        #region Overrides
        

        #endregion

        #region Event-Subscriber
        #endregion
    }
}