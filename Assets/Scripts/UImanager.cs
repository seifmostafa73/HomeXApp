using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Lean.Gui;
using System;
using System.IO;
using DG.Tweening;
using System.Linq;

public class UImanager : MonoBehaviour
{
    #region Types
    byte roomsnumber;//i check hom many room saved in the player prefab folder and set the number of rooms to that value 

    bool mainactivestate = true;
    bool receentActivestate = false;
    bool MyHomeActivestate = false;
    bool Contactactivestate = false;

    public byte CurrentMenu = 0;
    enum menus
    {
        MainMenu = 0,
        RecentMenu,
        MyHomeMenu,
        ContactUsMenu
    };
    public enum Products
    {
        Mini = 0,
        Classic,
        pro,
    };

 

    [System.Serializable]
    public struct DeviceData
    {
        public int RoomNumber;
        public string DeviceName;
        public int Type;
        public string ProductName;
        public bool[] States;
        public string[] ToggleNames;
        public bool[] Favourtie;
        public DeviceData(string DName, int DType, bool[] DState,bool[] Fav, string[] TNames, int Roomnumber, string DProductName)
        {
            RoomNumber = Roomnumber;
            DeviceName = DName;
            Type = DType;
            States = DState;
            Favourtie = Fav;
            ToggleNames = TNames;
            ProductName = DProductName;
        }
    }

#endregion

    [Header("Perfromance")]
    [Range(10, 60)] public int FrameRate = 35;
    [SerializeField] bool ScreenAutoDimming = false;

    [Space(10)]

    [Header("Trasnforms")]
    [SerializeField] Transform RoomParent = null;
    [SerializeField] Transform DevicesParent = null;
    [SerializeField] Transform FavouritesParent = null;
    [Space(5)]
    [SerializeField] GameObject ScenarioPage = null;
    [SerializeField] GameObject RemotePage = null;
    [Space(10)]

    [Header("Prefabs")]
    public GameObject newDevice = null;
    public GameObject[] MenusObj = null;
    [Space(5)]
    [SerializeField] GameObject GeneralRoomTemplete = null;
    [SerializeField] GameObject FavouriteDevicePrefab = null;
    [SerializeField] GameObject newRoom = null;
    [SerializeField] GameObject MyRooms = null;
    [Space(10)]

    [Header("Device Button Data")]
    [SerializeField] Image IsThereAnyDevice = null;
    public List<DeviceData> AllDeviceData = new List<DeviceData>();

    [SerializeField] Sprite[] DeviceIcons = null;
 
    [Header("Lean")]
    public Image[] TabButtons = null;
    [SerializeField] public Color SelectionColor = new Color();
    [SerializeField] Color NormalColor = new Color();
    public TextMeshProUGUI popupText = null;
    [Space(10)]

    [Header("Theme Changer")]
    public Sprite[] BackGrounds = null;
    public Image BKGimg = null;
    [SerializeField] Canvas canvas = null;

    //wifi config Input SSID and Password
    [Space]
    public TMP_InputField[] SSIDNPASS = null;

    [InspectorName("Room Attributes")]
    [Space(10)]
    [Header("Room Attributes")]
    //[SerializeField] Sprite[] RoomsIcons = null;
    public LeanWindow AddMenu = null;
    public TMP_InputField Roomname = null;
    public TMP_Dropdown RoomType = null;
    [SerializeField] Sprite[] Icons = null;
    [Space(10)]
  
    public static string directory = "/SavedDevices";
    public static string FileName = "Devices.txt";

    [Space(10)]
    [Header("Location Variables")]
    public float Latitude = 0f;
    public float Longitudte = 0f;
    public float HomeDistance = 5f;
    public bool IsUserInHome = true;
    public event Action OnHomeStateChange;
    [Space(10)]

