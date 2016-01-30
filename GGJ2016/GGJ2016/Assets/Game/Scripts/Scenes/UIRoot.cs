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
		private GameObject KillIconTemplate;
		[SerializeField]
		private Transform KillIconsParent;

		private List<GameObject> KillIcons = new List<GameObject> ();
		private List<GameObject> UnusedKillIcons = new List<GameObject> ();

		protected override void Awake() {
			base.Awake();

			// temp for test
			LoadSceneAdditive (EScene.Monsters);

			GameSignals.ON_MONSTER_DEAD.AddListener (OnMonsterDead);
		}

		void OnDestroy()
		{
			GameSignals.ON_MONSTER_DEAD.RemoveListener (OnMonsterDead);
		}

		public void RestartGame()
		{
			ClearKillIcons ();
			GameSignals.START_GAME.Dispatch ();
		}

		private void ClearKillIcons()
		{
			foreach (GameObject killIcon in KillIcons) {
				killIcon.SetActive (false);
				UnusedKillIcons.Add(killIcon);
			}
			KillIcons.Clear ();
		}

		private GameObject GetKillIcon()
		{
			if (UnusedKillIcons.Count == 0) {
				GameObject killIcon = Instantiate (KillIconTemplate) as GameObject;
				killIcon.transform.SetParent (KillIconsParent);
				return killIcon;
			}
			return UnusedKillIcons [0];
		}

		private void OnMonsterDead(ISignalParameters @params)
		{
			GameObject killIcon = GetKillIcon ();
			killIcon.SetActive (true);
		}
	}

}