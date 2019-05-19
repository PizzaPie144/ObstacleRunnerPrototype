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
        //private Quaternion startRotation;
        //private Vector3 startPosition;

        private Action winAction;
        private Action loseAction;
        private Collider finishLineTrigger;

        private NavMeshAgent navAgent;
        private Animator _animator;
        private Vector2 smoothDeltaPosition = Vector2.zero;
        private Vector2 velocity = Vector2.zero;

        #region Animator Params
        private int anim_move_id;
        private int anim_velx_id;
        private int anim_vely_id;
        private int anim_Restart_id;
        private int anim_Win_id;
        #endregion

        protected Coroutine moveRoutine;

        #region Unity Callbacks

        protected virtual void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            anim_move_id = Animator.StringToHash("move");
            anim_velx_id = Animator.StringToHash("velx");
            anim_vely_id = Animator.StringToHash("vely");
            anim_Restart_id = Animator.StringToHash("Restart");
            anim_Win_id = Animator.StringToHash("Win");

            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
            GameMaster.Instance.SubscribeOnWin(OnWinHandler);
            GameMaster.Instance.SubscribeOnLose(OnLoseHandler);

            navAgent.updatePosition = false;
        }

        protected virtual void OnDestroy()
        {
            winAction = null;
            loseAction = null;

            if (GameMaster.Instance != null) 
            {
                GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStartHandler);
                GameMaster.Instance.UnsubscribeOnWin(OnWinHandler);
                GameMaster.Instance.UnsubscribeOnLose(OnLoseHandler);
            }
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
                    winAction();
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
                navAgent.isStopped = InputHandler();

                Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;

                float dx = Vector3.Dot(transform.right, worldDeltaPosition);
                float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
                Vector2 deltaPosition = new Vector2(dx, dy);

                float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

                if (Time.deltaTime > 1e-5f)
                    velocity = smoothDeltaPosition / Time.deltaTime;

                bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;
                _animator.SetBool(anim_move_id, shouldMove);
                _animator.SetFloat(anim_velx_id, velocity.x);
                _animator.SetFloat(anim_vely_id, velocity.y);

                if (worldDeltaPosition.magnitude > navAgent.radius)
                    transform.position = navAgent.nextPosition - 0.9f * worldDeltaPosition;

                yield return null;
            }
        }

        #region Event Subscriber

        protected virtual void OnLevelStartHandler(object sender, LevelStartArgs args)
        {
            DisableMovement();
            ResetState(args.StartTransform);

            _animator.SetTrigger(anim_Restart_id);

            winAction = args.WinAction;
            loseAction = args.LoseAction;

            navAgent.destination = args.FinishLinePosition;

            finishLineTrigger = args.FinishLineTrigger;

            moveRoutine = StartCoroutine(Move());
        }

        protected virtual void OnWinHandler(object sender, EventArgs args)
        {
            _animator.SetBool(anim_move_id, false);
            _animator.SetLayerWeight(_animator.GetLayerIndex("NoLegs"), 1);
            _animator.SetTrigger(anim_Win_id);          //win animation
            DisableMovement();
        }

        protected virtual void OnLoseHandler(object sender, EventArgs args)
        {
            DisableMovement();
            _animator.enabled = false;
        }

        #endregion

        private void DisableMovement()
        {
            winAction = null;
            loseAction = null;
            navAgent.enabled = false;
            _animator.SetBool(anim_move_id, false);
            _animator.SetFloat(anim_velx_id, 0f);
            _animator.SetFloat(anim_vely_id, 0f);

            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
        }

        protected virtual void ResetState(Transform startTransform)
        {
            transform.rotation = startTransform.rotation;
            transform.position = startTransform.position + startPositionOffset;
            navAgent.velocity = Vector3.zero;
            navAgent.enabled = true;
            _animator.enabled = true;
            _animator.SetLayerWeight(_animator.GetLayerIndex("NoLegs"), 0);
            _animator.SetBool(anim_move_id, false);
            _animator.SetFloat(anim_velx_id, 0f);
            _animator.SetFloat(anim_vely_id, 0f);
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
