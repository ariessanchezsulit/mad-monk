using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Common.Signal;

public enum ESfx {
	Start,
	Monster001,
	Monster002,
	ShowBubble,
	PopBubble,
};

namespace Game {
	
	public class AudioManager : MonoBehaviour {

		[SerializeField]
		private AudioSource source;

		[SerializeField]
		private AudioSource bgmSource;

		[SerializeField]
		private AudioClip bgm;

		[SerializeField]
		private AudioClip monsterStart;

		[SerializeField]
		private AudioClip monster001;

		[SerializeField]
		private AudioClip monster002;

		[SerializeField]
		private AudioClip showBubble;

		[SerializeField]
		private AudioClip popBubble;

		private Dictionary<ESfx, AudioClip> audioMap;

		private void Awake() {
			Assertion.AssertNotNull(this.source);
			Assertion.AssertNotNull(this.bgmSource);
			Assertion.AssertNotNull(this.bgm);
			Assertion.AssertNotNull(this.monsterStart);
			Assertion.AssertNotNull(this.monster001);
			Assertion.AssertNotNull(this.monster002);
			Assertion.AssertNotNull(this.showBubble);
			Assertion.AssertNotNull(this.popBubble);

			this.audioMap = new Dictionary<ESfx, AudioClip>();
			this.audioMap[ESfx.Start] = this.monsterStart;
			this.audioMap[ESfx.Monster001] = this.monster001;
			this.audioMap[ESfx.Monster002] = this.monster002;
			this.audioMap[ESfx.ShowBubble] = this.showBubble;
			this.audioMap[ESfx.PopBubble] = this.popBubble;
		}

		private void Start() {
			GameSignals.ON_PLAY_SFX.AddListener(this.OnPlaySFX);
			GameSignals.ON_PLAY_BGM.AddListener(this.OnPlayBGM);
		}

		private void OnDestroy() {
			GameSignals.ON_PLAY_SFX.RemoveListener(this.OnPlaySFX);
			GameSignals.ON_PLAY_BGM.RemoveListener(this.OnPlayBGM);
		}

		#region Signals

		private void OnPlaySFX(ISignalParameters parameters) {
			ESfx sfx = (ESfx)parameters.GetParameter(GameParams.AUDIO_ID);
			this.source.clip = this.audioMap[sfx];
			this.source.Play();
		}

		private void OnPlayBGM(ISignalParameters parameters) {
			this.bgmSource.clip = this.bgm;
			this.bgmSource.loop = true;
			this.bgmSource.Play();
		}

		#endregion

	}

}
