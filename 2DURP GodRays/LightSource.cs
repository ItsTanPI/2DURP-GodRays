using UnityEngine;
using UnityEngine.Rendering.Universal;


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
