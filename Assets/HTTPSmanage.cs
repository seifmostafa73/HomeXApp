using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPSmanage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string ServerURL = "http://192.168.208.174";
    [SerializeField] int HTTPPORT = 80;
    void Start()
    {
        //StartCoroutine(SendHTTPMessage(ServerURL,"username=Tarek&password=1234"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SendviaGet(string _state)
    {
        StartCoroutine(GetRequest(_state));
    }
    public void SendMessage(string Message)
    {
        StartCoroutine(SendHTTPMessage(ServerURL,Message));
    }

    IEnumerator SendHTTPMessage(string URL,string Message) 
    {
        URL = "http://192.168.208.174" + "/?username=Tarek&password=1234";
        UnityWebRequest request = UnityWebRequest.Post(URL,Message);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
    }

    IEnumerator GetRequest(string State)
    {
        string URL = ServerURL + "/control?state="+ State;

        //URL = URL + ":" + HTTPPORT+ "/ISbla?Type="+CoffeType;// this open URL  at port HTTPPORT , and calls the uri Isbla giving it an arguments Type of value CoffeType
        UnityWebRequest request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        yield return new WaitUntil(()=> request.downloadHandler.isDone );
        var HEader = request.GetRequestHeader("code");
        var Data = request.downloadHandler.text; 
        Debug.Log("HEader "+HEader+" Data "+Data);  
    }

}
