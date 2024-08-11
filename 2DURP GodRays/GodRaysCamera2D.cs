using System.Collections.Generic;
using UnityEngine;

// GodRaysCamera2D.cs
// This script manages the God Rays effect in a Unity URP 2D environment by interacting with a list of light sources in the scene.
// It calculates the positions, width, and height of each light source and passes this data to the God Rays shader for rendering.
// The script also handles adding and removing light sources dynamically during runtime.
//
// Project Repository: https://github.com/ItsTanPI/2DURP-GodRays
// My itch.io Profile: https://tan-pi.itch.io

namespace GodRays2D.Runtime.GodRays
{
    public class GodRaysCamera2D : MonoBehaviour
    {
        [SerializeField] Material GodRays;  // Material for the God Rays effect
        [SerializeField] int LightsOnScreen; // Number of lights that can be rendered on the screen

        [SerializeField] List<GameObject> lightSource;  // List of light source objects in the scene
        [SerializeField] List<Vector4> Positions;  // List of positions for each light source
        [SerializeField] List<float> Width;  // List of width values for each light source
        [SerializeField] List<float> Height;  // List of height values for each light source

        Vector4[] PositionArray = new Vector4[5];  // Array for storing light positions to be sent to the shader
        float[] WidthArray = new float[5];  // Array for storing width values to be sent to the shader
        float[] HeightArray = new float[5];  // Array for storing height values to be sent to the shader
        Vector4[] ColorArray = new Vector4[5];  // Array for storing color values to be sent to the shader

        private void Update()
        {
            // Clear the current position, width, and height data
            Positions.Clear();
            Width.Clear();
            Height.Clear();

            // Iterate over each light source, calculate its viewport position, and update the width and height
            for (int i = 0; i < lightSource.Count; i++)
            {
                Vector4 pos = Camera.main.WorldToViewportPoint(lightSource[i].transform.position);
                Positions.Add(pos);
                HeighAndWidth(lightSource[i]);
            }

            // Populate the arrays with the light source data
            for (int i = 0; i < lightSource.Count; i++)
            {
                LightSource LS = lightSource[i].GetComponent<LightSource>();

                PositionArray[i] = Positions[i];
                WidthArray[i] = Width[i];
                HeightArray[i] = Height[i];
                ColorArray[i] = LS.LightColor;
            }

            // Set shader parameters with the light source data
            GodRays.SetFloat("_Count", lightSource.Count);
            if (Positions.Count > 0)
            {
                GodRays.SetVectorArray("_LightPosition", PositionArray);
                GodRays.SetFloatArray("_Width", WidthArray);
                GodRays.SetFloatArray("_Height", HeightArray);
                GodRays.SetVectorArray("_Color", ColorArray);
            }
        }

        // Calculate the width and height of the light source in viewport space
        void HeighAndWidth(GameObject LS)
        {
            Camera camera = Camera.main;
            SpriteRenderer sp = LS.GetComponent<SpriteRenderer>();
            Bounds bounds = sp.bounds;

            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
            corners[3] = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);

            Vector3[] screenCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                screenCorners[i] = camera.WorldToViewportPoint(corners[i]);
            }

            Vector3 minScreenPoint = screenCorners[0];
            Vector3 maxScreenPoint = screenCorners[0];

            for (int i = 1; i < 4; i++)
            {
                minScreenPoint = Vector3.Min(minScreenPoint, screenCorners[i]);
                maxScreenPoint = Vector3.Max(maxScreenPoint, screenCorners[i]);
            }

            float width = maxScreenPoint.x - minScreenPoint.x;
            float height = maxScreenPoint.y - minScreenPoint.y;

            Width.Add(width / 2);
            Height.Add(height / 2);
        }

        // Add a light source to the list
        public void Push(GameObject Source)
        {
            lightSource.Add(Source);
        }

        // Remove a light source from the list
        public void Pop(GameObject Source)
        {
            lightSource.Remove(Source);
        }
    }
}
