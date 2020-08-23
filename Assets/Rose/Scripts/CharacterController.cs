using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character
{

    public class CharacterController : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float rotationSpeed;

        private Rigidbody rb;

        private Vector3 inputVector;
        bool isMoving;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
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
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            isMoving = horizontal != 0 || vertical != 0;

            inputVector = new Vector3(horizontal, 0f, vertical);
        }

        private void Movement()
        {
            //move rigidbody
            // update velocity weight according the the amount of items you have in your inventory
            //float velocity_weight = Mathf.Clamp(food.CountAmount() * 0.1f, 0f, 2f);
            //float weight_modifier = (1f + velocity_weight);

            //movement_vector.y = 2 * Physics.gravity.y * Time.deltaTime;
            rb.velocity = inputVector * (speed/* / weight_modifier*/) * Time.deltaTime;

        }

        private void FaceDirection()
        {
            //rotate rigidbody
            if (isMoving)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVector), rotationSpeed * Time.deltaTime);
        }
    }

}

