using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThruster : MonoBehaviour
{
    Material mat;
    Renderer render;
    Color newCol, baseCol;
    readonly float maxEmission = 4.59479341999f;
    readonly float minEmission = 0f;

    ParticleSystem.EmissionModule eEngine;

    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
        mat = render.material;

        baseCol = mat.color;
        newCol = mat.color;

        eEngine = GetComponentInChildren<ParticleSystem>().emission;
        eEngine.rateOverTime = 0f;
        eEngine.rateOverDistance = 0f;
    }

    public void SetNewEmission(float newEmission)
    {
        if(newEmission >= 0.3f)
            eEngine.rateOverDistance = 0.05f;
        else
            eEngine.rateOverDistance = 0f;

        if (newEmission > maxEmission)
            newEmission = maxEmission;
        else if (newEmission < minEmission)
            newEmission = minEmission;

        newCol = baseCol * Mathf.LinearToGammaSpace(newEmission);

        mat.SetColor("_EmissionColor", newCol);
    }
}
