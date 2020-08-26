using UnityEngine;
using System.Collections;

namespace Game.Object
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityObject : MonoBehaviour
    {
        protected Rigidbody rb;
        protected GravityAttractor planet;
        
        private float time;
        [SerializeField] float timeUntilNextPoint;
        [SerializeField] protected float speed;
        [SerializeField] protected float rotationSpeed;

        Vector3 newPoint;
        Vector3 direction;

        protected void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.useGravity = false;
            planet = GameObject.FindGameObjectWithTag("planet").GetComponent<GravityAttractor>();
        }

        private void Start()
        {
            time = timeUntilNextPoint + 1;
            newPoint = new Vector3(0f, 0f, 0f);
        }

        private void FixedUpdate()
        {
            planet.Attract(transform);
            MoveAround();
        }

        private void MoveAround()
        {
            if (tag == "Roaming")
            {
                time += Time.deltaTime;
                if (time >= timeUntilNextPoint)
                {
                    newPoint = GenerateNewPoint();
                    direction = (transform.position - newPoint).normalized;
                    time = 0;
                }
                //Debug.DrawLine(planet.transform.position, transform.position);
                //Debug.DrawLine(planet.transform.position, newPoint);
                //Debug.DrawLine(transform.position, direction + transform.position);

                //rb.MovePosition(rb.position + transform.TransformDirection(direction) * speed * Time.deltaTime);
                rb.velocity = direction * speed * Time.deltaTime;
                FaceDirection();
            }
            else
            {
                return;
            }
        }

        private void FaceDirection()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), rotationSpeed * Time.deltaTime);
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
    }
}