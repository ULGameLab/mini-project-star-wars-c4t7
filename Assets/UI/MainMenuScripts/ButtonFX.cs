using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource MenuFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;


    public void hoverSoundPlay()
    {
        MenuFx.PlayOneShot(hoverFx);
    }
    public void clikSoundPlay()
    {
        MenuFx.PlayOneShot(clickFx);
    }
}
