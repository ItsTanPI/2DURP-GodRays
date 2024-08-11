using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using GodRays2D.Runtime.GodRays;
using UnityEngine.UI;

// PostProcessingEdit.cs
// This script serves as an example of how to modify post-processing settings in Unity through code.
// It allows for real-time adjustments of various post-processing parameters, such as exposure, 
// density, weight, and decay of the God Rays effect in a 2D scene.
// 
// The script can be attached to any GameObject in the scene, and it will modify the post-processing 
// profile attached to the global volume.
//
// Project Repository: https://github.com/ItsTanPI/2DURP-GodRays
// My itch.io Profile: https://tan-pi.itch.io


public class PostProcessEdit : MonoBehaviour
{
    Volume volume;
    [SerializeField] VolumeProfile MainProfile;
    GodRaysVolume godRaysVolume;
    public GameObject UI;


    [Header("Sliders")]
    public Slider Exposure;
    public Slider Density;
    public Slider Weight;
    public Slider Decay;
    public Slider DownscaleFactor;
    public Slider Samples;
    public Slider FilterMode;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile = MainProfile;
        volume.profile.TryGet<GodRaysVolume>(out godRaysVolume);

        Exposure.value = 0.2f;
        Density.value = 90f;
        Weight.value = 0.51f;
        Decay.value = 0.96f;
        DownscaleFactor.value = 2f;
        Samples.value = 111f;
        FilterMode.value = 1f;

        SetExposure();
        SetDensity();
        SetWeight();
        SetDecay();
        SetDownscaleFactor();
        SetSamples();
        SetFilterMode();
    }

    [System.Obsolete]
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.U)) 
        {
            UI.active = !UI.active;
        }
    }

    public void SetExposure()
    {
        godRaysVolume.Exposure.value = Exposure.value;
        Exposure.GetComponentInChildren<Text>().text = Exposure.value.ToString();
    }

    public void SetDensity()
    {
        godRaysVolume.Density.value = Density.value;
        Density.GetComponentInChildren<Text>().text = Density.value.ToString();
    }

    public void SetWeight()
    {
        godRaysVolume.Weight.value = Weight.value;
        Weight.GetComponentInChildren<Text>().text = Weight.value.ToString();
    }

    public void SetDecay()
    {
        godRaysVolume.Decay.value = Decay.value;
        Decay.GetComponentInChildren<Text>().text = Decay.value.ToString();
    }

    public void SetDownscaleFactor()
    {
        godRaysVolume.DownscaleFactor.value = (int)DownscaleFactor.value;
        DownscaleFactor.GetComponentInChildren<Text>().text = DownscaleFactor.value.ToString();
    }

    public void SetSamples()
    {
        godRaysVolume.Samples.value = (int)Samples.value;
        Samples.GetComponentInChildren<Text>().text = Samples.value.ToString();
    }

    public void SetFilterMode()
    {
        godRaysVolume.filterMode.value = (int)FilterMode.value;
        FilterMode.GetComponentInChildren<Text>().text = FilterMode.value.ToString();
    }
}
