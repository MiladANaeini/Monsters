using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BrighnessSlider : MonoBehaviour
{
    public Slider brighnessSlider;
    public PostProcessProfile brighness;
    public PostProcessLayer layer;

    AutoExposure exposure;
    void Start()
    {
        brighness.TryGetSettings(out exposure);
        AdjustBrighness(brighnessSlider.value);
    }

    public void AdjustBrighness(float value)
    {
        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 0.5f;
        }
    }
}
