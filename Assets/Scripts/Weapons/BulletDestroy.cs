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
    [SerializeField] ParticleSystem explosionEffect;

    private void OnEnable() {
        startPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        bulletCollided = false;
    }

    private void OnCollisionEnter(Collision collision) {
        bulletCollided = true;
        rb.velocity = Vector3.zero;

        if (gun.isExplosive) {
            ParticleSystem explosion = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);

            Collider[] colliders = Physics.OverlapSphere(transform.position, gun.expRadius);
            foreach (Collider hit in colliders) {
                Rigidbody explosionRB = hit.GetComponent<Rigidbody>();

                if (explosionRB != null)
                    explosionRB.AddExplosionForce(gun.expPower, transform.position, gun.expRadius, 3.0f);
            }
        } else {
            ParticleSystem impact = Instantiate(impactEffect, gameObject.transform.position, gameObject.transform.rotation);
        }

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
