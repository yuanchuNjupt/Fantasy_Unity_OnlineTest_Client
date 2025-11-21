using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Event
{
    public class AnimationEvent : MonoBehaviour
    {
        private void PlaySound(string soundname)
        {
            GamePoolManager.MainInstance.TryGetPoolItem(soundname, transform.position, Quaternion.identity);
        }
    }
}
