using System;
using Light;
using Oxygen_Line;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Activator = Activators.Activator;

public class Indicator : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    [SerializeField] private MeshRenderer indicatorMesh;
    [SerializeField] private Material LightOff;
    [SerializeField] private Material LightOn;
    private MaterialPropertyBlock mtb;
    private static readonly int Color = Shader.PropertyToID("_BaseColor");
    private static readonly int GlowColor = Shader.PropertyToID("Color_5e925b7b40b34f5498475f67824c9887");
    [HideInInspector] public LightColor lightColor;
    [SerializeField] private UnityEngine.Light pointLight;
    [SerializeField] private float indicatorOffIntensity;
    [SerializeField] private float indicatorOnIntensity;

    [Range(0.15f, 10)] public float flashesPerSecond;

    [HideInInspector] public bool IsTurnedOn;

    public void SetLight(bool lampOn, LightColor lightColor)
    {
        indicatorMesh.material = lampOn ? LightOn : LightOff;
        if (lampOn)
        {
            mtb = new MaterialPropertyBlock();
            mtb.SetColor(GlowColor,
                lightColor.onColor);
            indicatorMesh.SetPropertyBlock(mtb);
            SetPointLight(lightColor.onColor, indicatorOnIntensity);
            IsTurnedOn = true;
        }
        else
        {
            
            mtb = new MaterialPropertyBlock();
            mtb.SetColor(Color, lightColor.offColor);
            indicatorMesh.SetPropertyBlock(mtb);
            SetPointLight(lightColor.offColor, indicatorOffIntensity);
            IsTurnedOn = false;
        }
    }

    public void SetPointLight(Color color, float intensity)
    {
        if(pointLight == null)
            return;
        pointLight.color = color;
        pointLight.intensity = intensity;
    }
}
