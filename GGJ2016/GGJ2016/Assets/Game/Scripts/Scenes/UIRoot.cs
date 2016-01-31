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
		private List<MonsterIcon> MonsterIcons;
		[SerializeField]
		private GameObject GaugeIcon;
		[SerializeField]
		private Image GaugeFillImage;

		[SerializeField]
		private float GaugeFillMaxWidth = 360;

		[Serializable]
		public class MonsterIcon
		{
			public MonsterType Type;
			public GameObject Icon;
		}

		[SerializeField]
		private GameObject ResultsScreen;
		[SerializeField]
		private Text ResultsKillCountText;
		[SerializeField]
		private Text ResultsHighestKillCountText;

		private AudioListener AudioListener;

		protected override void Awake() {
			base.Awake();

			GameSignals.ON_MONSTER_HIT.AddListener (UpdateGauge);
			GameSignals.ON_MONSTER_SUMMONED.AddListener (UpdateGauge);
			GameSignals.ON_MONSTER_SHOWN.AddListener (OnMonsterShown);
			GameSignals.ON_MONSTER_DEAD.AddListener (OnMonsterDead);
			GameSignals.END_GAME.AddListener (OnGameEnd);
		}

		void Start()
		{
			AudioListener = GameObject.FindObjectOfType<AudioListener> ();

			GameSignals.START_GAME.Dispatch ();
		}

		void OnDestroy()
		{
			GameSignals.ON_MONSTER_HIT.RemoveListener (UpdateGauge);
			GameSignals.ON_MONSTER_SUMMONED.RemoveListener (UpdateGauge);
			GameSignals.ON_MONSTER_SHOWN.RemoveListener (OnMonsterShown);
			GameSignals.ON_MONSTER_DEAD.RemoveListener (OnMonsterDead);
			GameSignals.END_GAME.RemoveListener (OnGameEnd);
		}

		void UpdateGauge(ISignalParameters parameters)
		{
			float hp = (float)(int)parameters.GetParameter(GameParams.MONSTER_HP);
			float maxHp = (float)(int)parameters.GetParameter(GameParams.MONSTER_MAX_HP);

			float rectWidth = GaugeFillMaxWidth * (hp / maxHp);

			Vector2 sizeDelta = GaugeFillImage.rectTransform.sizeDelta;
			sizeDelta.x = rectWidth;
			GaugeFillImage.rectTransform.sizeDelta = sizeDelta;
		}

		private void OnMonsterShown(ISignalParameters parameters)
		{
			if (GaugeIcon != null) GaugeIcon.SetActive (false);
			MonsterType monsterType = (MonsterType)parameters.GetParameter(GameParams.MONSTER_TYPE);
			GaugeIcon = MonsterIcons.Find (mi => mi.Type == monsterType).Icon;
			GaugeIcon.SetActive (true);
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
			ResultsHighestKillCountText.text = string.Format("Best: {0}", currHighScore.ToString("N0"));

			ResultsScreen.SetActive (true);
		}

		public void QuitGame()
		{
			Application.Quit ();
		}

		public void TogglePause(bool shouldPause)
		{
			Time.timeScale = shouldPause ? 0 : 1;
		}

		public void ToggleSound(bool enableSound)
		{
			AudioListener.enabled = enableSound;
		}
	}

}