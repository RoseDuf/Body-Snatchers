using UnityEngine;
using System.Collections;

namespace Game.Object
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityObject : MonoBehaviour
    {
        protected Rigidbody rb;
        protected GravityAttractor planet;
        
        private float time = 0;
        [SerializeField] float timeUntilNextPoint;
        [SerializeField] float defaultSpeed;

        Vector3 newPoint = new Vector3(0f, 0f, 0f);

        float angle = 1;

        protected void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.useGravity = false;
            planet = GameObject.FindGameObjectWithTag("planet").GetComponent<GravityAttractor>();
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
                    angle = Mathf.Clamp(Vector3.Angle(transform.up, newPoint - planet.transform.position), 1, 360);
                    print(newPoint - planet.transform.position);
                    print(transform.up);
                    print(angle);
                    time = 0;
                }
                rb.MovePosition(rb.position + transform.TransformDirection(newPoint) * defaultSpeed/angle * Time.deltaTime);
            }
            else
            {
                return;
            }
        }

        private Vector3 GenerateNewPoint()
        {
            Vector3 newPoint = new Vector3(0f, 0f, 0f);
            float planetRimVectorMagnitude = (transform.position - planet.transform.position).magnitude;
            
            float anglePhiX = Random.Range(0, 2*Mathf.PI);
            float angleThetaY = Random.Range(0, 2*Mathf.PI);
            float anglePhiZ = Random.Range(0, 2*Mathf.PI);

            //Do some trig to find the final position vector
            //X
            newPoint.x = planetRimVectorMagnitude * Mathf.Cos(angleThetaY) * Mathf.Cos(anglePhiX);
            ////Y
            newPoint.y = planetRimVectorMagnitude * Mathf.Sin(angleThetaY);
            ////Z
            newPoint.z = planetRimVectorMagnitude * Mathf.Cos(angleThetaY) * Mathf.Sin(anglePhiZ);

            return newPoint + planet.transform.position;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = new Color(0f, 0f, 1f, 0.25f);
        //    Vector3 newPoint = new Vector3(0f, 0f, 0f);
        //    float planetRimVectorMagnitude = (transform.position - planet.transform.position).magnitude;
        //    for (int i = 0; i < 2 * Mathf.PI; i++)
        //    {
        //        for (int j = 0; j < 2 * Mathf.PI; j++)
        //        {
        //            newPoint.x = planetRimVectorMagnitude * Mathf.Cos(i) * Mathf.Cos(j);
        //            //Y
        //            newPoint.y = planetRimVectorMagnitude * Mathf.Sin(i);
        //            //Z
        //            newPoint.z = planetRimVectorMagnitude * Mathf.Cos(i) * Mathf.Sin(j);

        //            Gizmos.DrawSphere(newPoint + planet.transform.position, 1.5f);
        //        }
        //    }
        //}
    }
}