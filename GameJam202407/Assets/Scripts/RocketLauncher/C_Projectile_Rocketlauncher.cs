using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Projectile_Rocketlauncher : MonoBehaviour
{

    public GameObject parentReference;
    [SerializeField]
    public LayerMask HitMask;
    public LayerMask AddForceMask;
    public float ExplosionForce = 700f;
    public float ExplosionDelay = 0.5f;
    public float ExplosionRadius = 5f;
    public float FlyStrenght = 5f;
    Rigidbody rb;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent < Collider > ();
        StartCoroutine(Selfdestruct()); //starts explosion timer
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private IEnumerator Selfdestruct()
    {
        float passedTime = 0;
        while (ExplosionDelay > passedTime)
        {
            passedTime += Time.deltaTime;
            yield return null;
            MoveProjecile();
        }

        Explode();
        Debug.Log("Destroy Self");
    }

    void Explode()
    {
        // Sucht zuerst nach Pillars in der Range der Granante und zerstört diese
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Pillar pillar = nearbyObject.GetComponent<Pillar>();
            if (pillar != null)
            {
                pillar.Destroy();
            }
            Destroy(gameObject);
        }
        // Sucht dann nach Rigidbodies, die auf den Trümmerteilen liegen, um auf diese eine Kraft auszuwirken
        // und sie expolsionsartig wegfleigen zu lassen
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, ExplosionRadius, AddForceMask);

        foreach (Collider nearbyFragments in collidersToMove)
        {
            Rigidbody rb = nearbyFragments.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
        }
    }

    void MoveProjecile()
    {
        rb.AddForce(FlyStrenght * transform.forward, ForceMode.Force);
    }
}
