using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObstacleRunner.Events;

namespace ObstacleRunner.Objstacles
{
    /// <summary>
    /// Base class for all Obstacles
    /// </summary>
    public abstract class Obstacle : MonoBehaviour
    {
        //Factor to fine tune move speed individually for each Obstacle
        [SerializeField]
        protected float baseSpeed = 1f;

        //allows to pass the speed state to running routines
        protected float GameSpeed { get; set; } = 1;
        //the current move routine executed, if any
        protected Coroutine MoveRoutine { get; set; }   
        
        protected Rigidbody rigidbody;

        //use to save initial state
        protected Vector3 StartPosition { get; set; }
        protected Quaternion StartRotation { get; set; }

        #region Unity Callbacks

        protected virtual void Awake()
        {
            StartPosition = transform.position;
            StartRotation = transform.rotation;
            rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Start()
        {
            //Events Subscriptions
            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStart);
            GameMaster.Instance.SubscribeOnSpeedChange(OnSpeedChange);

            BeginMove(true);
        }

        protected virtual void OnDestroy()
        {
            //Events Unsubscribe
            if (GameMaster.Instance != null)
            {
                GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStart);
                GameMaster.Instance.UnsubscribeOnSpeedChange(OnSpeedChange);
            }
        }

        #endregion

        /// <summary>
        /// Base method to Start and Stop Movement
        /// </summary>
        /// <param name="moveON"></param>
        protected virtual void BeginMove(bool moveON)
        {
            if (moveON)
            {
                if (MoveRoutine != null)
                    StopMove();

                ResetState();
                MoveRoutine = StartCoroutine(Move());
            }
            else
            {
                StopMove();
            }
        }

        protected virtual void OnLevelStart(object sender, LevelStartArgs args)
        {
            BeginMove(true);
        }


        protected virtual void OnSpeedChange(object sender, SpeedChangeArgs args)
        {
            GameSpeed = args.GameSpeed;
        }

        /// <summary>
        /// Resets state to initial 
        /// </summary>
        protected virtual void ResetState()
        {
            transform.position = StartPosition;
            transform.rotation = StartRotation;
        }

        #region Abstacts
        
        /// <summary>
        /// Abstract method to define Movement
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Move();

        /// <summary>
        /// Abstact method to define Stop Movement
        /// </summary>
        protected abstract void StopMove();
        #endregion
    }
}