using UnityEngine;

public class PS1TextureController : MonoBehaviour
{
    public Vector2 tiling = new Vector2(1, 1); // Tiling per object
    public Vector2 offset = new Vector2(0, 0); // Offset per object
    public float pixelationAmount = 100; // Pixelation per object

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        UpdateMaterialProperties();
    }

    void UpdateMaterialProperties()
    {
        // Hakee MaterialPropertyBlock:in nykyiset arvot
        rend.GetPropertyBlock(propBlock);

        // Asettaa Tiling ja Offset arvot objektille
        propBlock.SetVector("_MainTex_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y));

        // Asettaa pikselöinnin määrän
        propBlock.SetFloat("_PixelationAmount", pixelationAmount);

        // Apply the updated values to the object
        rend.SetPropertyBlock(propBlock);
    }

    // Debug-päivitys (esimerkiksi Unityn Inspectorin kautta)
    void OnValidate()
    {
        if (rend != null)
        {
            UpdateMaterialProperties();
        }
    }
}
