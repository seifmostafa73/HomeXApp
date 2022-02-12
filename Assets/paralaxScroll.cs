using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class paralaxScroll : MonoBehaviour
{
    [SerializeField] ScrollRect PrimaryScroll = null;
    [SerializeField] ScrollRect PrimaryScrollBackground = null;
    [SerializeField] ScrollRect SecondaryScroll = null;

    [SerializeField] Image PrimaryBackgroundImage = null;
    [SerializeField] Image maskImage = null;

    [SerializeField] TMP_Text ShowingText = null;
    [SerializeField] TMP_Text HidingText = null;

    [SerializeField] [Range(0f, 1f)] float BackgroundFollowSpeed = 0.8f;
    [SerializeField] [Range(0f, 1f)] float SecondaryFollowSpeed = 0.8f;
    // Update is called once per frame
    void Start()
    {
        PrimaryScroll.onValueChanged.AddListener(ListenerMethod);
    }
    public void ListenerMethod(Vector2 value)
    {
        var PrimaryrectTransform = maskImage.GetComponent<RectTransform>();
        var BackGroundrectTransform = PrimaryBackgroundImage.GetComponent<RectTransform>();

            ShowingText.alpha = PrimaryScrollBackground.verticalNormalizedPosition <= 0.6f ? PrimaryScrollBackground.verticalNormalizedPosition : 1f;
            HidingText.alpha = PrimaryScrollBackground.verticalNormalizedPosition <= 0.5f ?  1f: (1-PrimaryScrollBackground.verticalNormalizedPosition);
        


            PrimaryScrollBackground.velocity = new Vector2(PrimaryScroll.velocity.x * BackgroundFollowSpeed, PrimaryScroll.velocity.y * BackgroundFollowSpeed);
        SecondaryScroll.velocity = new Vector2(PrimaryScroll.velocity.x* SecondaryFollowSpeed, PrimaryScroll.velocity.y* SecondaryFollowSpeed);

        PrimaryrectTransform.offsetMax = new Vector2(PrimaryrectTransform.offsetMax.x, BackGroundrectTransform.offsetMax.y);
    }

}
