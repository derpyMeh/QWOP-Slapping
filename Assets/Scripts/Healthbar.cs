using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthbarIMG;

    public void UpdateHealthbar(float damageAmount)
    {
        healthbarIMG.fillAmount = damageAmount;
    }
}
