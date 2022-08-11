using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public Slider teleportCooldown;

    public void SetTeleport(float teleport) {
        teleportCooldown.value = teleport;
    }
}