    [HideInInspector]public static UImanager UIMANAGER;
    private void Awake()
    {
        initializeAllRooms();
        if (Load_Devices_Data() != null) {
            AllDeviceData = Load_Devices_Data();
        }
        RefrechBlockView();
        MenusObj[0].SetActive(mainactivestate);
        MenusObj[1].SetActive(receentActivestate);
       //MenusObj[2].SetActive(MyHomeActivestate);
        //MenusObj[3].SetActive(Contactactivestate);
    }
    void Start()
    {
        UIMANAGER = this;
        Application.targetFrameRate = FrameRate;
        Screen.sleepTimeout = (ScreenAutoDimming == true)? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
        SelectionColor = colorthemeController.CurrentTheme.ExtraColor;
        roomsnumber = (byte)PlayerPrefs.GetInt("Created Rooms");//i check hom many room saved in the player prefab folder and set the number of rooms to that value 
        CurrentMenu = (byte)menus.MainMenu;
        TabButtonsColorCheck();
        InvokeFavouriteList();
        IsUserInHome = PlayerPrefs.GetInt("IsUserInHome",1) == 1;
        Longitudte = PlayerPrefs.GetFloat("UserLong", 0f);
        Latitude = PlayerPrefs.GetFloat("UserLat", 0f);
        StartCoroutine(GetGPSLocation());
        SortMenus();
    }

    public void RefrechBlockView()
    {
        IsThereAnyDevice.gameObject.SetActive(AllDeviceData.Count == 0);
    }
    public void SortMenus()
    {
        for (int i = 0; i < MenusObj.Length; i++)
        {
            Vector3 Newlocation = new Vector3((canvas.GetComponent<RectTransform>().rect.width) * i, 0f, 0f);
            MenusObj[i].GetComponent<RectTransform>().localPosition = Newlocation;
        }
    }
    public void ResetallButtons()
    {
        var _ScenarioAddingScript = gameObject.GetComponent<ScenarioAddingScript>();
        String Name = PlayerPrefs.GetString("Name", "null");
        int RMe = PlayerPrefs.GetInt("RememberMe", 1);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("Name", Name);
        PlayerPrefs.SetInt("RememberMe", RMe);
        AllDeviceData.Clear();
        _ScenarioAddingScript.SO.Clear();
        _ScenarioAddingScript.Save();
        Save_Devices_inJASON(AllDeviceData);
        SceneManager.LoadSceneAsync(3);
    }

    public void SignOut() {
        PlayerPrefs.SetInt("RememberMe", 0);
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadARScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void InvokeFavouriteList()
    {
        FavouriteCard[] Children = FavouritesParent.GetComponentsInChildren<FavouriteCard>();
        if (Children.Length > 0)
        {
            foreach (FavouriteCard child in Children)
            {
                Destroy(child.gameObject);
            }
        }



        if (AllDeviceData.Count > 0 && AllDeviceData.Exists((x)=>x.Favourtie.Contains(true)) )
        {
            AllDeviceData.ForEach(x =>
            {
                for (int i = 0; i < x.Favourtie.Length; i++)
                {
                    if (x.Favourtie[i])
                    {
                        GameObject FavouriteDevice = Instantiate(FavouriteDevicePrefab, FavouritesParent);
                        var CardScript = FavouriteDevice.GetComponent<FavouriteCard>();
                        CardScript.Index = i;
                        CardScript.RoomName =  PlayerPrefs.GetString("Room Name" + x.RoomNumber, "");
                        CardScript.CardName = x.ToggleNames[i];
                        CardScript.ProductName = x.ProductName;
                        CardScript.State = x.States[i];
                    }
                }
            });
        }
        else 
        {
            GameObject FavouriteDevice = Instantiate(FavouriteDevicePrefab, FavouritesParent);
            var CardScript = FavouriteDevice.GetComponent<FavouriteCard>();
            CardScript.Index = 0;
            CardScript.RoomName = "Favourites are empty";
            CardScript.CardName = "Add a device to your favourite list to show up in here";
            CardScript.ProductName = "";
            CardScript.State = false;
        }
    }
    

    public void TabButtonsColorCheck()
    {
        TabButtons[CurrentMenu].color = SelectionColor;
        for (int i = 0; i < TabButtons.Length; i++)
        {
            if (i != CurrentMenu)
            {
                TabButtons[i].color = NormalColor;
            }
        }
    }

    public void BringSceneCreationDown()
    {
        ScenarioPage.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, .5f);
    }
    public void BringRemotePageDown()
    {
        if (PlayerPrefs.GetString("Remote" + 0, "") != "")
        {
            Vector2 Newpos = RemotePage.GetComponent<RectTransform>().anchoredPosition == Vector2.zero ? new Vector2(0f, Screen.height) : Vector2.zero;
            RemotePage.GetComponent<RectTransform>().DOAnchorPos(Newpos, .5f);
        }else{
            Toast.Instance.Show("No remotes found 😕",2f,Toast.ToastColor.Magenta);
        }
    }
    
    
    #region GPS Code
    IEnumerator GetGPSLocation()
    {
        if (!Input.location.isEnabledByUser) 
        {
            Toast.Instance.Show("Please enable phone GPS !",3f,Toast.ToastColor.Magenta);
            yield break;
        }
        Input.location.Start();
        int Waittime = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && Waittime > 0) 
        {
            yield return new WaitForSeconds(1);
            Waittime--;
        }

