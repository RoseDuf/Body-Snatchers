using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Object;
using Game.UI;

namespace Game.Player
{
    public class CharacterController : GravityObject
    {
        [SerializeField] Vector2 speed;
        [SerializeField] Vector2 rotationSpeed;
        private Vector3 moveAmount;
        private Vector3 smoothMoveVelocity;
        private Vector3 inputVector;
        private bool isMoving;

        private Transform childObject;
        
        private float time;
        private float timeUntilNextPoint;
        Vector3 newPoint;
        Vector3 direction;

        private new void Awake()
        {
            base.Awake();
            childObject = transform.GetChild(0).transform;
        }

        private void Start()
        {
            inputVector = new Vector3(0, 0, 0);
            timeUntilNextPoint = Random.Range(1, 10f);
            time = timeUntilNextPoint + 1;
            newPoint = new Vector3(0f, 0f, 0f);
        }

        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            Movement();
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
            if (tag == "Roaming")
            {
                time += Time.deltaTime;
                if (time >= timeUntilNextPoint)
                {
                    timeUntilNextPoint = Random.Range(1, 10f);
                    newPoint = GenerateNewPoint();
                    direction = (transform.position - newPoint).normalized;
                    time = 0;
                }
                //Debug.DrawLine(planet.transform.position, transform.position);
                //Debug.DrawLine(planet.transform.position, newPoint);
                //Debug.DrawLine(transform.position, direction + transform.position);

                //rb.MovePosition(rb.position + transform.TransformDirection(direction) * speed * Time.deltaTime);
                rb.velocity = direction * speed.x * Time.deltaTime;
                FaceDirection();
            }
            else
            {
                //move character, transform.TransformDirection(moveAmount) changes world coordinates to local coordinates
                rb.velocity = transform.TransformDirection(moveAmount) * speed.y * Time.deltaTime;
                FaceDirection();
            }

            //update velocity weight according the the amount of items you have in your inventory
            //float velocity_weight = Mathf.Clamp(food.CountAmount() * 0.1f, 0f, 2f);
            //float weight_modifier = (1f + velocity_weight);
            
            //More ways of moving character:
            //rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * speed * Time.deltaTime);
            //transform.Translate(inputVector * Time.deltaTime);
        }

        private void FaceDirection()
        {
            //rotate rigidbody
            if (tag == "Roaming")
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), rotationSpeed.x * Time.deltaTime);
            }
            else
            {
                if (isMoving)
                    childObject.rotation = Quaternion.Slerp(childObject.rotation, Quaternion.LookRotation(transform.TransformDirection(moveAmount), transform.up), rotationSpeed.y * Time.deltaTime);
            }
        }

        private Vector3 GenerateNewPoint()
        {
            Vector3 point = new Vector3(0f, 0f, 0f);
            Vector3 XZVector = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 YVector = new Vector3(0f, transform.position.y, 0f);
            float planetRimVectorMagnitude = (transform.position - planet.transform.position).magnitude;

            float anglePhi = Random.Range((Vector3.SignedAngle(Vector3.forward, XZVector, Vector3.up) - 1f), (Vector3.SignedAngle(Vector3.forward, XZVector, Vector3.up) + 1f));
            float angleTheta = Random.Range((Vector3.Angle(Vector3.up, transform.up) - 1f), (Vector3.Angle(Vector3.up, transform.up) + 1f));

            //Do some trig to find the final position vector
            //X
            point.x = planetRimVectorMagnitude * Mathf.Sin(angleTheta * Mathf.Deg2Rad) * Mathf.Sin(anglePhi * Mathf.Deg2Rad);
            //Y
            point.y = planetRimVectorMagnitude * Mathf.Cos(angleTheta * Mathf.Deg2Rad);
            //Z
            point.z = planetRimVectorMagnitude * Mathf.Sin(angleTheta * Mathf.Deg2Rad) * Mathf.Cos(anglePhi * Mathf.Deg2Rad);

            return point + planet.transform.position;
        }

        private void SwitchBodies(GameObject newBody)
        {
            Timer.isAlive = false;
            newBody.tag = "Player";
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (tag == "Player" && collision.collider.tag == "Roaming")
            {
                SwitchBodies(collision.collider.gameObject);
            }
        }
    }
}

