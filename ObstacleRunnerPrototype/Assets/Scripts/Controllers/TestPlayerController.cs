using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

using ObstacleRunner.Events;

namespace ObstacleRunner
{
    //this is a Test Controller -> No animations Used
    [RequireComponent(typeof(NavMeshAgent))]
    public class TestPlayerController : MonoBehaviour
    {
        private NavMeshAgent navAgent;
        [SerializeField]
        private Vector3 StartPositionOffset;

        #region Unity Callbacks

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();   
        }

        private void Start()
        {
            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Lose if collide with obstacle!
        }

        private void OnTriggerEnter(Collider other)
        {
            //probably used to define if finishline reached
        }

        private void OnDestroy()
        {
            if(GameMaster.Instance != null) //else has been destroyed before
                GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
        }

        #endregion

        protected virtual IEnumerator InputRoutine()
        {
            while (true)
            {
                if (Input.GetKey(KeyCode.Space))    //directives between android and editor required
                    navAgent.isStopped = true;
                else
                    navAgent.isStopped = false;

                yield return null;
            }
        }

        #region Event Subscriber

        protected virtual void OnLevelStartHandler(object sender,LevelStartArgs args)
        {
            transform.position = args.StartPosition + StartPositionOffset; 
            navAgent.SetDestination(args.FinishLinePosition);
            StartCoroutine(InputRoutine());
        }

        protected virtual void OnLevelRestart(object sender,EventArgs args)
        {

        }

        #endregion
    }
}