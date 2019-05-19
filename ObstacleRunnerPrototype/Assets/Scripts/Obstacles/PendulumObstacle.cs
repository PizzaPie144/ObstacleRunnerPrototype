using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ObstacleRunner.Events;

namespace ObstacleRunner.Objstacles
{
    public class PendulumObstacle : Obstacle
    {
        [SerializeField]
        private float startDelay;
        private Vector3 downForce = new Vector3(0, -10f, 0);
        private Vector3 sideForce = new Vector3(0, 0, -50f);

        HingeJoint hingeJoint;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            hingeJoint = GetComponent<HingeJoint>();
        }


        #endregion

        #region Overrides
        #region Abstacts
        protected override IEnumerator Move()
        {
            if(startDelay != 0)
                yield return new WaitForSeconds(startDelay);

            do
            {
                if ((hingeJoint.limits.max - hingeJoint.angle) <= 2)
                    break;

                rigidbody.AddForce(sideForce * baseSpeed);
                yield return null;

            } while (true);

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            while (true)
            {
                rigidbody.AddForce(downForce * baseSpeed);

                yield return null;
            }
        }

        protected override void StopMove()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            if (MoveRoutine != null)
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