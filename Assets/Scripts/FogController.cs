using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public enum FogDensity
{
    NONE,
    VERYLIGHT,
    LIGHT,
    NORMAL,
    HEAVY,
    VERYHEAVY,
    MAX
}

public class FogController : MonoBehaviour
{

    Volume volume;
    Fog fog;

    public FogDensity density = FogDensity.NORMAL;
    public float fogTransitionSpeed = 0.2f;

    float newDistance = 0;
    float previousDistance = 0;

    float minFogDinstance = 1;
    [SerializeField] float maxFogDistance = 30;

    List<float> fogDenseList = new List<float>();

    const int FOGDENSITYCOUNT = 7;

    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet<Fog>(out fog))
        {
            fog.meanFreePath.value = minFogDinstance;
        }

        MakeFogList();

        newDistance = fogDenseList[(int)FogDensity.NORMAL];
        previousDistance = fogDenseList[(int)FogDensity.NORMAL];
    }

    void MakeFogList()
    {
        fogDenseList.Clear();

        var fogDistanceSeparator = maxFogDistance / FOGDENSITYCOUNT;

        fogDenseList.Add(maxFogDistance);
        for (int i = 1; i < FOGDENSITYCOUNT - 1; i++)
        {
            fogDenseList.Add(maxFogDistance - (fogDistanceSeparator * i));
        }
        fogDenseList.Add(minFogDinstance);
    }

    private void Update()
    {
        //TestFog();

        if (volume.profile.TryGet<Fog>(out fog))
        {
            fog.meanFreePath.value = Mathf.LerpUnclamped(previousDistance, newDistance, time);
        }

        time += fogTransitionSpeed * Time.deltaTime;

        if (time > 1f) {
            time = 1f;
        }
    }

    public void SetFogDensity(FogDensity fogDensity)
    {
        time = 0f;
        density = fogDensity;
        previousDistance = fog.meanFreePath.value;
        newDistance = fogDenseList[(int)density];
    }

    void TestFog()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            SetFogDensity(FogDensity.MAX);
            Debug.Log("Set Fog Density to max");
        }
        if (Input.GetKey(KeyCode.W))
        {
            SetFogDensity(FogDensity.VERYHEAVY);
            Debug.Log("Set Fog Density to very heavy");
        }
        if (Input.GetKey(KeyCode.E))
        {
            SetFogDensity(FogDensity.HEAVY);
            Debug.Log("Set Fog Density to heavy");
        }
        if (Input.GetKey(KeyCode.R))
        {
            SetFogDensity(FogDensity.NORMAL);
            Debug.Log("Set Fog Density to normal");
        }
        if (Input.GetKey(KeyCode.T))
        {
            SetFogDensity(FogDensity.LIGHT);
            Debug.Log("Set Fog Density to light");
        }
        if (Input.GetKey(KeyCode.Y))
        {
            SetFogDensity(FogDensity.VERYLIGHT);
            Debug.Log("Set Fog Density to very light");
        }
        if (Input.GetKey(KeyCode.U))
        {
            SetFogDensity(FogDensity.NONE);
            Debug.Log("Set Fog Density to none");
        }
    }

}
