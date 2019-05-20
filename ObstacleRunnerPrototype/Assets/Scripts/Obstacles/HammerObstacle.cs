using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleRunner.Objstacles
{
    /// <summary>
    /// Hammer Obstacle, should be coupled with HammerHead Component
    /// </summary>
    public class HammerObstacle : Obstacle
    {
        //Downward force applied
        private Vector3 downForce = new Vector3(1,1,1);
        //Constant velocity of Obstacle while moving upwards
        private Vector3 upVelocity = new Vector3(-1, 0, 0);

        [SerializeField]
        private float downForceBase = 50f;
        [SerializeField]
        //delay before obstacle moves upwards, once reached bottom
        private float retractDelay;
        [SerializeField]
        private HammerHead hammerHead;
        [SerializeField]
        //delay before Obstacle starts move
        private float startDelay = 0;

        private HingeJoint hingeJoint;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            hingeJoint = GetComponent<HingeJoint>();
        }

        #endregion

        #region Overrides

        #region Abstracts

        /// <summary>
        /// Move method, defines how Obstacle moves
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator Move()
        {
            yield return new WaitForSeconds(startDelay);
            bool isRetract = true;
            while (true)
            {
                //Up
                if (isRetract)
                {
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.velocity = Vector3.zero;
                    bool c = true;
                    float a;
                    float b;
                    do
                    {
                        a = hingeJoint.angle;
                        b = hingeJoint.limits.min;
                        c = hingeJoint.angle < hingeJoint.limits.max;               //What Sorcery is this?
                        rigidbody.angularVelocity = upVelocity * baseSpeed * GameSpeed;
                        yield return null;
                    } while (a > b);
                    isRetract = false;
                }
                //Down
                else
                {
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.velocity = Vector3.zero;

                    rigidbody.AddForce(downForce * baseSpeed * GameSpeed * downForceBase);
                    
                    do
                    {
                        yield return null;
                        float a = hingeJoint.angle;
                        float b = hingeJoint.limits.min;
                        if ( hammerHead.RaycastPathway())
                        {
                            isRetract = true;
                            yield return new WaitForSeconds(retractDelay);
                        }
                    } while (!isRetract);
                    isRetract = true;
                }

                yield return null;  //useless, just incase...
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
