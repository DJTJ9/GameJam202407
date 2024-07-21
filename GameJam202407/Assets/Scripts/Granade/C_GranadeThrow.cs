using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_GranadeThrow : MonoBehaviour
{
    public C_GrenadeData GrenadeData;
    public GameObject parentReference;
    [SerializeField]
    public LayerMask HitMask;
    public float ExplosionForce = 700f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ThrowObject();
        StartCoroutine(Selfdestruct());
    }
    private IEnumerator Selfdestruct()
    {
        float passedTime = 0;
        while (GrenadeData.ExplosionDelay > passedTime)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }

        Explode();

        Destroy(parentReference);
    }
    void ThrowObject()
    {
        rb.AddForce(GrenadeData.ThrowStrenght * transform.forward, ForceMode.Impulse);
    }

    void Explode()
    {
        // Sucht zuerst nach Pillars in der Range der Granante und zerstört diese
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, GrenadeData.ExplosionRadius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Pillar pillar = nearbyObject.GetComponent<Pillar>();
            if (pillar != null)
            {
                pillar.Destroy();
            }           

        }
        // Sucht dann nach Rigidbodies, die auf den Trümmerteilen liegen, um auf diese eine Kraft auszuwirken
        // und sie expolsionsartig wegfleigen zu lassen
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, GrenadeData.ExplosionRadius/*, HitMask*/);

        foreach (Collider nearbyFragments in collidersToMove)
        {
            Rigidbody rb = nearbyFragments.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, GrenadeData.ExplosionRadius);
            }
        }
    }
}
