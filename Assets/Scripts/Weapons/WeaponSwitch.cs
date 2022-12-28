using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    private PlayerMovement movement;

    private bool switchPressed;

    void Start()
    {
        movement = transform.root.GetComponent<PlayerMovement>();

        SelectWeapon();
    }

    void Update()
    {
        switchPressed = movement.playerControls.Controls.WeaponSwitch.triggered;
        int prevSelectedWeapon = selectedWeapon;

        if (switchPressed) {
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0;
            } else {
                selectedWeapon++;
            }
        }

        if (prevSelectedWeapon != selectedWeapon) {
            SelectWeapon();
        }
    }

    void SelectWeapon() {
        int i = 0;
        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
            }
            else {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
