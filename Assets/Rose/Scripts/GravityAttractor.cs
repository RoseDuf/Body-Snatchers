using UnityEngine;
using System.Collections;

namespace Game.Object
{
    public class GravityAttractor : MonoBehaviour
    {
        private const float GRAVITY = -100f;

        public void Attract(Transform body)
        {
            Vector3 creatureToPlanetVector = (body.position - transform.position).normalized;

            //gravity
            body.rotation = /*Quaternion.Slerp(transform.rotation, */Quaternion.FromToRotation(body.up, creatureToPlanetVector) * body.rotation/*, rotationSpeed * Time.deltaTime)*/;
            body.GetComponent<Rigidbody>().AddForce(creatureToPlanetVector * GRAVITY);
        }
    }

}