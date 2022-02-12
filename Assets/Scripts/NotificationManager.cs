using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
#endif
public class NotificationManager : MonoBehaviour
{
    public void CreateNotifLOWchannel() {

#if UNITY_ANDROID
        var NotifChannel1 = new AndroidNotificationChannel()
        {
            Id = "HomeXchannellow",
            Name = "HomeXChannel1",
            Importance = Importance.Low,
            Description = "trial channel with low piority",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(NotifChannel1);
#endif
#if UNITY_IOS
       /* var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, minutes, seconds),
            Repeats = false
        };*/

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "_notification_01",
            Title = "HomeX",
            Body = "Scheduled at: " + System.DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "Just a test notification",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            //Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }
    public void CreateNotifMEDchannel()
    {
#if UNITY_ANDROID
        var NotifChannel2 = new AndroidNotificationChannel()
        {
            Id = "HomeXchannelMed",
            Name = "HomeXChannel2",
            Importance = Importance.Default,
            Description = "trial channel with defult piority",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(NotifChannel2);
#endif
#if UNITY_IOS
#endif
    }
    public void CreateNotifHIGHchannel()
    {
#if UNITY_ANDROID
        var NotifChannel3 = new AndroidNotificationChannel()
        {
            Id = "HomeXchannelHigh",
            Name = "HomeXChannel3",
            Importance = Importance.High,
            Description = "trial channel with high piority",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(NotifChannel3);
#endif

#if UNITY_IOS

#endif
    }


#if UNITY_ANDROID
        public void SendNotification(string title,string text,string LIconname,int hours,int min,int sec,string channelid)
        {
        AndroidNotification HomexSendnotif = new AndroidNotification();
        HomexSendnotif.Title = title;
        HomexSendnotif.Text = text;
        HomexSendnotif.FireTime =System.DateTime.Now.AddHours(hours).AddMinutes(min).AddSeconds(sec);
        HomexSendnotif.LargeIcon = LIconname;
        HomexSendnotif.SmallIcon = "Homex_SIcon";

        AndroidNotificationCenter.SendNotification(HomexSendnotif,channelid);
        }
#endif

#if UNITY_IOS
    public void SendNotification(string title, string text, string LIconname, int hours, int min, int sec, string channelid)
    {
        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "_notification_01",
            Title = "title",
            Body = "Scheduled at: " + System.DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "text",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
        };
            iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif

}
