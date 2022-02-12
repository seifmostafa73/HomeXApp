using System;
using UnityEngine;

namespace LeTai.TrueShadow
{
public enum BlendMode
{
    Normal,
    Addictive,
    Multiply,
}

public static class BlendModeExtensions
{
    internal static Material materialAddictive;
    internal static Material materialMultiply;

    public static Material GetMaterial(this BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Normal:
                return null; // should use graphic.materialForRendering
            case BlendMode.Addictive:
                if (!materialAddictive) materialAddictive = new Material(Shader.Find("UI/TrueShadow-Addictive"));
                return materialAddictive;
            case BlendMode.Multiply:
                if (!materialMultiply) materialMultiply = new Material(Shader.Find("UI/TrueShadow-Multiply"));
                return materialMultiply;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
}
