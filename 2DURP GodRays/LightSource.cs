using UnityEngine;
using UnityEngine.Rendering.Universal;


// LightSource.cs
// This script is used to define a light source in the Unity scene that will interact with the God Rays effect.
// The light source can be customized with different shapes and intensities.
//
// Project Repository: https://github.com/ItsTanPI/2DURP-GodRays
// My itch.io Profile: https://tan-pi.itch.io


namespace GodRays2D.Runtime.GodRays
{
    public class LightSource : MonoBehaviour
    {
        [Header("LightSetting")]

        public Color LightColor;

        [SerializeField] bool MainLight;

        GodRaysCamera2D cam;
        public Light2D Light;
        


        private void Update()
        {
            if (MainLight && Light != null) 
            {
                Light.color = LightColor;
            }
        }
        private void Start()
        {
            cam = Camera.main.GetComponent<GodRaysCamera2D>();
            Light = GetComponent<Light2D>();

        }

        void OnBecameVisible()
        {
            cam.Push(this.gameObject);
        }


        void OnBecameInvisible()
        {
            cam.Pop(this.gameObject);
        }
    }
}
