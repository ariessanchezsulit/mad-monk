using UnityEngine;
using System.Collections;

namespace Game {

	public class SystemRoot : GameScene {
		void Start()
		{
			LoadSceneAdditive (EScene.Bubbles);
			LoadSceneAdditive (EScene.UI);
			LoadSceneAdditive (EScene.Monsters);
		}
	}

}