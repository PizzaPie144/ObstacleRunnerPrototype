using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleRunner.Objstacles
{
    /// <summary>
    /// Component to allow HammerObstacle know when to Restract (should be Coupled with HammerObstacle)
    /// </summary>
    public class HammerHead : MonoBehaviour
    {
        private int layerID;
        private int inversedLayerID;

        //Raycast range to fine tune HammerHead and path collision effect
        [SerializeField]
        private float range;

        private void Awake()
        {
            layerID = LayerMask.NameToLayer("Pathway");
            inversedLayerID = 1 << layerID;
        }

        /// <summary>
        /// Raycast against Pathway Layer
        /// </summary>
        /// <returns></returns>
        public bool RaycastPathway()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, range, inversedLayerID))
            {
                if (hit.collider.gameObject.layer == layerID)
                    return true;
            }
            return false;
        }

    }
}
