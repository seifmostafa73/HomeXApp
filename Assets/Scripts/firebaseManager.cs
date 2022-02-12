using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Google;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Auth;
using TMPro;
public class firebaseManager : MonoBehaviour
{
    private UnityAndroidJavaPlugins Javaplugins = null;
    struct UserInfo 
    {
        public String Name;
        public Uri ProfileImage;
        public UserInfo(String name, Uri ImageURL)
        {
            Name = name;
            ProfileImage = ImageURL;
        }
    }
    [SerializeField] UserInfo User = new UserInfo("anonymous", new System.Uri("https://www.searchpng.com/wp-content/uploads/2019/02/Profile-PNG-Icon-1024x1024.png") );
    [SerializeField] TMP_Text NameText = null;
    [SerializeField] Image ImageUI = null;

    private DatabaseReference DBreference;
    [SerializeField] TMP_InputField ChatMessageInputfiled = null;

    [SerializeField] GameObject MessagePrefab = null;
    [SerializeField] Transform MessageParent = null;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        Javaplugins = GetComponent<UnityAndroidJavaPlugins>();

        CheckFirebaseDependencies();
#endif
    }

    private void Start()
    {

        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        //Start DB Listener
        ListenForNewMessages((M)=> { StartCoroutine(InstantiateMessageUI(M)); },Debug.LogError);


    }

    IEnumerator InstantiateMessageUI(MessageClass msg)
    {
        GameObject newMessage = Instantiate(MessagePrefab, MessageParent);
        var UserImg = newMessage.transform.Find("Image").Find("UserImg").GetComponent<Image>();
        newMessage.transform.Find("UserName").GetComponent<TMP_Text>().text = msg.User_Name;
        newMessage.transform.Find("Note").GetComponent<TMP_Text>().text = msg.Message_Content;

        UnityWebRequest Request = UnityWebRequestTexture.GetTexture(msg.UserImage);

        yield return Request.SendWebRequest();
        DownloadHandlerTexture DownloadedImage = Request.downloadHandler as DownloadHandlerTexture;
        yield return new WaitUntil(() => Request.downloadHandler.isDone);

        UserImg.color = Color.white;
        UserImg.sprite = Sprite.Create(DownloadedImage.texture, new Rect(0, 0, DownloadedImage.texture.width, DownloadedImage.texture.height), new Vector2(0, 0));
        UserImg.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        UserImg.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

    }



    #region Firebase Functions Defintion

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
                SignInWithGoogle();

            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
#endregion

#region Google Login

    const string ClientSecret = "CqLpE77DdCTYgsuA_oh9BfOi";
    const string webClientId = "93024184755-6h8uq43vcs20j11jcu91rat99ntfp6gd.apps.googleusercontent.com";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;


    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        if (configuration == null)
        {
            configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        }
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            AddToInformation("Google ID Token = " + task.Result.IdToken);
            AddToInformation("profile image = " + task.Result.ImageUrl);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
            User = new UserInfo(task.Result.DisplayName, task.Result.ImageUrl);
        }
    }
    IEnumerator SetImageAndText(String Name, Uri ImageURI) 
    {
        UnityWebRequest Request = UnityWebRequest.Get(ImageURI);
        yield return Request.SendWebRequest();
        DownloadHandlerTexture DownloadedImage = Request.downloadHandler as DownloadHandlerTexture;
        NameText.text = Name;
        ImageUI.sprite = Sprite.Create(DownloadedImage.texture, new Rect(0, 0, DownloadedImage.texture.width, DownloadedImage.texture.height), new Vector2(0, 0));
        Debug.Log(User.Name);
    }

    //Called at start

    public void CallCoroutine()
    {
        StartCoroutine(SetImageAndText());
    }
    IEnumerator SetImageAndText()
    {

            if (User.Name == "" || User.ProfileImage == null || UnityWebRequest.Get(User.ProfileImage) == null)
               yield return new WaitForSeconds(1.5f);

            Debug.LogError("Passed if statement" + User.ProfileImage);
            NameText.text = User.Name;
            UnityWebRequest Request = UnityWebRequestTexture.GetTexture(User.ProfileImage);

            yield return Request.SendWebRequest();
            DownloadHandlerTexture DownloadedImage = Request.downloadHandler as DownloadHandlerTexture;
            yield return new WaitUntil(() => Request.downloadHandler.isDone);

            ImageUI.color = Color.white;
            ImageUI.sprite = Sprite.Create(DownloadedImage.texture, new Rect(0, 0, DownloadedImage.texture.width, DownloadedImage.texture.height), new Vector2(0, 0));
            ImageUI.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
            ImageUI.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception; 
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
                StartCoroutine(SetImageAndText());


#if UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
#endif

            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str) {
        Debug.Log(str);
    }
#endregion

#region Firebase Database messages
    private void SendMessagetoDB(MessageClass MessageObject, Action Callback,Action<AggregateException> Fallback)
    {
        if (DBreference == null) 
        {
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            ListenForNewMessages((M)=> { StartCoroutine(InstantiateMessageUI(M)); },Debug.LogError);
        }
        DBreference.Child("messages").Push().SetRawJsonValueAsync(JsonUtility.ToJson(MessageObject)).ContinueWith(
            task=> {
                if (task.IsCanceled || task.IsFaulted) { Fallback(task.Exception); }
                else { Callback(); }
            });
    }

    public void ListenForNewMessages(Action<MessageClass> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson(args.Snapshot.GetRawJsonValue(), typeof(MessageClass)) as MessageClass);
            
        }
        Debug.Log("Listening To database");
        DBreference.Child("messages").ChildAdded += CurrentListener;
    }

    public void SendChatMessage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
       if(User.Name == "anonymous") SignInWithGoogle();
#endif

        //string IMGURL = User.ProfileImage.ToString() != null ? User.ProfileImage.ToString() : "https://cdn.business2community.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640.png";
        MessageClass message = new MessageClass(ChatMessageInputfiled.text,PlayerPrefs.GetString("Name", "null"), User.ProfileImage.ToString(), User.Name);
        SendMessagetoDB(message,
            ()=>Debug.Log(ChatMessageInputfiled.text+"Sent"),
            (e)=>Debug.Log(e.InnerExceptions)
            );
    }

    

#endregion

}

[Serializable]
public class MessageClass 
{
    public string Message_Content = null;
    public string Message_Channel = null;
    public string UserImage = null;
    public string User_Name = null;
    public MessageClass(string Message ,string Channel, string UserImg , string Username)
    {
        this.Message_Content = Message;
        this.Message_Channel = Channel;
        this.UserImage = UserImg;
        this.User_Name = Username;
    }
}

