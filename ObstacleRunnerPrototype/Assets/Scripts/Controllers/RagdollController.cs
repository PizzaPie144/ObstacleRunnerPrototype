using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleRunner
{
    public class RagdollController : MonoBehaviour
    {
        private Collider[] childColliders;
        private Rigidbody[] childRigidbodies;

        private Collider parentCollider;
        private Rigidbody parentRigidbody;

        [SerializeField]
        private float colliderRadiousFactor = 0.5f;   

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
            EnableRagdoll(false);

            GameMaster.Instance.SubscribeOnLevelStart(OnLevelStartHandler);
            GameMaster.Instance.SubscribeOnLose(OnLoseHandler);
        }

        private void OnDestroy()
        {
            if (GameMaster.Instance != null)
            {
                GameMaster.Instance.UnsubscribeOnLevelStart(OnLevelStartHandler);
                GameMaster.Instance.UnsubscribeOnLose(OnLoseHandler);
            }
        }

        #endregion


        private void EnableRagdoll(bool isActive)
        {
            foreach (var rb in childRigidbodies)
                rb.isKinematic = !isActive;

            foreach (var col in childColliders)
                col.enabled = isActive;

            parentCollider.enabled = !isActive;
            parentRigidbody.isKinematic = !isActive;
        }

        //should be done on Editor and NOT runtime!
        private void AdjustColliders()
        {
            CapsuleCollider c;
            foreach(var col in childColliders)
            {
                if (typeof(CapsuleCollider) == col.GetType())
                {
                    c = col as CapsuleCollider;
                    c.radius *= colliderRadiousFactor;
                }
            }
        }

        private void OnLoseHandler(object sender,EventArgs args)
        {
            EnableRagdoll(true);
        }

        private void OnLevelStartHandler(object sender,EventArgs args)
        {
            EnableRagdoll(false);
        }


    }
}
