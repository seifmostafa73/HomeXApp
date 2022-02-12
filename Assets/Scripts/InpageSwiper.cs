using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class InpageSwiper : MonoBehaviour
{
    [SerializeField] RectTransform PagesParent = null;
    [SerializeField] RectTransform[] ChildPages = null;
    [SerializeField] Canvas canvas = null;
    [SerializeField] QRchooseRoom QRCode = null;

    [SerializeField] int CurrentPage = 1;

    [SerializeField] TMPro.TMP_Text NextButtonText = null;
    [SerializeField] TMPro.TMP_Text BackButtonText = null;

    [SerializeField] Color ActiveColor = Color.blue;
    [SerializeField] Color DullColor = Color.grey;


    private void OnRectTransformDimensionsChange()
    {
        for (int i =0; i<ChildPages.Length;i++) {
            ChildPages[i].localPosition = new Vector3(canvas.GetComponent<RectTransform>().rect.width * i, 0, 0);
        }
    }

    public void NextButton()
    {
        if(CurrentPage < ChildPages.Length){
        var  newpostion = new Vector3(PagesParent.position.x - Screen.width, PagesParent.position.y);
        StartCoroutine(SmoothMove(PagesParent, PagesParent.position, newpostion, 0.2f));
        CurrentPage ++;
            if(BackButtonText!=null &&NextButtonText !=null)
            {
                NextButtonText.color = ActiveColor;
                BackButtonText.color = DullColor;
            }
        }
    }
    public void CancelButton()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, Screen.height), .5f).OnComplete(()=>
        {
            PagesParent.DOAnchorPos(new Vector2(0,0), .5f);
        });
        Clear();
    }

    public void BrindDown()
    {

        gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0), .5f);

    }
    public void Clear()
    {
        CurrentPage=1;
       if(QRCode != null) QRCode.QRCODESTRING = "";
    }

    public void BackButton()
    {
        if(CurrentPage > 1 ){
        var newpostion = new Vector3(PagesParent.position.x + Screen.width, PagesParent.position.y);
        StartCoroutine(SmoothMove(PagesParent, PagesParent.position, newpostion, 0.2f));
        CurrentPage --;
            if(BackButtonText!=null &&NextButtonText !=null)
            {
                BackButtonText.color = ActiveColor;
                NextButtonText.color = DullColor;
            }
        }
    }

    IEnumerator SmoothMove(RectTransform Transfrom,Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            Transfrom.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

    }
}