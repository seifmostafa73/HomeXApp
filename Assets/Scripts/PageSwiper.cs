using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation = new Vector3();
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    public int totalPages = 4;
    public int currentPage = 1;

    [InspectorName("TabButtons")]
    [SerializeField] UImanager uiManager = null;
    [SerializeField] Canvas canvas = null;

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
    }
    private void OnRectTransformDimensionsChange()
    { 
        uiManager.SortMenus();
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(
                                                             -canvas.GetComponent<RectTransform>().rect.width * uiManager.CurrentMenu,0, 0);
        panelLocation = transform.position;
    }
    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x; // sees the diffence between the start of the drag and the current position of the drag
        transform.position = panelLocation - new Vector3(difference, 0, 0);// then setting the position of the panel to the new diffenece , called each time a drag is called
    }
    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width; //how much of the screen has the user dragged
        if (Mathf.Abs(percentage) >= percentThreshold) // if it's greater than say :50 % of the screen then swipe
        {
            Vector3 newLocation = panelLocation; // saving the first location of the panel to return it if the user exceeds the last or first page
            if (percentage > 0 && currentPage < totalPages) // if the user swipes to the left-> increae the current page numbe, and set the new location to the cureent location + the screen width
            {
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                SmoothDeactivate();
            }
            else if (percentage < 0 && currentPage > 1) // same as the previous condition but in the previous direction
            {
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
                SmoothDeactivate();
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing)); // moves the panel smoothly
          
            panelLocation = newLocation;// set the saved current location to the new location 

        }
        else //if it's less than say :50 % of the screen then return the panel to the saved position
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

    }
    void SmoothDeactivate() 
    {
            uiManager.MenusObj[currentPage - 1].SetActive(true);
            uiManager.MenusObj[uiManager.CurrentMenu].SetActive(false);
            uiManager.CurrentMenu = (byte)(currentPage-1);
            uiManager.TabButtonsColorCheck();
    }
    public void SwiperButton(int RoomNumber)
    {
        int numberofswipes = Mathf.Abs(RoomNumber - (currentPage));

        Vector3 newLocation = panelLocation; // saving the first location of the panel to return it if the user exceeds the last or first page

        if (RoomNumber > currentPage && currentPage+numberofswipes <= totalPages)
        {
                currentPage += numberofswipes;
                newLocation += new Vector3(-Screen.width*numberofswipes, 0, 0);
                SmoothDeactivate();
        }
        else if (RoomNumber < currentPage && currentPage - numberofswipes >= 1)
        {
                currentPage -= numberofswipes;
                newLocation += new Vector3(Screen.width*numberofswipes, 0, 0);
                SmoothDeactivate();
        }
        StartCoroutine(SmoothMove(transform.position, newLocation, easing)); // moves the panel smoothly

        panelLocation = newLocation;// set the saved current location to the new location 
      
    }
}