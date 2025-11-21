using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ARPG.Assets
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Create/Assets/Sound", order = 0)]
    public class AssetsSoundSO : ScriptableObject
    {
        [System.Serializable]
        public class Sounds
        {
            public SoundType SoundType;
            public AudioClip[] AudioClips;
        }
        
        
        [SerializeField] private List<Sounds> _configSounds = new List<Sounds>();

        public AudioClip GetAudioClip(SoundType soundType)
        {
            if(_configSounds == null) return null;
            switch (soundType)
            {
                case SoundType.ATK:
                    
                    return _configSounds[0].AudioClips[Random.Range(0, _configSounds[0].AudioClips.Length)];
                    
                
                case SoundType.HIT:
                    return _configSounds[1].AudioClips[Random.Range(0, _configSounds[1].AudioClips.Length)];

                    
                

                    
                case SoundType.BLOCK:
                    return _configSounds[2].AudioClips[Random.Range(0, _configSounds[2].AudioClips.Length)];
                
                case SoundType.FOOT:
                    return _configSounds[3].AudioClips[Random.Range(0, _configSounds[3].AudioClips.Length)];

               
            }
            return null;
        }
        
        
    }
}