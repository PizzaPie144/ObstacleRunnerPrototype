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

        public Vector3 angularVelocity;
        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();
            
        }
        #endregion

        #region Private Methods

        #endregion
        protected override IEnumerator Move()
        {
            while (true)
            {
                yield return null;
                rigidBody.maxAngularVelocity = angularVelocity.y;
                rigidBody.angularVelocity = angularVelocity;
            }
        }

        protected void StartMove(Vector3 angularVelocity)
        {
            angularVelocity = angularVelocity* GameSpeed;
            rigidBody.angularVelocity = angularVelocity;
        }
        #region Overrides
        //protected override IEnumerator Move()
        //{
        //    while (true)
        //    {
        //        //if (IsPaused)
        //        //    yield return null;  //suspend execution while paused

        //        if (!IsPaused || true)
        //        {
        //            rigidBody.angularVelocity = angularVelocity;
        //        }
        //        //yield re
        //    }
    
        #endregion

        #region Event-Subscriber
        #endregion
    }
}