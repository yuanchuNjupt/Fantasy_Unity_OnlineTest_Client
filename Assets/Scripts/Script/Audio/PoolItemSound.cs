using System;
using System.Collections;
using System.Collections.Generic;
using ARPG.Assets;
using Script.Unilts;
using UnityEngine;



public enum SoundType
{
    ATK,
    HIT,
    BLOCK,
    FOOT
    
    
}


public class PoolItemSound : PoolItemBase
{
    private AudioSource _audio;
    [SerializeField] private SoundType _type;
    [SerializeField] private AssetsSoundSO _soundAssets;
    

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }


    public override void Spawn()
    {
        PlaySound();
        StartRecycle();
    }

    private void PlaySound()
    {
        _audio.clip = _soundAssets.GetAudioClip(_type);
        _audio.Play();
    }

    private void StartRecycle()
    {
        TimerManager.MainInstance.TryGetOneTimer(0.3f , DisableSelf);
    }

    private void DisableSelf()
    {
        _audio.Stop();
        this.gameObject.SetActive(false);
    }
}
