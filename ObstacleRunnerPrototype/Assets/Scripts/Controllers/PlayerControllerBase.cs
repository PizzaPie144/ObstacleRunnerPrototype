using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

using ObstacleRunner.Events;

namespace ObstacleRunner
{
    public /*abstract*/ class PlayerControllerBase : MonoBehaviour
    {
        [SerializeField]
        private Vector3 startPositionOffset;
        private Quaternion startRotation;

        protected Coroutine moveRoutine;

        private NavMeshAgent navAgent;
        private Action winAction;
        private Action loseAction;
        private Collider finishLineTrigger;

        private Animator _animator;
        private Vector2 smoothDeltaPosition = Vector2.zero;
        private Vector2 velocity = Vector2.zero;

        #region Unity Callbacks

        protected virtual void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            startRotation = transform.rotation;
            startPositionOffset = transform.position;       //!!!

            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
            GameMaster.Instance.SubscribeOnWin(OnWinHandler);
            GameMaster.Instance.SubscribeOnLose(OnLoseHandler);

            //navAgent.destination = new Vector3(-17.45f, 1.1f, 0.568f);
            navAgent.updatePosition = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (loseAction != null && loseAction.Target != null)
            {
                loseAction();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == finishLineTrigger)   
                if (winAction != null && winAction.Target != null)
                {
                    Debug.Log("WIN!!!");
                    winAction();
                }
                    
        }

        protected virtual void OnDestroy()
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

        private void OnAnimatorMove()
        {
            transform.position = navAgent.nextPosition;
        }

        #endregion

        protected virtual IEnumerator Move()
        {
            while (true)
            {
                //Debug.Log("Running");
                //if (Input.GetKey(KeyCode.Space))
                //    navAgent.isStopped = true;
                //else
                //    navAgent.isStopped = false;
                navAgent.isStopped = InputHandler();

                Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;

                // Map 'worldDeltaPosition' to local space
                float dx = Vector3.Dot(transform.right, worldDeltaPosition);
                float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
                Vector2 deltaPosition = new Vector2(dx, dy);

                // Low-pass filter the deltaMove
                float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

                // Update velocity if delta time is safe
                if (Time.deltaTime > 1e-5f)
                    velocity = smoothDeltaPosition / Time.deltaTime;

                bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;
                // Update animation parameters
                _animator.SetBool("move", shouldMove);
                _animator.SetFloat("velx", velocity.x);
                _animator.SetFloat("vely", velocity.y);

                // Pull character towards agent
                if (worldDeltaPosition.magnitude > navAgent.radius)
                    transform.position = navAgent.nextPosition - 0.9f * worldDeltaPosition;

                yield return null;
            }
        }

        #region Event Subscriber

        protected virtual void OnLevelStartHandler(object sender, LevelStartArgs args)
        {
            Disable();
            Debug.Log("Start");
            //_animator.ResetTrigger("Lose");
            //_animator.ResetTrigger("Win");
            _animator.SetTrigger("Restart");
            transform.position = startPositionOffset;//args.StartPosition + startPositionOffset;
            transform.rotation = startRotation;

            winAction = args.WinAction;
            loseAction = args.LoseAction;

            navAgent.enabled = true;
            navAgent.velocity = Vector3.zero;
            navAgent.destination = args.FinishLinePosition;

            finishLineTrigger = args.FinishLineTrigger;

            if (moveRoutine != null)
                StopCoroutine(moveRoutine);

            moveRoutine = StartCoroutine(Move());
        }

        protected virtual void OnWinHandler(object sender, EventArgs args)
        {
            //_animator.ResetTrigger("")
            Disable();
            _animator.SetBool("move", false);
            _animator.SetTrigger("Win");
        }

        protected virtual void OnLoseHandler(object sender, EventArgs args)
        {
            Disable();
            _animator.SetBool("move", false);
            _animator.SetTrigger("Lose");
        }

        #endregion

        private void Disable()
        {
            //_animator.ResetTrigger("Win");
            //_animator.ResetTrigger("Lose");
            winAction = null;
            loseAction = null;
            navAgent.enabled = false;
            if(moveRoutine != null)
                StopCoroutine(moveRoutine);
        }

        #region Abstract Methods
        protected virtual bool InputHandler()
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))    //directives between android and editor required
                return true;
            else
                return false;
        }
        #endregion


    }
}
