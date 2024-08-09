using System.Collections.Generic;
using UnityEngine;


namespace GodRays2D.Runtime.GodRays
{
    public class GodRaysCamera2D : MonoBehaviour
    {

        [SerializeField] Material GodRays;
        [SerializeField] int LightsOnScreen;


        [SerializeField] List<GameObject> lightSource;
        [SerializeField] List<Vector4> Positions;
        [SerializeField] List<float> Width;
        [SerializeField] List<float> Height;



        Vector4[] PositionArray = new Vector4[5];
        float[] WidthArray = new float[5];
        float[] HeightArray = new float[5];
        Vector4[] ColorArray = new Vector4[5];


        private void Update()
        {


            Positions.Clear();
            Width.Clear();
            Height.Clear();


            for (int i = 0; i < lightSource.Count; i++)
            {
                Vector4 pos = Camera.main.WorldToViewportPoint(lightSource[i].transform.position);
                Positions.Add(pos);
                HeighAndWidth(lightSource[i]);
            }

            for (int i = 0; i < lightSource.Count; i++)
            {
                LightSource LS = lightSource[i].GetComponent<LightSource>();

                PositionArray[i] = Positions[i];
                WidthArray[i] = Width[i];
                HeightArray[i] = Height[i];
                ColorArray[i] = LS.LightColor;
            }


            GodRays.SetFloat("_Count", lightSource.Count);
            if (Positions.Count > 0)
            {
                GodRays.SetVectorArray("_LightPosition", PositionArray);
                GodRays.SetFloatArray("_Width", WidthArray);
                GodRays.SetFloatArray("_Height", HeightArray);
                GodRays.SetVectorArray("_Color", ColorArray);
            }
        }

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

        public void Push(GameObject Source)
        {
            lightSource.Add(Source);
        }

        public void Pop(GameObject Source)
        {
            lightSource.Remove(Source);
        }

    }
}
