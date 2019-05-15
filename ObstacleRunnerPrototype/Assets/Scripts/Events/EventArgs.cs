using System;
using UnityEngine;

namespace ObstacleRunner.Events
{
    public class SpeedChangeArgs : EventArgs
    {
        public float GameSpeed { get; private set; }

        public SpeedChangeArgs(float GameSpeed)
        {
            this.GameSpeed = GameSpeed;
        }
    }

    public class LevelStartArgs : EventArgs
    {
        public float GameSpeed { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public Vector3 FinishLinePosition { get; private set; }

        public LevelStartArgs(float GameSpeed, Vector3 StartPosition, Vector3 FinishLinePosition)
        {
            this.GameSpeed = GameSpeed;
            this.StartPosition = StartPosition;
            this.FinishLinePosition = FinishLinePosition;
        }
    }

    public class PauseArgs : EventArgs
    {
        public bool IsPaused { get; private set; }

        public PauseArgs(bool IsPaused)
        {
            this.IsPaused = IsPaused;
        }
    }
}