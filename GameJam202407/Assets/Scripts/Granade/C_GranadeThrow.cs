using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_GranadeThrow : MonoBehaviour
{
    public C_GrenadeData GrenadeData;
    public GameObject parentReference;
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
        Destroy(parentReference);
    }
    void ThrowObject()
    {
       rb.AddForce(GrenadeData.ThrowStrenght * transform.forward, ForceMode.Impulse);
    }
}
