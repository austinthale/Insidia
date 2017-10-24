using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundEngine_JR : MonoBehaviour {

    public float meleeVolume = 0.5f;
    public float shootVolume = 0.5f;

    //actions to subscribe to in order to call sounds
    public static Action meleeSound;
    public static Action shootSound;

    //sounds
    public AudioClip melee;
    public AudioClip shoot;

    //audioSource
    public AudioSource audioSource;


	// Use this for initialization
	void Start () {
        //subscript actions
        meleeSound += MeleeHandler;
        shootSound += shootSound;
	}
	
    //Handler for Melee Action
    void MeleeHandler()
    {
        audioSource.PlayOneShot(melee, meleeVolume);
    }
    
    //Handler for Shooting Action
    void ShootHandler()
    {
        audioSource.PlayOneShot(shoot, shootVolume);
    }
}
