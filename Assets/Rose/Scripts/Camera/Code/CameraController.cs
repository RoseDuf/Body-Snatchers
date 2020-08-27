using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Camera
{

    public class CameraController : MonoBehaviour
    {
        Transform target;
        [SerializeField] float height; //above target
        [SerializeField] float distance; //behind or in front of target
        [SerializeField] float angle; //around the target
        [SerializeField] float smoothingSpeed;

        private Vector3 refVelocity;
        private float fixedLookAtRotation;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            fixedLookAtRotation = transform.rotation.z;
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected virtual void HandleCamera()
        {
            if (!target)
            {
                return;
            }

            Vector3 worldPosition = (Vector3.forward * -distance) + (target.up * height);
            //Debug.DrawLine(target.position, worldPosition, Color.red);

            Vector3 rotatedVector = Quaternion.AngleAxis(angle, target.up) * worldPosition;

            //move camera position to follow target
            Vector3 targetPosition = target.position;
            //targetPosition.y = 0f;
            Vector3 finalPosition = targetPosition + rotatedVector;

            //transform.position = finalPosition;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothingSpeed);
            transform.LookAt(target.position, target.forward);
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
