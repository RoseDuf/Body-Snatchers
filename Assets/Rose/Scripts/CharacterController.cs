using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Object;

namespace Game.Player
{
    public class CharacterController : GravityObject
    {
        [SerializeField] float speed;
        [SerializeField] float rotationSpeed;
        
        private Vector3 moveAmount;
        private Vector3 smoothMoveVelocity;
        private Vector3 inputVector;
        private Transform childObject;
        private bool isMoving;

        private new void Awake()
        {
            base.Awake();
            childObject = transform.GetChild(0).transform;
        }

        private void Start()
        {
            inputVector = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            Movement();
            FaceDirection();
        }

        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            isMoving = horizontal != 0 || vertical != 0;
          
            inputVector = new Vector3(horizontal, 0f, vertical).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, inputVector, ref smoothMoveVelocity, 0.15f);
        }

        private void Movement()
        {
            //move rigidbody
            // update velocity weight according the the amount of items you have in your inventory
            //float velocity_weight = Mathf.Clamp(food.CountAmount() * 0.1f, 0f, 2f);
            //float weight_modifier = (1f + velocity_weight);

            //move character, transform.TransformDirection(moveAmount) changes world coordinates to local coordinates

            rb.velocity = transform.TransformDirection(moveAmount) * speed * Time.deltaTime;

            //More ways of moving character:
            //rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * speed * Time.deltaTime);
            //transform.Translate(inputVector * Time.deltaTime);
        }

        private void FaceDirection()
        {
            //rotate rigidbody
            if (isMoving)
                childObject.rotation = Quaternion.Slerp(childObject.rotation, Quaternion.LookRotation(transform.TransformDirection(moveAmount), transform.up), rotationSpeed * Time.deltaTime);
        }
    }

}

