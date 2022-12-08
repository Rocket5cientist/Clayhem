using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public PlayerMovement movement;

    private Vector3 bulletSpawnRotation;

    //Input Handling
    bool reloadPressed;
    bool firePressed;    

    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float recoilForce;
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
    private bool isReloading = false;

    public Animator animator;

    private void Start() {
        currentAmmo = maxAmmo;
    }

    private void OnEnable() {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void HandleInput() {
        if (isAutomatic) {
            firePressed = movement.playerControls.Controls.Fire.triggered;
        }
        else {
            firePressed = movement.playerControls.Controls.Fire.WasPressedThisFrame();
        }

        reloadPressed = movement.playerControls.Controls.Reload.triggered;
    }

    void Update() {
        HandleInput();

        if (isReloading) {
            return;
        }

        if (currentAmmo <= 0 || reloadPressed) {
            StartCoroutine(Reload());
            return;
        }

        if (firePressed && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            movement.Recoil(recoilForce);
        }
    }

    IEnumerator Reload() {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
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