        if (Waittime <= 0)
        {
            Toast.Instance.Show("GPS Service timeout");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Toast.Instance.Show("Unable to find phone location");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Running)
        {
            StartCoroutine(HasUserLeftHome());
        }
    }

    public void SaveHomeLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            Longitudte = Input.location.lastData.longitude;
            Latitude = Input.location.lastData.latitude;
            PlayerPrefs.SetFloat("UserLong", Longitudte);
            PlayerPrefs.SetFloat("UserLat", Latitude);
            Toast.Instance.Show("New Home Location Saved", 2f, Toast.ToastColor.Blue);
        }
        else
        {
            Toast.Instance.Show("GPS Service is not enabled 🌎", 2f, Toast.ToastColor.Magenta);
        }
    }

    IEnumerator HasUserLeftHome()
    {
        while (Input.location.status == LocationServiceStatus.Running) {
            bool TempBool = IsUserInHome;
            var TempLongitudte = Input.location.lastData.longitude;
            var TempLatitude = Input.location.lastData.latitude;
            //IsUserInHome = Vector2.Distance(new Vector2(TempLatitude, TempLongitudte), new Vector2(Latitude, Longitudte)) <= HomeDistance;
            IsUserInHome= Calculate_Distance(TempLongitudte, TempLatitude, Longitudte, Latitude) <= HomeDistance;
            Debug.Log(Calculate_Distance(TempLongitudte, TempLatitude, Longitudte, Latitude));
            if (TempBool != IsUserInHome) 
            {
                string Message = IsUserInHome ? "Welcome Home 😄" : " Closing the door behind you";
                Toast.Instance.Show(Message, 3f, Toast.ToastColor.Dark);
                HomeStateChangeTrigger();
                PlayerPrefs.SetInt("IsUserInHome", IsUserInHome?1:0);
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public void HomeStateChangeTrigger()
    {
        if (OnHomeStateChange !=null)
        {
            OnHomeStateChange();
        }
    }
    float DegToRad(float deg)
    {
        float temp;
        temp = (deg * Mathf.PI) / 180.0f;
        temp = Mathf.Tan(temp);
        return temp;
    }

    float Distance_x(float lon_a, float lon_b, float lat_a, float lat_b)
    {
        float temp;
        float c;
        temp = (lat_b - lat_a);
        c = Mathf.Abs(temp * Mathf.Cos((lat_a + lat_b)) / 2);
        return c;
    }

    private float Distance_y(float lat_a, float lat_b)
    {
        float c;
        c = (lat_b - lat_a);
        return c;
    }

    float Final_distance(float x, float y)
    {
        float c;
        c = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f))) * 6371;
        return c;
    }

    //*******************************
    //This is the function to call to calculate the distance between two points

    public float Calculate_Distance(float long_a, float lat_a, float long_b, float lat_b)
    {
        float a_long_r, a_lat_r, p_long_r, p_lat_r, dist_x, dist_y, total_dist;
        a_long_r = DegToRad(long_a);
        a_lat_r = DegToRad(lat_a);
        p_long_r = DegToRad(long_b);
        p_lat_r = DegToRad(lat_b);
        dist_x = Distance_x(a_long_r, p_long_r, a_lat_r, p_lat_r);
        dist_y = Distance_y(a_lat_r, p_lat_r);
        total_dist = Final_distance(dist_x, dist_y);
        //prints the distance on the console
        return total_dist;

    }
    #endregion

    #region Room logic
    public void initializeAllRooms()
    {
        RefrechBlockView();
        for (int i = 1; i <= PlayerPrefs.GetInt("Created Rooms",0) && PlayerPrefs.GetInt("Created Rooms",0) != 0; i++)
        {
            GameObject AddedRoom = Instantiate(newRoom, RoomParent);
            AddedRoom.transform.SetSiblingIndex(AddedRoom.transform.GetSiblingIndex() - 1);
            AddedRoom.GetComponent<OpenRoomScript>().roomattribute = (byte)i;
            string Name = PlayerPrefs.GetString("Room Name" + i, "null");
            AddedRoom.GetComponentInChildren<TextMeshProUGUI>().text = Name;
            AddedRoom.GetComponentsInChildren<Image>()[1].sprite = Icons[PlayerPrefs.GetInt("Room Type" + i,1)];
        }
    }
    public void InitializeAllDevices(int RoomAttribute)
    {
        if (PlayerPrefs.GetInt("Created Rooms",0) != 0)
        {
            if (AllDeviceData.Count > 0)
            {
                for (int index = 0; index < AllDeviceData.Count; index++)
                {
                    if (AllDeviceData[index].RoomNumber == RoomAttribute)
                    {

                        GameObject AddedDevice = Instantiate(newDevice, DevicesParent.transform);

                        var IdenityChanger = AddedDevice.GetComponent<DeviceIdentityChanger>();

                        IdenityChanger.NameUI.text = AllDeviceData[index].DeviceName;
                        IdenityChanger.IconUI.sprite = DeviceIcons[AllDeviceData[index].Type];
                        IdenityChanger.ProductName = AllDeviceData[index].ProductName;
                        IdenityChanger.State = AllDeviceData[index].States;
                        IdenityChanger.ToggleNames = AllDeviceData[index].ToggleNames;

                        if (AllDeviceData[index].Type == (int)Products.Mini)
                        {
                            IdenityChanger.NoOfPlugs = 4;
                        }
                        else if (AllDeviceData[index].Type == (int)Products.Classic)
                        {
                            IdenityChanger.NoOfPlugs = 8;
                        }
                    }
                }
            }
         
        }
        else 
        {
            Debug.Log("****NoRoomsCreated****");
        }
    }

    public void CreateRoomPopup() 
    {
        DeviceSettings.Instance.Show("Room settings","Room Name"
            ,() => 
            {
                if (DeviceSettings.Instance.NameField.text != "")
                {
                    Createroom(DeviceSettings.Instance.NameField.text,DeviceSettings.Instance.IconIndex);
                    Toast.Instance.Show("Room Created");
                    DeviceSettings.Instance.NameField.text = "";
                } else 
                {
                    Toast.Instance.Show("Select a name");
                    DeviceSettings.Instance.NameField.text = "";
                }
            }
            );
    }

    public void Createroom(string RoomName,int RoomIcon)
    {
        GameObject AddedRoom = Instantiate(newRoom, RoomParent);
        AddedRoom.transform.SetSiblingIndex(AddedRoom.transform.GetSiblingIndex() - 1);
        roomsnumber++;
        PlayerPrefs.SetInt("Created Rooms", roomsnumber);
        AddedRoom.GetComponent<OpenRoomScript>().roomattribute = roomsnumber;
        PlayerPrefs.SetInt("Room Type" + roomsnumber, RoomIcon);
        PlayerPrefs.SetString("Room Name" + roomsnumber, RoomName);
        PlayerPrefs.Save();

        AddedRoom.GetComponentsInChildren<Image>()[1].sprite = Icons[RoomIcon];
        AddedRoom.GetComponentInChildren<TMP_Text>().text = RoomName;
        Debug.Log(AddedRoom.GetComponentInChildren<TMP_Text>());

        AddMenu.TurnOff();
    }
    #endregion

    #region PHPCALLING

    void AddRoomDatabase()
        {
            StartCoroutine(AddRoomDB());
        }
        IEnumerator AddRoomDB()
        {
            WWWForm UserForm = new WWWForm();
            string UserName = PlayerPrefs.GetString("Name", "null");
            UserForm.AddField("Name", UserName);
            UserForm.AddField("Room_Name", Roomname.text);
            UserForm.AddField("Room_Type", RoomType.value);
            UnityWebRequest PHP = UnityWebRequest.Post("https://homexbolyka.000webhostapp.com/RoomsData.php", UserForm);
            yield return PHP.SendWebRequest();
            if (PHP.result == UnityWebRequest.Result.ProtocolError)
            {
                 Debug.Log(PHP.error);
            }
            Debug.Log(PHP.downloadHandler.text);
        }


        #endregion

    #region WIFICONFIG

    public void confirmButton() 
    {
        StartCoroutine(ConfirmbuttonWebRequest());
    }
    IEnumerator ConfirmbuttonWebRequest() 
    {
        string SSID = SSIDNPASS[0].text;
        string Password = SSIDNPASS[1].text;
        string URLREquest = "http://192.168.2.1/SAVE?ssidlength=" + SSID.Length + "&ssid=" + SSID + "&passlength=" + Password.Length + "&pass=" + Password + "&username=" + PlayerPrefs.GetString("Name", "null");
        UnityWebRequest request = UnityWebRequest.Get(URLREquest);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
    }

    #endregion

    #region JASON SAVING

    public static void Save_Devices_inJASON(List<DeviceData> Object)
    {
        Debug.Log("saving Device Data");
        string direct = Application.persistentDataPath + directory;
        Debug.Log("Files Directory :" + direct);
        if (!Directory.Exists(direct)) { Directory.CreateDirectory(direct); }
        string JSONDATA = null;
        if (Object != null)
        {
            for (int i = 0; i < Object.Count; i++)
            {
                if (i + 1 != Object.Count)
                {
                    JSONDATA += JsonUtility.ToJson(Object[i]) + ",";
                }
                else
                {
                    JSONDATA += JsonUtility.ToJson(Object[i]);
                }
            }
            Debug.Log(JSONDATA);
            File.WriteAllText(direct + FileName, "[" + JSONDATA + "]");
            PlayerPrefs.Save();
        }

    }

    public static List<DeviceData> Load_Devices_Data()
    {
        string path = Application.persistentDataPath + directory + FileName;
        if (File.Exists(path))
        {
            string jasonData = File.ReadAllText(path);
            if (JsonHelper.GetArray<DeviceData>(jasonData) != null)
            {
                DeviceData[] Savedarray = JsonHelper.GetArray<DeviceData>(jasonData);
                List<DeviceData> SavedObject = new List<DeviceData>(Savedarray);
                return SavedObject;
            }
            else
            {
                File.WriteAllText(path, "{}");
                return null;
            }
        }
        else
        {
            Debug.LogError("JSON FILE WAS NOT CREATED");
            Debug.LogError(path);
            File.WriteAllText(path, "{}");
            Load_Devices_Data();
            return null;
        }

    }

    #endregion
}
