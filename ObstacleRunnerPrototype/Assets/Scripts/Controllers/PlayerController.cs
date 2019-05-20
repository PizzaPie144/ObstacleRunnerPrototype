using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ObstacleRunner
{
    /// <summary>
    /// Player Controller, implements manual Input Method
    /// </summary>
    public class PlayerController : PlayerControllerBase
    {
        protected override bool InputHandler()
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))   
                return true;
            else
                return false;
        }
    }
}
