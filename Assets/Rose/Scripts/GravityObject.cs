using UnityEngine;
using System.Collections;

namespace Game.Object
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityObject : MonoBehaviour
    {
        protected Rigidbody rb;
        protected GravityAttractor planet;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.useGravity = false;
            planet = GameObject.FindGameObjectWithTag("planet").GetComponent<GravityAttractor>();
        }

        private void FixedUpdate()
        {
            planet.Attract(transform);
        }
    }
}