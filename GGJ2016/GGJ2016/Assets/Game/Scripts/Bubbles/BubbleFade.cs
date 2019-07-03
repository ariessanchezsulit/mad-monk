using UnityEngine;
using URandom = UnityEngine.Random;

using System;
using System.Collections;

using Common;
using Common.Signal;

namespace Game {

	public class BubbleFade : MonoBehaviour {

		// fade
		[SerializeField]
		private float fadeDuration = 1.0f;

		[SerializeField]
		private GameObject explosion;

		private void Awake() {
			Assertion.AssertNotNull(this.explosion);
		}

		private void OnEnable() {
			this.StartCoroutine(this.Fade());
		}

		private void OnDisable() {
			this.StopAllCoroutines();
		}

		private IEnumerator Fade() {
			yield return null;
			this.explosion.SetActive(true);
			yield return new WaitForSeconds(this.fadeDuration);

			// dispatch bubble popped
			Signal signal = GameSignals.ON_BUBBLE_POPPED;
			signal.Dispatch();

			// destroy game object
			GameObject.Destroy(this.gameObject);
		}

	}

}