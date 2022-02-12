using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
	[Header ("space between menu items")]
	[SerializeField] Vector2 spacing = new Vector2();

	[Space]
	[Header ("Main button rotation")]
	[SerializeField] float rotationDuration = 0f;
	[SerializeField] Ease rotationEase = new Ease();

	[Space]
	[Header ("Animation")]
	[SerializeField] float expandDuration = 0f;
	[SerializeField] float collapseDuration = 0f;
	[SerializeField] Ease expandEase = new Ease();
	[SerializeField] Ease collapseEase = new Ease();

	[Space]
	[Header ("Fading")]
	[SerializeField] float expandFadeDuration = 0f;
	[SerializeField] float collapseFadeDuration = 0f;

	Button mainButton = null;
	SettingsMenuItem[] menuItems = null;

	//is menu opened or not
	bool isExpanded = false;

	Vector2 mainButtonPosition = new Vector2();
	int itemsCount = 0;

	void Start ()
	{
		//add all the items to the menuItems array
		itemsCount = transform.childCount - 1;
		menuItems = new SettingsMenuItem[itemsCount];
		for (int i = 0; i < itemsCount; i++) {
			// +1 to ignore the main button
			menuItems [i] = transform.GetChild (i + 1).GetComponent <SettingsMenuItem> ();
		}

		mainButton = transform.GetChild (0).GetComponent <Button> ();
		mainButton.onClick.AddListener (ToggleMenu);
		//SetAsLastSibling () to make sure that the main button will be always at the top layer
		mainButton.transform.SetAsLastSibling ();

		mainButtonPosition = mainButton.transform.position;

		//set all menu items position to mainButtonPosition
		ResetPositions ();
		RecalculateElements();
		Invoke("RecalculateElements",0.25f);
	}

    private void OnRectTransformDimensionsChange()
    {
		RecalculateElements();
	}

	public void RecalculateElements()
	{
		if (mainButton!=null) {
			mainButtonPosition = mainButton.transform.position;
			ResetPositions(); 
			Debug.Log("Recalcualted");
			if (isExpanded) { ToggleMenu(); }
		}
	}

    public void ResetPositions ()
	{
		for (int i = 0; i < itemsCount; i++) {
			menuItems[i].trans.position = mainButtonPosition;
		}
	}

	void ToggleMenu ()
	{
		isExpanded = !isExpanded;

		if (isExpanded) {
			//menu opened
			for (int i = 0; i < itemsCount; i++) {
				menuItems [i].trans.DOMove (mainButtonPosition + spacing * (i + 1), expandDuration).SetEase (expandEase);
				//Fade to alpha=1 starting from alpha=0 immediately
				menuItems [i].img.DOFade (1f, expandFadeDuration).From (0f);
			}
		} else {
			//menu closed
			for (int i = 0; i < itemsCount; i++) {
				menuItems [i].trans.DOMove (mainButtonPosition, collapseDuration).SetEase (collapseEase);
				//Fade to alpha=0
				menuItems [i].img.DOFade (0f, collapseFadeDuration);
			}
		}

		//rotate main button arround Z axis by 180 degree starting from 0
		mainButton.transform
			.DORotate (Vector3.forward * 180f, rotationDuration)
			.From (Vector3.zero)
			.SetEase (rotationEase);
	}

	void OnDestroy ()
	{
		//remove click listener to avoid memory leaks
		mainButton.onClick.RemoveListener (ToggleMenu);
	}
}
