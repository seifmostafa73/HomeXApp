using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;


public class UnityAndroidJavaPlugins : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    const string ToastClassName = "android.widget.Toast"; //the name of the class that will be handed over th unity's Android Java Class to inherit funtions from

    const string IntentClassName = "android.content.Intent"; //The intent class which we will call and pass the action to be performed

    const string ActionSetAlarmString = "android.intent.action.SET_ALARM"; //the Set alarm action , which will be passed as an argumenmt to the intent class to search for the best suiting action


    const string EXTRA_HOUR = "android.intent.extra.alarm.HOUR";            //types of the arguments we will give to the
    const string EXTRA_MINUTES = "android.intent.extra.alarm.MINUTES";      //Set Alarm Action object
    const string EXTRA_MESSAGE = "android.intent.extra.alarm.MESSAGE";
    const string SKIP_NATIVEUI = "android.intent.extra.alarm.SKIP_UI";

    const string ACTION_CALL = "android.intent.action.CALL_PHONE";



    void AndroidToast(string toastText, int ToastDuration) 
    {
#if UNITY_ANDROID
        string text = toastText;
        int duration = ToastDuration;
        var context = GetUnityActivity(); //gets the permissions , and activity of the app
        AndroidJavaClass JavaClass = new AndroidJavaClass(ToastClassName);
        var ToastObject = JavaClass.CallStatic<AndroidJavaObject>("makeText", context, text, duration);// here we are creating our toast Object , which in java is by calling a static method called MakeText(context,text,duration) which returns a java object (toast) hence the <AndroidJavaObject>
        ToastObject.Call("show");
        ToastObject.Dispose();
#endif
    }

    public void AndroidAlarmIntentAction(string AlarmMessageString, int AlarmHour , int AlarmMin) 
    {
        
        AndroidJavaObject IntentObject = new AndroidJavaObject(IntentClassName, ActionSetAlarmString);
        IntentObject.Call<AndroidJavaObject>("putExtra", EXTRA_MESSAGE, AlarmMessageString);
        IntentObject.Call<AndroidJavaObject>("putExtra", EXTRA_HOUR, AlarmHour);
        IntentObject.Call<AndroidJavaObject>("putExtra", EXTRA_MINUTES, AlarmMin);
        IntentObject.Call<AndroidJavaObject>("putExtra", SKIP_NATIVEUI, true);


        GetUnityActivity().Call("startActivity", IntentObject);

    }


    public void OnClickTechsupport()
    {

        if (!Permission.HasUserAuthorizedPermission("android.permission.CALL_PHONE"))
        {
            Permission.RequestUserPermission("android.permission.CALL_PHONE");
        }
        print("Calling");
        CreatePhonecall();
    }

    public void CreatePhonecall()
    {
        string teleno = "tel:010076704531";
        var intentAJO = new AndroidJavaObject("android.content.Intent", ACTION_CALL);
        intentAJO.Call<AndroidJavaObject>("setData", "Uri.parse(" + teleno + ")");

        GetUnityActivity().Call("startActivity", intentAJO);
    }

#if UNITY_ANDROID && !UNITY_EDITOR

    static AndroidJavaObject GetUnityActivity()
    {
    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
}
#endif


#endif
    public void ToastButton3Sec(string toast) 
    {
        Toast.Instance.Show(toast, 3f, Toast.ToastColor.Blue);
    }

}
