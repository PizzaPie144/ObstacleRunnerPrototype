using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ObstacleRunner.Objstacles
{
    /// <summary>
    /// Pendulum Obstacle, moved via gravity like effect
    /// </summary>
    public class PendulumObstacle : Obstacle
    {
        //Delay before movement starts
        [SerializeField]
        private float startDelay;
        //Downard constant force applied
        private Vector3 downForce = new Vector3(0, -10f, 0);
        //Force to set Obstcle to side most position
        private Vector3 sideForce = new Vector3(0, 0, -50f);

        private HingeJoint hingeJoint;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            hingeJoint = GetComponent<HingeJoint>();
        }


        #endregion

        #region Overrides

        #region Abstacts

        /// <summary>
        /// Move method, defines how Obstacle moves
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator Move()
        {
            if (startDelay != 0)
                yield return new WaitForSeconds(startDelay);

            do
            {
                if ((hingeJoint.limits.max - hingeJoint.angle) <= 2)
                    break;

                rigidbody.AddForce(sideForce * baseSpeed * GameSpeed);
                yield return null;

            } while (true);

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            while (true)
            {
                rigidbody.AddForce(downForce * baseSpeed * GameSpeed);

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