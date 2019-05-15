using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ObstacleRunner.Events;

namespace ObstacleRunner
{
    //oversimplified Singleton
    //NullRefException incase instance gets destroyed 
    public class GameMaster : MonoBehaviour
    {
        private static GameMaster instance;
        [SerializeField]
        private float gameSpeed;            //don't use!
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Transform startTransform;
        [SerializeField]
        private Transform finishLineTransform;

        private float GameSpeed { get { return gameSpeed;  } set{ if(gameSpeed != value) OnSpeedChange(new SpeedChangeArgs(value)); gameSpeed = value; } } //on line to rule them all

        public static GameMaster Instance { get { return instance; } }

        #region Unity Callbacks

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);    //or this, depends if there we team Singletons in same GameObject

            //DontDestroyOnLoad         //not needed as we are using only one scene
        }

        private void Start()
        {
            //UI buttons Set up
            startButton.onClick.AddListener(StartGame);
            restartButton.onClick.AddListener(RestartLevel);
            exitButton.onClick.AddListener(Exit);
        }

        #endregion

        private void StartGame()
        {
            OnSpeedChange(new SpeedChangeArgs(gameSpeed));  //or pass the gamespeed via the LevelStartArgs
            OnLevelStart(new LevelStartArgs(gameSpeed,startTransform.position,finishLineTransform.position));

            startButton.interactable = false;
        }

        //remove???
        private void RestartLevel()
        {
            StartGame();    //... 
            //OnLevelRestart();
        }

        private void Exit()
        {
            Application.Quit();
        }

        #region Event Publisher
        #region OnLevelStart Event
        event EventHandler<LevelStartArgs> levelStart;

        private void OnLevelStart(LevelStartArgs args)
        {
            if (levelStart != null)
                levelStart(this, args);
        }

        public void SubscribeOnLevelStart(EventHandler<LevelStartArgs> handler)
        {
            levelStart += handler;
        }

        public void UnsubscribeOnLevelStart(EventHandler<LevelStartArgs> handler)
        {
            levelStart -= handler;
        }
        #endregion

        #region OnPause Event

        #endregion

        #region OnRestartLevel Event
        event EventHandler levelRestart;

        private void OnLevelRestart()
        {
            if (levelRestart != null)
                levelRestart(this, EventArgs.Empty);
        }

        private void SubscribeOnLevelRestart(EventHandler handler)
        {
            levelRestart += handler;
        }

        private void UnsubscribeOnLevelRestart(EventHandler handler)
        {
            levelRestart -= handler;
        }

        #endregion

        #region OnSpeedChange Event

        event EventHandler<SpeedChangeArgs> speedChange;

        private void OnSpeedChange(SpeedChangeArgs args)
        {
            if (speedChange != null)
                speedChange(this, args);
        }

        public void SubscribeOnSpeedChange(EventHandler<SpeedChangeArgs> handler)
        {
            speedChange += handler;
        }

        public void UnsubscribeOnSpeedChange(EventHandler<SpeedChangeArgs> handler)
        {
            speedChange -= handler;
        }

        #endregion

        #endregion

        #region Event Subscriber
        #endregion
    }
}