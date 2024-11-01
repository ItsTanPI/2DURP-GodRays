using System.Collections;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [SerializeField] SpinOff[] SpinOff;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Bomb());
        }
    }


    IEnumerator Bomb()
    {
        
        for (int i = 0; i < SpinOff.Length; i++) 
        {
            yield return StartCoroutine(SpinOff[i].Spin());
        }
        
    }
}
