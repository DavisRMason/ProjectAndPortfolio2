using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DetectWeapon : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textBox;

    string[] weaponTut =
    {
        "LEFT CLICK - Throw Weapon\nRIGHT CLICK - Teleport to Weapon\nR-BUTTON - Recall Weapon",
        "LEFT CLICK - Throw Weapon\nRIGHT CLICK - Move to Weapon\nR-BUTTON - Recall Weapon\n(Works in Air)",
        "LEFT CLICK - Charge Forwards\nRIGHT CLICK - Charge Weapon\nR-BUTTON - Dispel Weapon\n(AoE Attack)",
        "LEFT CLICK - Attack\n\nGOOD LUCK :)",
        "LEFT CLICK - Throw Weapon\nRIGHT CLICK - Teleport to Weapon\nR-BUTTON - Recall Weapon\n(Works in Air)"
    };

    void Update()
    {
        weaponTutorialChange();
    }

    void weaponTutorialChange()
    {
        textBox.text = weaponTut[gameManager.instance.playerScript.currentWeapon];
    }
}
