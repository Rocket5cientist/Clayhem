using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public PlayerMovement movement;
    private BulletDestroy bulletDestroy;

    private Vector3 bulletSpawnRotation;

    bool firePressed;    

    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float criticalDamage;
    [SerializeField] public float range;
    [SerializeField] public float bulletSpeed;

    [SerializeField] private bool isAutomatic;

    [Header("Ammo")]
    [SerializeField] private float maxAmmo;
    [SerializeField] private float currentAmmo;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private GameObject bullet;

    [Header("Reloading")]
    [SerializeField] private float reloadTime;
    [SerializeField] private float fireRate;
    [SerializeField] private float nextTimeToFire = 0f;

    private void Start() {
        currentAmmo = maxAmmo;
    }

    void Update() {
        firePressed = movement.playerControls.Controls.Fire.IsPressed();

        if (firePressed && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot() {
        currentAmmo--;

        bulletSpawnRotation = bulletSpawn.transform.rotation.eulerAngles;
        bulletSpawnRotation = new Vector3(bulletSpawnRotation.x, bulletSpawnRotation.y + 180, bulletSpawnRotation.z);

        GameObject bulletClone = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(bulletSpawnRotation));
        bulletClone.GetComponent<BulletDestroy>().gun = this.gameObject.GetComponent<Gun>();

        //Bullet is rotated 180, so make velocity negative
        bulletClone.GetComponent<Rigidbody>().velocity = -(bulletClone.transform.forward * bulletSpeed * Time.deltaTime);
    }

}
