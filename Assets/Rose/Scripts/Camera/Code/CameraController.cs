﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Camera
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float height; //above target
        [SerializeField] float distance; //behind or in front of target
        [SerializeField] float angle; //around the target
        [SerializeField] float smoothingSpeed;

        private Vector3 refVelocity;

        private void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }

        protected virtual void HandleCamera()
        {
            if (!target)
            {
                return;
            }

            Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
            //Debug.DrawLine(target.position, worldPosition, Color.red);

            Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

            //move camera position to follow target
            Vector3 targetPosition = target.position;
            targetPosition.y = 0f;
            Vector3 finalPosition = targetPosition + rotatedVector;

            //transform.position = finalPosition;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothingSpeed);
            transform.LookAt(target.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            if (target)
            {
                Gizmos.DrawLine(transform.position, target.position);
                Gizmos.DrawSphere(target.position, 1.5f);
            }
            Gizmos.DrawSphere(transform.position, 1.5f);
        }
    }

}