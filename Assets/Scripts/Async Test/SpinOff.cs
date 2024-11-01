using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpinOff : MonoBehaviour
{
    public IEnumerator Spin()
    {
        float end = Time.time + 3f;  

        while(Time.time <= end) 
        {
            transform.Rotate(new Vector3(0, 0, 150) * Time.deltaTime);
            yield return null;
                
        }
    }
}
