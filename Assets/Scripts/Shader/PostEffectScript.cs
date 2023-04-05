using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    [SerializeField]
    public float curvature;

    [SerializeField]
    public float vignette;

    public Material mat;

    /* After whole scene is rendered, source is the fully rendered scene that you would normally
     * be seeing. We are intercepting it and manipulating it before serving it up
     */
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Curvature",curvature);
        mat.SetFloat("_Vignette", vignette);

        Graphics.Blit(source, destination, mat);
    }

}
