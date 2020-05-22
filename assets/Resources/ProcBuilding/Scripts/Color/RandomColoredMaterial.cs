using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColoredMaterial : MonoBehaviour
{
    public MeshRenderer[] renderers;

    private void Start()
    {
        //Color newColor = Random.ColorHSV();
        //Color newColor = Random.ColorHSV(0f, .5f);
        Color newColor = ColorStorage.wallColor;
        //ApplyMaterial(newColor, 0);
        ApplyMaterialFromGenerator(ProGen.material);
    }

    void ApplyMaterialFromGenerator(Material material)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material = material;
        }
    }

    void ApplyMaterial(Color color, int targetMaterialIndex)
    {
        Material generatedMaterial = GenerateMaterial(color);
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material = generatedMaterial;
        }
    }

    public static Material GenerateMaterial(Color color)
    {
        Material generatedMaterial = new Material(Shader.Find("Standard"));
        generatedMaterial.SetColor("_Color", color);
        return generatedMaterial;
    }
}
