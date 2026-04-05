using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState : int {
	MAIN,
	DIFFICULTY,
	SETTINGS,
	LOSE,
	WIN,
	CREDITS,
	GAMEPLAY
}
public class MenuController : MonoBehaviour
{
	public static keymap currentKeymap;

	public MenuState menuState = MenuState.MAIN;
	Dictionary<MenuState, GameObject> menuObj = new Dictionary<MenuState, GameObject>();
	
	// keys
	int keyCycle = 1;
	
	public keymap[] keymaps;
	public Texture[] sprites;
	public RawImage key_u, handL, handR;

	void Start() {}

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
		foreach (var m in menuObj) {
			m.Value.SetActive(false);
		}
		if (menuState == MenuState.GAMEPLAY) {
			startGame();
		}
		else {
			menuObj[menu].SetActive(true);
		}
	}

	void startGame() {

	}
	
	void Update()
	{
		switch(menuState) {
			case MenuState.MAIN: break;
			case MenuState.DIFFICULTY: break;
			case MenuState.SETTINGS: 
				SettingsUpdate();
				break;
			case MenuState.CREDITS: break;
			case MenuState.LOSE: break;
			case MenuState.WIN: break;
			case MenuState.GAMEPLAY: break;
		}
	}
	public void goMain() {
		changeMenu(MenuState.MAIN);
	}
	public void goSettings() {
		changeMenu(MenuState.SETTINGS);
	}
	public void goCredits() {
		changeMenu(MenuState.CREDITS);
	}
	public void goDifficulty() {
		changeMenu(MenuState.CREDITS);
	}
	public void goPlay() {
		changeMenu(MenuState.GAMEPLAY);
	}
	public void goLose() {
		changeMenu(MenuState.LOSE);
	}
	public void goWin() {
		changeMenu(MenuState.WIN);
	}
}
