using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffect : MonoBehaviour
{
    //reference
    public BoxCollider waterBox;
    public GameObject player;
    public Volume postProcess;
    public bool underwater;

    //skyboxes
    int skyboxIndex;
    public Material[] skyboxes; //0 = ocean, 1 = noon, 2 = night, 3 = evening
    //effects
    private DepthOfField pp_depth;
    private ColorAdjustments pp_color;
    private Vignette pp_vignette;
    private LensDistortion pp_lens;

    //effect parameters
    //vignette
    private float intensity_water = 0.25f;
    private float intensity_normal = 0;

    //depth of view
    private float focusLength_water = 2;
    private float focusLength_normal = 2.5f;

    //color adjustments
    public Color color_underwater;
    public Color color_normal;

    //lens distortion
    private float lens_normal = 0;
    private float lens_water = 0.425f;

    private void Start()
    {
        postProcess.profile.TryGet(out pp_depth);
        postProcess.profile.TryGet(out pp_color);
        postProcess.profile.TryGet(out pp_vignette);
        postProcess.profile.TryGet(out pp_lens);
    }

    private void Update()
    {
        if(waterBox != null && waterBox.bounds.Contains(gameObject.transform.position))
        {
            underwater = true;
        }
        else
        {
            underwater = false;
        }

        if(underwater)
        {
            //skyboxes
            if(skyboxIndex != 0)
            {
                skyboxIndex = 0;
                RenderSettings.skybox = skyboxes[skyboxIndex];
            }
            //post processing
            pp_vignette.intensity.value = intensity_water;
            pp_depth.focusDistance.value = focusLength_water;
            pp_color.colorFilter.value = color_underwater;
            pp_lens.intensity.value = lens_water;
        }
        else
        {
            //skyboxes
            if(skyboxIndex != 3)
            {
                skyboxIndex = 3;
                RenderSettings.skybox = skyboxes[skyboxIndex];
            }
            //post processing
            pp_vignette.intensity.value = intensity_normal;
            pp_depth.focusDistance.value = focusLength_normal;
            pp_color.colorFilter.value = color_normal;
            pp_lens.intensity.value = lens_normal;
        }
    }
}
