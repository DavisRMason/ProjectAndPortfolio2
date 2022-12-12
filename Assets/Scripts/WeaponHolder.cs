using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] public Weapon selectedWeapon;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
    }

    public void DoThing()
    {
        gameManager.instance.playerScript.weaponFunc = selectedWeapon;
    }

    public void ChangeWeapon(Weapon weapon)
    {
        selectedWeapon = weapon;
    }
}
