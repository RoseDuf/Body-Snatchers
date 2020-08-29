using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngleTheta;
    [Range(0,360)]
    public float viewAnglePhi;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //public Transform childObject;

    public List<Transform> visibleTargets = new List<Transform>();

    //private void Awake()
    //{
    //    childObject = transform.GetChild(0).transform;
    //}

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerable FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        //Collider[] Returns an array with all colliders touching or inside the sphere.
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i=0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAnglePhi/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAnglePhi(float anglePhi/*, bool angleIsGlobal*/)
    {
        //if(!angleIsGlobal)
        //{
        //    anglePhi += transform.eulerAngles.y;
        //}

        Vector3 vector = new Vector3(Mathf.Sin(anglePhi * Mathf.Deg2Rad), 0f, Mathf.Cos(anglePhi * Mathf.Deg2Rad));
        return transform.TransformDirection(vector);
    }

    public Vector3 DirFromAngleTheta(float angleTheta/*, bool angleIsGlobal*/)
    {
        float anglePhi = Vector3.SignedAngle(Vector3.forward, transform.forward, transform.up);
        //if (!angleIsGlobal)
        //{
        //    angleTheta += transform.eulerAngles.y;
        //    anglePhi += transform.eulerAngles.y;
        //}

        Vector3 vector = new Vector3(Mathf.Cos(angleTheta * Mathf.Deg2Rad) * Mathf.Sin(anglePhi * Mathf.Deg2Rad), Mathf.Sin(angleTheta * Mathf.Deg2Rad), Mathf.Cos(angleTheta * Mathf.Deg2Rad) * Mathf.Cos(anglePhi * Mathf.Deg2Rad));
        return transform.TransformDirection(vector);
    }
}
