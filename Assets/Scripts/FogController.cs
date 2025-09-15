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
    public static FogController instance;

    public FogDensity density = FogDensity.NORMAL;

    Volume volume;
    Fog fog;

    Gradient fogColorGradient = new Gradient();

    public float fogTransitionSpeed = 0.2f;

    float newDistance = 0;
    float previousDistance = 0;

    float minFogDinstance = 1;
    [SerializeField] float maxFogDistance = 30;

    List<float> fogDenseList = new List<float>();

    float time = 0f;

    const float MAXREDCOLORAMOUNT = 0.4f; // How much R in RGB the fog color can be in float percentage. 0~255 == 0.0f~1.0f

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet<Fog>(out fog))
        {
            fog.meanFreePath.value = minFogDinstance;
        }

        MakeFogList();
        CreateColorGradient();

        newDistance = fogDenseList[(int)FogDensity.NORMAL];
        previousDistance = fogDenseList[(int)FogDensity.NORMAL];
    }

    // Divides the fog densities by the fogDensity enums. Spreads all densities evenly.
    void MakeFogList()
    {
        fogDenseList.Clear();

        var fogDensityCount = System.Enum.GetValues(typeof(FogDensity)).Length;

        var fogDistanceSeparator = maxFogDistance / fogDensityCount;

        for (int i = 0; i < fogDensityCount - 1; i++)
        {
            fogDenseList.Add(maxFogDistance - (fogDistanceSeparator * i));
        }
        fogDenseList.Add(minFogDinstance);
    }

    void CreateColorGradient()
    {
        // Blend color from white at 0% to red at 100%
        var colors = new GradientColorKey[2];
        colors[0] = new GradientColorKey(Color.white, 0.0f);
        colors[1] = new GradientColorKey(new Color(MAXREDCOLORAMOUNT,0,0), 1.0f);

        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        fogColorGradient.SetKeys(colors, alphas);
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

    /// <summary>
    /// Sets the fog density with the appropriate FogDensity enum level.
    /// </summary>
    /// <param name="fogDensity"></param>
    public void SetFogDensity(FogDensity fogDensity)
    {
        time = 0f;
        density = fogDensity;
        previousDistance = fog.meanFreePath.value;
        newDistance = fogDenseList[(int)density];
    }

    /// <summary>
    /// Sets the fog color to be between white (0.0f) and red (1.0f).
    /// </summary>
    /// <param name="evilFogColorAmount"></param>
    public void SetEvilFogColorAmount(float evilFogColorAmount)
    {
        if (evilFogColorAmount > 1f) { evilFogColorAmount = 1.0f; }
        if (evilFogColorAmount < 0f) { evilFogColorAmount = 0.0f; }

        var newFogColor = fogColorGradient.Evaluate(evilFogColorAmount);
        if (volume.profile.TryGet<Fog>(out fog))
        {
            fog.albedo.value = newFogColor;
        }
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


        if (Input.GetKey(KeyCode.A))
        {
            SetEvilFogColorAmount(0f);
            Debug.Log("Set Fog Color to white");
        }
        if (Input.GetKey(KeyCode.S))
        {
            SetEvilFogColorAmount(0.25f);
            Debug.Log("Set Fog Density to mostly white");
        }
        if (Input.GetKey(KeyCode.D))
        {
            SetEvilFogColorAmount(0.5f);
            Debug.Log("Set Fog Color to half-mix");
        }
        if (Input.GetKey(KeyCode.F))
        {
            SetEvilFogColorAmount(0.75f);
            Debug.Log("Set Fog Density to mostly red");
        }
        if (Input.GetKey(KeyCode.G))
        {
            SetEvilFogColorAmount(1f);
            Debug.Log("Set Fog Density to red");
        }
    }

   

}
