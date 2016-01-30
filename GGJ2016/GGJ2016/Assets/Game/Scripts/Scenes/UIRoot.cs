using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Common;
using Common.Signal;

namespace Game {

	public class UIRoot : GameScene {

		[SerializeField]
		private Text KillCountText;

		private int KillCount;

		[SerializeField]
		private GameObject ResultsScreen;
		[SerializeField]
		private Text ResultsKillCountText;
		[SerializeField]
		private Text ResultsHighestKillCountText;

		protected override void Awake() {
			base.Awake();

			// temp for test
			LoadSceneAdditive (EScene.Monsters);

			GameSignals.ON_MONSTER_DEAD.AddListener (OnMonsterDead);
			GameSignals.END_GAME.AddListener (OnGameEnd);
		}

		void OnDestroy()
		{
			GameSignals.ON_MONSTER_DEAD.RemoveListener (OnMonsterDead);
			GameSignals.END_GAME.RemoveListener (OnGameEnd);
		}

		public void RestartGame()
		{
			ResultsScreen.SetActive (false);
			ResetKillCount ();
			GameSignals.START_GAME.Dispatch ();
		}

		private void ResetKillCount()
		{
			SetKillCount(0);
		}

		private void OnMonsterDead(ISignalParameters @params)
		{
			SetKillCount (KillCount + 1);
		}

		private void SetKillCount(int killCount)
		{
			KillCount = killCount;
			KillCountText.text = KillCount.ToString ("N0");
		}

		private void OnGameEnd(ISignalParameters @params)
		{
			ResultsKillCountText.text = KillCount.ToString ("N0");
			int currHighScore = PlayerPrefs.GetInt ("HighScore", 0);
			if (KillCount > currHighScore) {
				currHighScore = KillCount;
				PlayerPrefs.SetInt ("HighScore", KillCount);
			}
			ResultsHighestKillCountText.text = currHighScore.ToString("N0");

			ResultsScreen.SetActive (true);
		}
	}

}