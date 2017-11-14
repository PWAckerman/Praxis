using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

static class CircularLinkedList {
	public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
	{
		return current.Next ?? current.List.First;
	}

	public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
	{
		return current.Previous ?? current.List.Last;
	}
}

public class PauseMenuController : MonoBehaviour {

	// Use this for initialization
	public ResourceManager rm;
	public PauseMenuManager pm;
	public GameModeManager gm;
	public AudioSource music;
	public GameObject[] panels;
	public Image[] frames;
	public GameObject currentPanel;
	public int currentPanelIndex = 0;
	public bool paused;
	public float pauseRate = 0.5f;
	public float nextPause;
	public Dictionary<string, RectTransform> Menus = new Dictionary<string, RectTransform>();


	void Start () {
		gm = GameModeManager.getInstance ();
		pm = PauseMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager();
		Menus.Add ("info", GameObject.FindGameObjectWithTag ("InfoMenu").GetComponent<RectTransform>());
		Menus.Add ("research", GameObject.FindGameObjectWithTag ("ResearchMenu").GetComponent<RectTransform>());
		Menus.Add ("system", GameObject.FindGameObjectWithTag ("SystemMenu").GetComponent<RectTransform>());
		Menus.Add ("map", GameObject.FindGameObjectWithTag ("MapMenu").GetComponent<RectTransform>());
		Menus.Add ("inventory", GameObject.FindGameObjectWithTag ("InventoryMenu").GetComponent<RectTransform>());
		music = GameObject.FindGameObjectWithTag ("music").GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		bool pause = Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown (KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Delete);
		if (pause && gm.isPaused()) {
			if (Time.unscaledTime > nextPause) {
				gm.setCurrentMode (GameModeManager.Mode.RUNNING);
				music.UnPause ();
				HideMenus ();
				Time.timeScale = 1;
				nextPause = Time.unscaledTime + pauseRate;
			}
		} else if (!gm.isPaused() && pause) {
			if (Time.unscaledTime > nextPause) {
				gm.setCurrentMode (GameModeManager.Mode.PAUSED);
				music.Pause ();
				ShowSelectedMenu ();
				Time.timeScale = 0;
				nextPause = Time.unscaledTime + pauseRate;
			}
		} else if (gm.isPaused() && !pause) {
			Debug.Log ("Paused");
			ShowSelectedMenu ();
		}
	}
		
	void ShowSelectedMenu(){
		Debug.Log ("show menu");
		switch (pm.currentMenu) {
			case PauseMenuManager.Menu.INFO:
					Debug.Log ("Pause Menu Info");
					HideMenus ();
					Menus["info"].anchoredPosition = Vector2.Lerp (Menus["info"].anchoredPosition, new Vector2 (0f, 0f), 10f);
					break;
			case PauseMenuManager.Menu.RESEARCH:
					Debug.Log ("Research Menu Resarch");
					HideMenus ();
					Menus["research"].anchoredPosition = Vector2.Lerp (Menus["research"].anchoredPosition, new Vector2 (0f, 0f), 10f);
					break;
			case PauseMenuManager.Menu.MAP:
					Debug.Log ("Map Menu Map");
					HideMenus ();
					Menus["map"].anchoredPosition = Vector2.Lerp (Menus["map"].anchoredPosition, new Vector2 (0f, 0f), 10f);
					break;
			case PauseMenuManager.Menu.INVENTORY:
					Debug.Log ("Inventory Menu Inv");
					HideMenus ();
					Menus["inventory"].anchoredPosition = Vector2.Lerp (Menus["inventory"].anchoredPosition, new Vector2 (0f, 0f), 10f);
					break;
			case PauseMenuManager.Menu.SYSTEM:
					Debug.Log ("System Menu Sys");
					HideMenus ();
					Menus["system"].anchoredPosition = Vector2.Lerp (Menus["system"].anchoredPosition, new Vector2 (0f, 0f), 10f);
					break;
			default:
				return;
		}
	}

	void HideMenus(){
		Menus.Values.ToList ().ForEach (x => x.anchoredPosition = Vector2.Lerp (x.anchoredPosition, new Vector2 (0f, 900f), 10f));
	}
}
