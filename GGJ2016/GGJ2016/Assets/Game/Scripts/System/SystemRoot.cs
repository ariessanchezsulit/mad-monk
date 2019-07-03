using UnityEngine;
using System.Collections;

namespace Game {

	public class SystemRoot : GameScene
    {
		void Start()
		{
            //StartCoroutine(StartGame());
            GameSignals.START_GAME.Dispatch();
        }

        IEnumerator StartGame()
        {
            //yield return null;
            //yield return LoadSceneAdditiveAsync(EScene.World);
            //yield return LoadSceneAdditiveAsync(EScene.UI);

            yield return new WaitForSeconds(1f);

            GameSignals.START_GAME.Dispatch();
        }
	}
}