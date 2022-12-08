using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public Slider dashCooldown;

    public void SetDash(float dash) {
        dashCooldown.value = dash;
    }
}
