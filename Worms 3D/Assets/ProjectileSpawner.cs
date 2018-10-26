﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*How to use this script:
 Setting up a projectile prefab object and 
 instantiating the prefabs as game objects. 
 It acesses the projectile control script and the youAreA().
 It passes in the arguments for the projectile type, position, direction, speed for the game object*/

public class ProjectileSpawner : MonoBehaviour {

    public UnityEngine.Object grenadePrefab;
    public UnityEngine.Object MissilePrefab;
    FloatingDisplay strengthMeterDisplay;
    TimeAndDisplayCountup strengthMeter;
    private float MaxGrenadeSpeed = 40;
    AimCameraControl ourAimCam;
    GameObject crosshairs;
    WormControl ourOwner;

    
    

    // Use this for initialization
    void Start () {

     ourOwner = gameObject.GetComponent<WormControl>();

    }

    // Update is called once per frame
    void Update() {
        if (ourOwner.isWormActive())
        {
            if (Input.GetKey(KeyCode.G))
            {
                if (strengthMeterDisplay)  // grenade strength being calculated
                {
                    strengthMeterDisplay.setDisplay(strengthMeter.relativePercentage().ToString());

                    if (strengthMeter.relative() > 1.0f) createGrenade();
                }
                else   // STart of launch grenade
                {
                    strengthMeterDisplay = gameObject.AddComponent<FloatingDisplay>();
                    strengthMeter = gameObject.AddComponent<TimeAndDisplayCountup>();
                    strengthMeter.setDuration(5.0f);
                    strengthMeter.startTimer();


                }

            }

            else
            {
                if (strengthMeterDisplay)
                {
                    createGrenade();

                }

            }


            if (Input.GetKey(KeyCode.M))
            {
                if (ourAimCam)
                {
                   ourAimCam.transform.Rotate(Vector3.up, Input.GetAxis("Horizontal"));
                   ourAimCam.transform.Rotate(transform.right, Input.GetAxis("Vertical"));
                    if (crosshairs)
                    {
                        crosshairs.transform.position = ourAimCam.transform.position + 50.0f * ourAimCam.transform.forward;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject newProjectileGO = (GameObject)Instantiate(grenadePrefab);
                        ProjectileControl newProjectileScript = newProjectileGO.GetComponent<ProjectileControl>();

                        newProjectileScript.youAreA(ProjectileControl.ProjectileType.Missile, ourAimCam.transform.position, ourAimCam.transform.forward, 15.0f, ourOwner);
                        DestroyAimCam();
                        ourOwner.setActive(false);
                    }
                }

                else
                {
                    GameObject cam = new GameObject();
                    cam.AddComponent<Camera>();
                    ourAimCam = cam.gameObject.AddComponent<AimCameraControl>();

                    ourAimCam.transform.position = transform.position + 2.0f * Vector3.up - 2.0f * transform.forward;
                    ourAimCam.transform.rotation = transform.rotation;

                    crosshairs = GameObject.CreatePrimitive(PrimitiveType.Sphere);


                     
                        }
                    

            }

            else  // M released (or not pressed)
            {
                if (ourAimCam)
                {
                    Destroy(ourAimCam.gameObject);
                    Destroy(crosshairs);
                }

            }
        }
        }

    private void DestroyAimCam()
    {
        Destroy(crosshairs);
        Destroy(ourAimCam.gameObject,2.0f);
    }

    private void createGrenade()
        {
            GameObject newProjectileGO = (GameObject)Instantiate(grenadePrefab);
            ProjectileControl newProjectileScript = newProjectileGO.GetComponent<ProjectileControl>();

            newProjectileScript.youAreA(ProjectileControl.ProjectileType.Grenade, transform.position  + 2*transform.forward+ 2*transform.up , (transform.forward + Vector3.up).normalized,
            MaxGrenadeSpeed * strengthMeter.relative(),ourOwner);

            Destroy(strengthMeter);

            strengthMeterDisplay.manuallyDestroy();
        }

    

}
