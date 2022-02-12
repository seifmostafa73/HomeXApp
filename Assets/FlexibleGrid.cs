using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlexibleGrid : MonoBehaviour
{
    public float WidthRatio = .5f;
    public float HeightRatio = .5f;
    public float XspacingRatio = 0.15f;
    public float YspacingRatio = 0.15f;
    public bool ResizeWidth = true;
    public bool ResizeHeight = true;
    public bool DynamicSpacing = false;
    // Start is called before the first frame update
    ScreenOrientation Orientation;
    void Start()
    {
        Orientation = Screen.orientation;
        ResizeGrid();
    }
    void OnRectTransformDimensionsChange() 
    {
        ResizeGrid();
    }

    private void ResizeGrid()
    {
        var ContentFitter = GetComponent<ContentSizeFitter>() ? GetComponent<ContentSizeFitter>() : null;
        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
        RectTransform ParentRect = transform.parent.GetComponent<RectTransform>();
        if (ContentFitter != null) { ContentFitter.enabled = false; }
        var NewWidthVlaue = ResizeWidth ? ParentRect.rect.width * WidthRatio : layout.cellSize.x;
        var NewHeightValue = ResizeHeight ? ParentRect.rect.height * HeightRatio : layout.cellSize.y;
        layout.cellSize = new Vector2(NewWidthVlaue, NewHeightValue);
        if (DynamicSpacing) layout.spacing = new Vector2(ParentRect.rect.width * XspacingRatio, ParentRect.rect.height * YspacingRatio);
        if (ContentFitter != null) { ContentFitter.enabled = true; }
    }
}
