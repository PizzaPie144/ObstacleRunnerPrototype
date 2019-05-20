using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ObstacleRunner.Objstacles
{
    /// <summary>
    /// Helix Obstacle, constant move speed
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class HelixObstacle : Obstacle
    {
        //angular velocity speed set to Obstacle
        private Vector3 angularVelocity = new Vector3(1, 1, 1);

        #region Unity Callbacks

        #endregion

        #region Overrides

        #region Abstracts

        /// <summary>
        /// Move method, defines how Obstacle moves
        /// </summary>
        /// <returns></returns>
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