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
        //[SerializeField]
        protected float baseSpeed;              //offset to fine tune move speed

        protected float GameSpeed { get; set; } //allows to pass the state to running routines
        protected Coroutine MoveRoutine { get; set; }   //the current move routine executed, if any

        protected Vector3 StartPosition { get; set; }
        protected Quaternion StartRotations { get; set; }

        #region Unity Callbacks

        protected virtual void Awake()
        {
            StartPosition = transform.position;
            StartRotations = transform.rotation;
        }

        protected virtual void Start()
        {
            //subscribe!
        }

        protected virtual void OnDestroy()
        {
            //unsubscribe!
        }

        #endregion

        protected virtual void BeginMove(bool moveON)
        {
            if (moveON)
            {
                if (MoveRoutine != null)
                {
                    StopCoroutine(MoveRoutine);
                    MoveRoutine = null;
                }
                MoveRoutine = StartCoroutine(Move());
            }
            else if (!moveON)
            {
                StopCoroutine(MoveRoutine);
                MoveRoutine = null;
            }
        }

        protected virtual void OnLevelStart(object sender, LevelStartArgs args)
        {
            transform.position = StartPosition;
            transform.rotation = StartRotations;
            BeginMove(false);
            BeginMove(true);
        }

        protected virtual void OnLevelFinish(object sender, EventArgs args)   //arguably useless! only use incase Level is not Destroyed
        {
            BeginMove(false);       //stop previous movement
            // or load game/main menu if it exists
        }

        protected virtual void OnLevelRestart(object sender, EventArgs args)  //Use this Instead of Reload Level
        {
            transform.position = StartPosition;
            transform.rotation = StartRotations;
            BeginMove(false);   //stop previous movement
            BeginMove(true);
        }

        protected virtual void OnSpeedChange(object sender, SpeedChangeArgs args)
        {
            GameSpeed = args.GameSpeed;
        }

        protected abstract IEnumerator Move();
        

        
    }
}