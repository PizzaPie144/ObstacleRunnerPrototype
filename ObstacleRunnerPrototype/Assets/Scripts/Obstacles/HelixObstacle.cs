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
        [SerializeField]
        private Vector3 angularVelocity = new Vector3(1, 1, 1);

        #region Unity Callbacks

        #endregion

        #region Overrides

        #region Abstracts

        protected override IEnumerator Move()
        {
            while (true)
            {
                yield return null;
                rigidbody.maxAngularVelocity = angularVelocity.y * GameSpeed * baseSpeed;
                rigidbody.angularVelocity = angularVelocity * GameSpeed * baseSpeed;

            }
        }

        protected override void StopMove()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
            if(MoveRoutine != null)
                StopCoroutine(MoveRoutine);
        }
        #endregion

        protected override void ResetState()
        {
            base.ResetState();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        #endregion
    }
}