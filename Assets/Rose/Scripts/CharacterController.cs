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
        bool isMoving;
        
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
          
            inputVector = new Vector3(horizontal, 0f, vertical) * speed;
            moveAmount = Vector3.SmoothDamp(moveAmount, inputVector, ref smoothMoveVelocity, 0.15f);
        }

        private void Movement()
        {
            //move rigidbody
            // update velocity weight according the the amount of items you have in your inventory
            //float velocity_weight = Mathf.Clamp(food.CountAmount() * 0.1f, 0f, 2f);
            //float weight_modifier = (1f + velocity_weight);

            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
            //transform.Translate(inputVector * Time.deltaTime);
        }

        private void FaceDirection()
        {
            //rotate rigidbody
            if (isMoving)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVector), rotationSpeed * Time.deltaTime);
        }
    }

}

