using UnityEngine;
using System.Collections;

namespace Game {

	public class SystemRoot : GameScene
    {
		void Start()
		{
            GameSignals.START_GAME.Dispatch();
        }
	}
}