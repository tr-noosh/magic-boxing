using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuState : int {
	MAIN,
	SETTINGS,
	CREDITS,
	LOSE,
	WIN,
	GAMEPLAY
}

public class MenuController : MonoBehaviour
{
	public static Scene menuScene;
	public static Nullable<Scene> gameScene;
	public static MenuController instance;

	public static keymap currentKeymap;
	
	public MenuState menuState = MenuState.MAIN;
	Dictionary<MenuState, GameObject> menus = new Dictionary<MenuState, GameObject>();
	public GameObject mainObject, settingsObject, creditsObject, loseObject, winObject;
	
	// keys
	int keyCycle = 1;
	
	public keymap[] keymaps;
	public Texture[] sprites;
	public RawImage key_u, handL, handR;

	public RawImage scrn;

	void Awake() {
		instance = this;
		menuScene = gameObject.scene;
		SceneManager.sceneLoaded += onSceneLoaded;
		menus.Add(MenuState.MAIN, mainObject);
		menus.Add(MenuState.SETTINGS, settingsObject);
		menus.Add(MenuState.CREDITS, creditsObject);
		menus.Add(MenuState.LOSE, loseObject);
		menus.Add(MenuState.WIN, winObject);
		changeMenu(menuState);
		currentKeymap = keymaps[keyCycle];
	}

	void onSceneLoaded(Scene scn, LoadSceneMode _) {
		if (scn.name == "Game") { 
			gameScene = scn;
			SceneManager.SetActiveScene(scn);
		}
	}

	void SettingsUpdate()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			keyCycle = (keyCycle + 1) % sprites.Length;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			keyCycle = (keyCycle - 1 + sprites.Length) % sprites.Length;
		}
		key_u.texture = sprites[keyCycle];
		currentKeymap = keymaps[keyCycle];

		switch (keyCycle) {
			case 0:
				handL.rectTransform.anchoredPosition = new Vector2(91.9f, 52.4f);
				handR.rectTransform.anchoredPosition = new Vector2(181.5f, 52.4f);
				break;
			case 1:
				handL.rectTransform.anchoredPosition = new Vector2(91.9f, 52.4f);
				handR.rectTransform.anchoredPosition = new Vector2(231.8f, 39.2f);
				break;
			case 2:
				handL.rectTransform.anchoredPosition = new Vector2(99.2f, 56.4f);
				handR.rectTransform.anchoredPosition = new Vector2(231.8f, 39.2f);
				break;
		}
	}

	public void changeMenu(MenuState menu)
	{
		menuState = menu;
		foreach (var m in menus) {
			m.Value.SetActive(false);
		}
		if (menuState == MenuState.GAMEPLAY) {
			startGame();
		}
		else {
			menus[menu].SetActive(true);
		}
	}

	void Update()
	{
		switch(menuState) {
			case MenuState.SETTINGS: 
				SettingsUpdate();
				break;
			default: 
				break;
		}
	}

	public void startGame() {
		if (gameScene == null) {
			SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
		}
	}
	
	private IEnumerator winRoutine() {
		yield return new WaitForEndOfFrame();
		scrn.texture = ScreenCapture.CaptureScreenshotAsTexture();
		scrn.gameObject.SetActive(true);
		if (gameScene != null) {
			SceneManager.UnloadSceneAsync("Game");
			gameScene = null;
			SceneManager.SetActiveScene(menuScene);
			changeMenu(MenuState.WIN);
		}
	}
	private IEnumerator loseRoutine() {
		yield return new WaitForEndOfFrame();
		scrn.texture = ScreenCapture.CaptureScreenshotAsTexture();
		scrn.gameObject.SetActive(true);
		if (gameScene != null) {
			SceneManager.UnloadSceneAsync("Game");
			gameScene = null;
			SceneManager.SetActiveScene(menuScene);
			changeMenu(MenuState.LOSE);
		}
	}

	public void goMain() {
		scrn.gameObject.SetActive(false);
		changeMenu(MenuState.MAIN);
	}
	public void goSettings() {
		changeMenu(MenuState.SETTINGS);
	}
	public void goCredits() {
		changeMenu(MenuState.CREDITS);
	}
	public void goPlay() {
		changeMenu(MenuState.GAMEPLAY);
	}

	void stopWin() { StartCoroutine(winRoutine()); }
	void stopLose() { StartCoroutine(loseRoutine()); }
	public static void Win() { instance.stopWin(); }
	public static void Lose() { instance.stopLose(); }
}
