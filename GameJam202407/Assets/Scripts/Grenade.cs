using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float Radius = 5;
    public LayerMask PillarMask;

    private new Transform transform;
    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public Transform TargetInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, PillarMask);
        foreach (Collider collider in colliders)
        {
            Destroy(collider.gameObject);
        }
        return null;
    }
}
