using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.TrueShadow
{
public readonly struct ShadowRenderingRequest
{
    const int DIMENSIONS_HASH_STEP = 1;

    public readonly TrueShadow shadow;
    public readonly float      shadowSize;
    public readonly Vector2    shadowOffset;
    public readonly Vector2    dimensions;
    public readonly Rect       rect;

    readonly int hash;

    public ShadowRenderingRequest(TrueShadow shadow)
    {
        this.shadow = shadow;

        var casterScale = 1f;
        try
        {
            casterScale = shadow.Graphic.canvas.scaleFactor;
        }
        catch (NullReferenceException) { }

        shadowSize   = shadow.Size * casterScale;
        shadowOffset = shadow.Offset.Rotate(-shadow.RectTransform.eulerAngles.z) * casterScale;
        rect         = shadow.RectTransform.rect;
        dimensions   = rect.size * casterScale;

        // Tiled type cannot be batched by similar size
        int dimensionHash = shadow.Graphic is Image image && image.type == Image.Type.Tiled
                                ? dimensions.GetHashCode()
                                : HashUtils.CombineHashCodes(
                                    Mathf.CeilToInt(dimensions.x / DIMENSIONS_HASH_STEP) * DIMENSIONS_HASH_STEP,
                                    Mathf.CeilToInt(dimensions.y / DIMENSIONS_HASH_STEP) * DIMENSIONS_HASH_STEP
                                );

        hash = HashUtils.CombineHashCodes(
            Mathf.CeilToInt(shadowSize * 100),
            dimensionHash,
            shadow.ContentHash
        );
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return hash;
    }
}
}
