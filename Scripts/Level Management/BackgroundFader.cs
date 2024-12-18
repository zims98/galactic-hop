using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFader : MonoBehaviour
{
    [HideInInspector] public bool fadeIn, fadeOut; // - Referenced in Player Script
    [SerializeField] float fadeSpeed = 1f;

    [SerializeField] Renderer[] renderers;

    void Update()
    {
        if (fadeOut)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Color objectColor = renderers[i].material.color;
                float alphaAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, alphaAmount);
                renderers[i].material.color = objectColor;

                if (objectColor.a <= 0)
                {
                    fadeOut = false;
                }
            }           
        }

        if (fadeIn)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Color objectColor = renderers[i].material.color;
                float alphaAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, alphaAmount);
                renderers[i].material.color = objectColor;

                if (objectColor.a >= 1)
                {
                    fadeIn = false;
                }
            }
        }

    }
}
