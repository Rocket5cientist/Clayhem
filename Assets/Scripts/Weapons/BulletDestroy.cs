using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public Gun gun;

    private float travelDistance;
    private float velocity;
    private Vector3 startPos;

    private bool bulletCollided;

    Rigidbody rb;
    [SerializeField] ParticleSystem impactEffect;

    private void OnEnable() {
        startPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        bulletCollided = false;
    }

    private void OnCollisionEnter(Collision collision) {
        bulletCollided = true;
        rb.velocity = Vector3.zero;
        ParticleSystem impact = Instantiate(impactEffect, gameObject.transform.position, gameObject.transform.rotation);

        Destroy(impact, 1f);

        Destroy(gameObject);
    }

    private void Update() {
        travelDistance = gun.range;
        velocity = gun.bulletSpeed;

        if (bulletCollided == false) {
            //Bullet is rotated 180, so make velocity negative
            rb.velocity = -(transform.forward * velocity * Time.deltaTime); 
        }

        if (Vector3.Distance(transform.position, startPos) >= travelDistance) {
            Destroy(gameObject);
        }
    }



}
