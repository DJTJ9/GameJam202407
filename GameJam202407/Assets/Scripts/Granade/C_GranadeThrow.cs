using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_GranadeThrow : MonoBehaviour
{
    public C_GrenadeData GrenadeData;
    public GameObject parentReference;
    public LayerMask HitMask;
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
        while(GrenadeData.ExplosionDelay > passedTime)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }

        OverlappTrace();

        Destroy(parentReference);
    }
    void ThrowObject()
    {
       rb.AddForce(GrenadeData.ThrowStrenght * transform.forward, ForceMode.Impulse);
    }

    void OverlappTrace()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, GrenadeData.ExplosionRadius,HitMask);
   
    foreach(Collider collider in colliders)
        {
        
        
        }
    }
}
