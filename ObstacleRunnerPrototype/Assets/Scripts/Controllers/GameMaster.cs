using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ObstacleRunner.Events;

namespace ObstacleRunner
{
    /// <summary>
    /// Singleton* to dictate Game States and post notifications accordingly
    /// Main menu buttons' methods
    /// *Singleton implementation is partial
    /// </summary>
    public class GameMaster : MonoBehaviour
    {
        private static GameMaster instance;
        //Options
        [SerializeField]
        private float gameSpeed;            

        //UI Object References
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button exitButton;
        [SerializeField]
        private GameObject winScrene;
        [SerializeField]
        private GameObject loseScrene;

        //Game Object References
        [SerializeField]
        private Transform startTransform;
        [SerializeField]
        private Transform finishLineTransform;

        /// <summary>
        /// Invokes SpeedChange Event if different value is set
        /// </summary>
        private float GameSpeed { get { return gameSpeed; } set { if (gameSpeed != value) OnSpeedChange(new SpeedChangeArgs(value)); gameSpeed = value; } } //on line to rule them all

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

            winScrene.SetActive(false);
            loseScrene.SetActive(false);

            SubscribeOnLose(OnLoseHandler);
            SubscribeOnWin(OnWinHandler);
        }

        #endregion

        #region UI Button Handlers

        /// <summary>
        /// Start Game button callback handler
        /// </summary>
        private void StartGame()
        {
            winScrene.SetActive(false);
            loseScrene.SetActive(false);
            OnSpeedChange(new SpeedChangeArgs(gameSpeed));  //or pass the gamespeed via the LevelStartArgs  
            OnLevelStart(
                new LevelStartArgs
                (gameSpeed, startTransform, finishLineTransform.position, OnWin, OnLose, finishLineTransform.GetComponent<Collider>()));

        }

        /// <summary>
        /// Restart Button callback handler
        /// Currently same as StartGame()
        /// </summary>
        private void RestartLevel()
        {
            StartGame();     
            //OnLevelRestart();
        }

        /// <summary>
        /// Exit Button callback handler
        /// </summary>
        private void Exit()
        {
            Application.Quit();
        }
        
        #endregion

        #region Event Publisher
        #region OnLevelStart Event

        /// <summary>
        /// Event invoked once game starts or restarts*
        /// </summary>
        private event EventHandler<LevelStartArgs> levelStart;

        private void OnLevelStart(LevelStartArgs args)
        {
            if (levelStart != null)
                levelStart(this, args);
        }

        /// <summary>
        /// Subscribe to Level Start Event, invocked each time Game mode starts
        /// </summary>
        /// <param name="handler"></param>
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
        //there should probably be one!
        #endregion

        #region OnRestartLevel Event

        /// <summary>
        /// Event invoked on Level Restart (*** Not Implemented)
        /// </summary>
        private event EventHandler levelRestart;

        private void OnLevelRestart()
        {
            if (levelRestart != null)
                levelRestart(this, EventArgs.Empty);
        }

        /// <summary>
        /// Subscribe to Level Restart Event (*** Not Implemented)
        /// </summary>
        /// <param name="handler"></param>
        public void SubscribeOnLevelRestart(EventHandler handler)
        {
            levelRestart += handler;
        }

        public void UnsubscribeOnLevelRestart(EventHandler handler)
        {
            levelRestart -= handler;
        }

        #endregion

        #region OnSpeedChange Event

        /// <summary>
        /// Event invoked each time GameSpeed changes (*** Implemented, Not used)
        /// </summary>
        private event EventHandler<SpeedChangeArgs> speedChange;

        private void OnSpeedChange(SpeedChangeArgs args)
        {
            if (speedChange != null)
                speedChange(this, args);
        }

        /// <summary>
        /// Subscribe to Speed Change Event, Invoked each time Game Speed changes (*** Implemented, Not used)
        /// </summary>
        /// <param name="handler"></param>
        public void SubscribeOnSpeedChange(EventHandler<SpeedChangeArgs> handler)
        {
            speedChange += handler;
        }

        public void UnsubscribeOnSpeedChange(EventHandler<SpeedChangeArgs> handler)
        {
            speedChange -= handler;
        }

        #endregion
        #region Win Event

        /// <summary>
        /// Event externally invoked once win conditions met
        /// </summary>
        private event EventHandler win;

        private void OnWin()
        {
            if (win != null)
                win(this, EventArgs.Empty);
        }

        /// <summary>
        /// Subscribe to Win Event, Invoked once Win conditions met
        /// </summary>
        /// <param name="handler"></param>
        public void SubscribeOnWin(EventHandler handler)
        {
            win += handler;
        }

        public void UnsubscribeOnWin(EventHandler handler)
        {
            win -= handler;
        }

        #endregion

        #region Lose Event

        /// <summary>
        /// Event externally invoked once lose conditions met
        /// </summary>
        private event EventHandler lose;

        private void OnLose()
        {
            if (win != null)
                lose(this, EventArgs.Empty);
        }

        /// <summary>
        /// Subscribe to Lose Event, Invoked once lose conditions met
        /// </summary>
        /// <param name="handler"></param>
        public void SubscribeOnLose(EventHandler handler)
        {
            lose += handler;
        }

        public void UnsubscribeOnLose(EventHandler handler)
        {
            lose -= handler;
        }

        #endregion

        #endregion

        #region Event Subscriber

        protected void OnWinHandler(object sender, EventArgs args)
        {
            winScrene.SetActive(true);
        }

        protected void OnLoseHandler(object sender, EventArgs args)
        {
            loseScrene.SetActive(true);
        }

        #endregion
    }
}