using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Lean;
using Lean.Gui;

public class VideoScript : MonoBehaviour
{

    public VideoPlayer Video = null;
    RawImage Videoraw = null;
    [SerializeField] Image TButtonimg = null;

    public void Start()
    {
        Videoraw = gameObject.GetComponent<RawImage>();
        StartCoroutine(Waitforvideo());
    }
    IEnumerator Waitforvideo()
    {
        Video.Prepare();
        while (!Video.isPrepared) {
            yield return new WaitForSeconds(1f);
        }
        Videoraw.texture = Video.texture;
        Video.Pause();
    }
    public void Videotoggle() {

        Color buttoncolor = TButtonimg.color;

        if (Video.isPlaying) {
            Video.Pause();
            buttoncolor = new Color(buttoncolor.r,buttoncolor.g,buttoncolor.b,1f);
            TButtonimg.color = buttoncolor;
        }
        else if(Video.isPaused) {
            Video.Play();
            buttoncolor = new Color(buttoncolor.r, buttoncolor.g, buttoncolor.b, 0f);
            TButtonimg.color = buttoncolor;
        }
    }

}
