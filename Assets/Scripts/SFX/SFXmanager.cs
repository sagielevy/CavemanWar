using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXmanager : MonoBehaviour
{
    [SerializeField] List<AudioSource> list_sources = new List<AudioSource>();

    int stepIndex = 0;
    enum SFX{
        step1,
        step2,
        hurt1,
        hurt2,
        die,
        shoot,
        flame,
        pickup,
    }

    public void playHurt()
    {
        list_sources[(int)SFX.hurt1].Play();
    }
    public void playPickup()
    {
        list_sources[(int)SFX.pickup].Play();
    }
    public void playDie()
    {
        list_sources[(int)SFX.die].Play();
    }
    public void playFlame()
    {
        list_sources[(int)SFX.flame].Play();
    }
    public void playShoot()
    {
        list_sources[(int)SFX.shoot].Play();
    }
    public void playStep()
    {
        list_sources[(int)SFX.step1 + stepIndex].Play();

        stepIndex += 1;
        if(stepIndex > 1) stepIndex = 0;
    }
}