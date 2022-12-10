using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChange : MonoBehaviour
{
    [SerializeField] private Shader transparentShader;
    [SerializeField] private Shader standardShader;
    [SerializeField] private Transform targetPlayer;
    private float distanceToPlayer;

    private RaycastHit objectHit;
    private MeshRenderer hitRenderer = null;

    void Update()
    {
        RaycastHit raycastHit;
        distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        if (Physics.Raycast(transform.position, transform.forward * distanceToPlayer, out raycastHit)) {
            if (raycastHit.transform.tag != "Player") {
                objectHit = raycastHit;

                hitRenderer = objectHit.transform.GetComponent<MeshRenderer>();
                hitRenderer.material.shader = transparentShader;
            }
            else {
                if (hitRenderer != null) { 
                    hitRenderer.material.shader = standardShader;
                }
            }
        }
    }
}
