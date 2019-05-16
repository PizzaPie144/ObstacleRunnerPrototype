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
        private Vector3 startPositionOffset;
        private Quaternion startRotation;

        private Coroutine inputRoutine;
        private Action winAction;
        private Action loseAction;


        #region Unity Callbacks

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();   
        }

        private void Start()
        {
            startRotation = transform.rotation;

            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
            GameMaster.Instance.SubscribeOnWin(OnWinHandler);
            GameMaster.Instance.SubscribeOnLose(OnLoseHandler);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Lose if collide with obstacle!
            if(loseAction != null && loseAction.Target != null)
                loseAction();
        }

        private void OnTriggerEnter(Collider other)
        {
            //probably used to define if finishline reached
            if (other.name == "FinishLine")
                if(winAction != null && winAction.Target != null)
                    winAction();
        }

        private void OnDestroy()
        {
            winAction = null;
            loseAction = null;

            if (GameMaster.Instance != null) //else has been destroyed before
            {
                GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStartHandler);
                GameMaster.Instance.UnsubscribeOnWin(OnWinHandler);
                GameMaster.Instance.UnsubscribeOnLose(OnLoseHandler);
            }
        }

        #endregion

        protected virtual IEnumerator InputRoutine()
        {
            while (true)
            {
                if (Input.GetKey(KeyCode.Space))    //directives between android and editor required
                {
                    navAgent.isStopped = true;
                    navAgent.velocity = Vector3.zero;
                }
                else
                {
                    navAgent.isStopped = false;
                }
                yield return null;
            }
        }

        #region Event Subscriber
        
        protected virtual void OnLevelStartHandler(object sender,LevelStartArgs args)
        {
            navAgent.enabled = true;

            transform.position = args.StartPosition + startPositionOffset;
            transform.rotation = startRotation;

            winAction = args.WinAction;
            loseAction = args.LoseAction;

            navAgent.velocity = Vector3.zero;
            navAgent.SetDestination(args.FinishLinePosition);

            if (inputRoutine != null)
                StopCoroutine(inputRoutine);

            inputRoutine = StartCoroutine(InputRoutine());
        }

        protected virtual void OnLevelRestart(object sender,EventArgs args)
        {
            //...
        }

        protected virtual void OnWinHandler(object sender,EventArgs args)
        {
            //Win animation can be played
            Disable();
        }

        protected virtual void OnLoseHandler(object sender,EventArgs args)
        {
            Disable();
        }

        #endregion

        private void Disable()
        {
            winAction = null;
            loseAction = null;
            navAgent.enabled = false;
            StopCoroutine(inputRoutine);
        }
    }
}