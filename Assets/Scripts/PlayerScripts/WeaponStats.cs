using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class WeaponStats : ScriptableObject
{
    public float shootRate;
    public int shootDist;
    public int shootDamage;
    public int ammoCount;
    public GameObject weaponModel;
    public GameObject hitEffect;
    public GameObject muzzleFlash;
    public List <AudioClip> weaponSound = new List<AudioClip>();
    public Shoot shootScript;
}
