using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObstacleRunner;
using ObstacleRunner.Events;

namespace ObstacleRunner.Objstacles
{
    public abstract class Obstacle : MonoBehaviour
    {
        [SerializeField]
        protected float baseSpeed = 1f;              //offset to fine tune move speed

        protected float GameSpeed { get; set; } = 1;//allows to pass the state to running routines
        protected Coroutine MoveRoutine { get; set; }   //the current move routine executed, if any
        protected bool isMoving { get; set; }

        protected Rigidbody rigidbody;

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
            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStart);
            GameMaster.Instance.SubscribeOnSpeedChange(OnSpeedChange);
        }

        protected virtual void OnDestroy()
        {
            GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStart);
            GameMaster.Instance.UnsubscribeOnSpeedChange(OnSpeedChange);
        }

        #endregion

        protected virtual void BeginMove(bool moveON)
        {
            isMoving = moveON;
            if (moveON)
            {
                if (MoveRoutine != null)
                    StopMove();

                ResetState();
                MoveRoutine = StartCoroutine(Move());
                isMoving = true;
            }
            else
            {
                StopMove();
                isMoving = false;
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

        protected virtual void ResetState()
        {
            transform.position = StartPosition;
            transform.rotation = StartRotation;
        }

        protected abstract IEnumerator Move();
        protected abstract void StopMove();
        
    }
}