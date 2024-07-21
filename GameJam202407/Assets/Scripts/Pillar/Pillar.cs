using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public GameObject DestroyedVersion; // Die Tr�mmerteile, die bei der Zerst�rung der S�ule entstehen
    public GameObject ThisPillar; // Parent der zerst�rten S�ule
    public void Destroy()
    {
        //l�sst die erstellten Tr�merteile erscheinen
        Instantiate(DestroyedVersion, transform.position, transform.rotation);

        // Zerst�rt die S�ule
        Destroy(ThisPillar);
    }


}
