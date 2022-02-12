using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class colorthemeController : MonoBehaviour
{

    [System.Serializable]
    public class palette
    {
        public Color FrontColor;
        public Color BackColor;
        public Color MiddleColor;
        public Color ExtraColor;
        public Color FontColor;
        palette(Color F,Color B,Color M,Color Ext,Color Font) 
        {
            FrontColor = F;
            BackColor = B;
            MiddleColor = M;
            ExtraColor = Ext;
            FontColor = Font;
        }
    }

    [SerializeField] UImanager uimanager = null;
    [SerializeField] palette BrightTheme = null;
    [SerializeField] palette DarkTheme = null;

    public static palette CurrentTheme = null;

    public List<Image> PrimaryObjects=null;
    public List<Image> SecondaryObjects = null;
    public List<Image> MidleObjects = null;
    public List<Image> ExtraObjects = null;
    public List<TMP_Text> TextElements = null;

    public void ChangeObjectsThemes()
    {
        int Themenumber = PlayerPrefs.GetInt("Theme", 0);

        if (Themenumber == 0) { CurrentTheme = BrightTheme; } else if (Themenumber == 1) { CurrentTheme = DarkTheme; }
        if(uimanager!=null) uimanager.SelectionColor = CurrentTheme.ExtraColor;

        foreach (Image PrimaryImage in PrimaryObjects)
        {
            StartCoroutine(Changecolor(PrimaryImage,CurrentTheme.FrontColor,1f));
        }

        foreach (Image SecondaryImage in SecondaryObjects)
        {

            StartCoroutine(Changecolor(SecondaryImage, CurrentTheme.BackColor, 1f));
        }

        foreach (Image MiddleImage in MidleObjects)
        {
           StartCoroutine( Changecolor(MiddleImage, CurrentTheme.MiddleColor, 1f));
        }

        foreach (Image ExtraImage in ExtraObjects)
        {
            StartCoroutine( Changecolor(ExtraImage, CurrentTheme.ExtraColor,1f));
        }

        foreach (TMP_Text Text in TextElements) 
        {
            StartCoroutine(Changecolor(Text,CurrentTheme.FontColor,1f));
        }
    }

    public IEnumerator Changecolor(TMP_Text Text, Color color, float time)
    {
        float ElapsedTime = 0.0f;
        float TotalTime = time;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            Text.color = Color.Lerp(Text.color, color, (ElapsedTime / TotalTime));
            yield return null;
        }
    }
    public IEnumerator Changecolor(Image image,Color color,float time)
    {
        float ElapsedTime = 0.0f;
        float TotalTime = time;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            image.color = Color.Lerp(image.color, color, (ElapsedTime / TotalTime));
            yield return null;
        }
    }

    public void checktoogle()
    {
        int Themenumber = PlayerPrefs.GetInt("Theme", 0);

        if (Themenumber == 0)
        {
            PlayerPrefs.SetInt("Theme", 1);
            Themenumber = 1;
        }
        else if (Themenumber == 1)
        {
            PlayerPrefs.SetInt("Theme", 0);
            Themenumber = 0;
        }
        this.ChangeObjectsThemes();
    }

    private void Awake()
    {
        ChangeObjectsThemes();
    }
}
