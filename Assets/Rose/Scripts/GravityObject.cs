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
        [SerializeField] float defaultSpeed;

        Vector3 newPoint;

        Vector3 direction;
        float angle;

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
            angle = 0;
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
                    direction = newPoint - transform.position;
                    //angle = Mathf.Clamp(Vector3.Angle(transform.up, newPoint - planet.transform.position), 1, 360);
                    time = 0;
                }
                //Debug.DrawLine(planet.transform.position, transform.position);
                //Debug.DrawLine(planet.transform.position, newPoint);

                rb.MovePosition(rb.position + transform.TransformDirection(direction) * defaultSpeed/angle * Time.deltaTime);
                FaceDirection();
            }
            else
            {
                return;
            }
        }

        private void FaceDirection()
        {
           transform.rotation = Quaternion.LookRotation(transform.TransformDirection(newPoint));
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
            ////Y
            point.y = planetRimVectorMagnitude * Mathf.Cos(angleTheta * Mathf.Deg2Rad);
            ////Z
            point.z = planetRimVectorMagnitude * Mathf.Sin(angleTheta * Mathf.Deg2Rad) * Mathf.Cos(anglePhi * Mathf.Deg2Rad);

            return point + planet.transform.position;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = new Color(0f, 0f, 1f, 0.25f);
        //    Vector3 point = new Vector3(0f, 0f, 0f);
        //    float planetRimVectorMagnitude = (transform.position - planet.transform.position).magnitude;
        //    Vector3 XZVector = new Vector3(transform.position.x, 0f, transform.position.z);
        //    Vector3 YVector = new Vector3(0f, transform.position.y, 0f);
        //    if (time >= timeUntilNextPoint)
        //    {
        //        for (float i = (Vector3.Angle(Vector3.up, transform.up) - 1f); i < (Vector3.Angle(Vector3.up, transform.up) + 1f); i++)
        //        {
        //            for (float j = (Vector3.Angle(Vector3.forward, XZVector) - 1f); j < (Vector3.Angle(Vector3.forward, XZVector) + 1f); j++)
        //            {
        //                point.x = planetRimVectorMagnitude * Mathf.Cos(i * Mathf.Deg2Rad) * Mathf.Cos(j * Mathf.Deg2Rad);
        //                //Y
        //                point.y = planetRimVectorMagnitude * Mathf.Sin(i * Mathf.Deg2Rad);
        //                //Z
        //                point.z = planetRimVectorMagnitude * Mathf.Cos(i * Mathf.Deg2Rad) * Mathf.Sin(j * Mathf.Deg2Rad);

        //                Gizmos.DrawSphere(point + planet.transform.position, 1.5f);
        //            }
        //        }
        //    }
        //}
    }
}