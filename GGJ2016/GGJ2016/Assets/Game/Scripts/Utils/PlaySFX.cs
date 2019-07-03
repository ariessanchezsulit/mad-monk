using UnityEngine;
using System.Collections;

namespace Game {
	
	public class PlaySFX : MonoBehaviour {

		[SerializeField]
		private AudioSource source;

		[SerializeField]
		private AudioClip clip;

		private void Awake() {
		}

		private void Start() {
			this.Play();
		}

		public void Play() {
			this.source.clip = this.clip;
			this.source.Play();
		}

	}

}