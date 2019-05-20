using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleRunner
{
    /// <summary>
    /// Component to enable/ disable existing ragdoll and adjust underlying rigidbodies/ colliders
    /// State changes occure via GameMaster Events
    /// </summary>
    public class RagdollController : MonoBehaviour
    {
        private Collider[] childColliders;
        private Rigidbody[] childRigidbodies;

        private Collider parentCollider;
        private Rigidbody parentRigidbody;

        //the factor to change initial CapsuleColliders radius
        [SerializeField]
        private float colliderRadiousFactor = 0.8f;
        //Child rigidbodies angular drag value
        [SerializeField]
        private float angularDrag;
        //Child rigidbodies drag value
        [SerializeField]
        private float drag;

        #region Unity Callbacks

        private void Awake()
        {
            parentCollider = GetComponent<Collider>();
            parentRigidbody = GetComponent<Rigidbody>();

            childColliders = GetComponentsInChildren<Collider>(true);
            childRigidbodies = GetComponentsInChildren<Rigidbody>(true);
        }

        private void Start()
        {
            //Disables Ragdoll 
            EnableRagdoll(false);

            //Runtime Adjustments (*should be done on Editor)
            AdjustColliders();
            AdjustRigidbodies();

            //Events Subscriptions
            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
            GameMaster.Instance.SubscribeOnLose(OnLoseHandler);
        }

        private void OnDestroy()
        {
            //Events Unsubscribe
            if (GameMaster.Instance != null)
            {
                GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStartHandler);
                GameMaster.Instance.UnsubscribeOnLose(OnLoseHandler);
            }
        }

        #endregion

        /// <summary>
        /// Enables/ Disables underlying rigidbodies and colliders
        /// </summary>
        /// <param name="isActive"></param>
        private void EnableRagdoll(bool isActive)
        {
            foreach (var rb in childRigidbodies)
                rb.isKinematic = !isActive;

            foreach (var col in childColliders)
                col.enabled = isActive;

            parentCollider.enabled = !isActive;
            parentRigidbody.isKinematic = !isActive;
        }

        //Changes underlying CapsuleColliders radius based on colliderRadiousFactor field
        private void AdjustColliders()
        {
            CapsuleCollider c;
            foreach(var col in childColliders)
            {
                if (col.Equals(parentCollider))
                    continue;

                if (typeof(CapsuleCollider) == col.GetType())
                {
                    c = col as CapsuleCollider;
                    c.radius *= colliderRadiousFactor;
                }
            }
        }

        //Changes underlying Rigidbodies parameters Drag and AngularDrag based on drag and angularDrag fields
        private void AdjustRigidbodies()
        {
            foreach(var rb in childRigidbodies)
            {
                if (rb.Equals(parentRigidbody))
                    continue;

                rb.angularDrag = angularDrag;
                rb.drag = drag;
            }
        }

        #region Event Subscriber

        /// <summary>
        /// Enables Rangdoll on Lose Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnLoseHandler(object sender,EventArgs args)
        {
            EnableRagdoll(true);
        }

        private void OnLevelStartHandler(object sender,EventArgs args)
        {
            EnableRagdoll(false);
        }
        #endregion
    }
}
