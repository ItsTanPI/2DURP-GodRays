using System;
using UnityEngine;
using UnityEngine.Rendering;

// GodRaysVolume.cs
// This script handles the volume settings for the God Rays post-processing effect in the Unity scene.
// It allows for customization of light exposure, density, decay, and other related settings.
//
// Project Repository: https://github.com/ItsTanPI/2DURP-GodRays
// My itch.io Profile: https://tan-pi.itch.io


namespace GodRays2D.Runtime.GodRays
{
    [Serializable]
    [VolumeComponentMenu("GodRays")]
    public class GodRaysVolume : VolumeComponent
    {
        
        [Header("Light")]
        public ClampedFloatParameter Exposure = new(0f, 0f, 5f);
        public ClampedFloatParameter Density = new(64f, 16f, 256f);
        public ClampedFloatParameter Weight = new(0.3f, 0.05f, 5f);
        public ClampedFloatParameter Decay = new(0.95f, 0.05f, 1f);

        [Header("Perfomance")]
        public ClampedIntParameter DownscaleFactor = new(2, 1, 64);
        public ClampedIntParameter Samples = new(64, 0, 256);

        public ClampedIntParameter filterMode = new(1, 0, 2);

        public bool isActive() => Exposure != 0;
    }
}