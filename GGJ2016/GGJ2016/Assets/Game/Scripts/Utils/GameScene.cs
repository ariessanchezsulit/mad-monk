using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

public enum EScene {
	System,
	Monsters,
	UI,
	Bubbles,
};

namespace Game {

	public class GameScene : MonoBehaviour {
		protected virtual void Awake() {
			if (GameObject.FindObjectOfType<SystemRoot>() == null) {
				this.LoadSceneAdditive(EScene.System);
			}
		}

		protected void LoadScene(EScene scene) {
			//Application.LoadLevel(scene.ToString());
			SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);

		}

		protected void LoadSceneAdditive(EScene scene) {
			//Application.LoadLevelAdditive(scene.ToString());
			SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
		}
	}

}

