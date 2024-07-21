using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public GameObject DestroyedVersion; // Die Trümmerteile, die bei der Zerstörung der Säule entstehen
    public GameObject ThisPillar; // Parent der zerstörten Säule
    public void Destroy()
    {
        //lässt die erstellten Trümerteile erscheinen
        Instantiate(DestroyedVersion, transform.position, transform.rotation);

        // Zerstört die Säule
        Destroy(ThisPillar);
    }


}
