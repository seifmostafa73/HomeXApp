using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] SettingsMenu Settingsmenu = null;
    RectTransform SafeAreaPanel = null;

    Rect currentSafeAreaRect = new Rect();
    ScreenOrientation screenOrientation = ScreenOrientation.AutoRotation;

    void Start()
    {
        SafeAreaPanel = GetComponent<RectTransform>();

        currentSafeAreaRect = Screen.safeArea;
        screenOrientation = Screen.orientation;
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (SafeAreaPanel == null)
            return;

        Rect NewSafeArea = Screen.safeArea;

        Vector2 AnchoredMin = NewSafeArea.position;
        Vector2 AnchoredMax = NewSafeArea.position + NewSafeArea.size;

        AnchoredMin.x /= canvas.pixelRect.width;
        AnchoredMin.y /= canvas.pixelRect.height;

        AnchoredMax.x /= canvas.pixelRect.width;
        AnchoredMax.y /= canvas.pixelRect.height;

        SafeAreaPanel.anchorMin = AnchoredMin;
        SafeAreaPanel.anchorMax = AnchoredMax;

        currentSafeAreaRect = Screen.safeArea;
        screenOrientation = Screen.orientation;

    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
        if(Settingsmenu!=null) Settingsmenu.ResetPositions();
    }
}
