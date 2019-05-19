using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObstacleRunner.Events;

namespace ObstacleRunner.Objstacles
{
    public class HammerObstacle : Obstacle
    {
        private Vector3 downForce = new Vector3(1,1,1);
        private Vector3 upVelocity = new Vector3(-1, 0, 0);
        [SerializeField]
        private float downForceBase = 50f;
        [SerializeField]
        private float retractDelay;
        [SerializeField]
        private HammerHead hammerHead;
        [SerializeField]
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
        protected override IEnumerator Move()
        {
            yield return new WaitForSeconds(startDelay);
            bool isRetract = true;
            while (true)
            {
                //up
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
                        //Debug.Log(hingeJoint.angle + " < ?? " + hingeJoint.limits.min + " -> " + (a < b));    
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
