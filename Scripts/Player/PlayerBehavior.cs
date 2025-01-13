using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Lights")]
    public GameObject FlashlightLight;
    public GameObject LightBox; 

    [Header("Sounds")]
    public AudioSource WalkingAudio;
    public AudioSource FlashlightAudio;
    public AudioSource AmbientSound1;
    public AudioSource DeathSound; 

    [Header("Mask")]
    public GameObject mask;
    private int MaskTimer = 0; 

    [Header("Health")]
    public int TotalHealth = 100;

    [Header("Screens")]
    public GameObject Screen1;
    public GameObject Screen2;
    public GameObject Screen3;
    public GameObject Screen4;

    [Header("OnTrail")]
    public bool Ontrail;
    public int OntrailTimer = 0;
    // Update is called once per frame
    void Update()
    {
        FlashlightToggle();
        PlayerSounds();
        MaskToggle();
        DisplayBloodScreen(); 
    }
    /// <summary>
    /// -------------------------------------------------------------------------------------
    /// All sounds are here
    /// </summary>
    void PlayerSounds()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            WalkingAudio.enabled = true; 
        }
        else
        {
            WalkingAudio.enabled = false; 
        }
    }

    /// <summary>
    /// -------------------------------------------------------------------------------------
    /// Player Mechanics and usability tools are here
    /// </summary>
    void FlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashlightLight.SetActive(!FlashlightLight.activeInHierarchy);
            FlashlightAudio.Play();
        }
        if(FlashlightLight.activeInHierarchy && Input.GetMouseButtonDown(1))
        {
            LightBox.SetActive(!LightBox.activeInHierarchy); 
        }
    }
    //Mask 
    void MaskToggle()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mask.SetActive(!mask.activeInHierarchy);
            
        }
        else if (mask.activeInHierarchy)
        {
            this.gameObject.layer = 0;
            this.gameObject.name = "NotPlayer";
            MaskTimer += 1; 
        }
        else
        {
            this.gameObject.layer = 6;
            this.gameObject.name = "MainPlayer";
        }
        if(MaskTimer >= 200)
        {
            mask.SetActive(!mask.activeInHierarchy);
            MaskTimer = 0; 
        }
        
    }
    /// <summary>
    /// -------------------------------------------------------------------------------------
    /// All heaing and damage methods are below. 
    /// </summary>
    void TakeDamage(int Damage)
    {
        TotalHealth -= Damage; 
    }

    //Probably won't ever heal in this game, but nice to have the method
    void HealDamage(int Heal)
    {
        TotalHealth += Heal;
    }

    /// <summary>
    /// -------------------------------------------------------------------------------------
    /// Collision detection and operation is below 
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            TakeDamage(30);
        }
    }
    //On trail trigger
    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag.Equals("Trigger"))
        {
            Ontrail = false;
            NotOnTrail();
            if (Ontrail == false && OntrailTimer == 0)
            {
                TotalHealth = 0;
            }
        }
    }
    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag.Equals("Trigger"))
        {
            Ontrail = true;
            OntrailTimer += 1;
        }
    }
    private void NotOnTrail()
    {
        while (Ontrail = false) {
            OntrailTimer -= 1;
        }

    }
    /// <summary>
    /// Bloodscreens that display a different screen at each interval 
    /// </summary>
    void DisplayBloodScreen()
    {
        if(TotalHealth <= 80)
        {
            Screen1.SetActive(true); 
        }
        if(TotalHealth <= 50)
        {
            Screen2.SetActive(true);
        }
        if(TotalHealth <= 30)
        {
            Screen3.SetActive(true);
        }
        if(TotalHealth <= 0)
        {
            Screen4.SetActive(true);
            Death(); 
        }
    }
    /// <summary>
    /// Death. This will return a bool to the gamemanager to restart the level. 
    /// </summary>
    public bool Death()
    {
        if (TotalHealth <= 0)
        {
            Debug.Log("Death Works in Beh");
            DeathSound.Play(); 
            return true;
        }
        else
        {
            return false;
        }
    }
}
