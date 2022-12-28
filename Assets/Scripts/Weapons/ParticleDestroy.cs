using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroy : MonoBehaviour
{
    private void OnEnable() {
        Destroy(gameObject, transform.GetComponent<ParticleSystem>().main.duration);
    }
}