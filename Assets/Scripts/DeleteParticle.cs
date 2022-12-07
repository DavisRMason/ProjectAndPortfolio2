using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticle : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule fuckYou = GetComponent<ParticleSystem>().main;
        fuckYou.loop = false;
        Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().main.duration);
    }
}
