using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

using System;
using System.Collections;

using Common;
using Common.Signal;

namespace Game {

	public class BubbleFadeIn : MonoBehaviour {

		[SerializeField]
		private Image image;

		[SerializeField]
		private float fadeDuration = 0.15f;

		private float totalTime = 0.0f;

		private void Awake() {
			Assertion.AssertNotNull(this.image);
		}

		private void OnEnable() {
			this.StartCoroutine(this.Fade());
		}

		private void OnDisable() {
			this.StopAllCoroutines();
		}

		private IEnumerator Fade() {
			yield return new WaitForSeconds(this.fadeDuration);

			while (true) {
				// total time
				this.totalTime += Time.deltaTime;

				// fade
				float maxTrailSize = 1.0f;
				float fade = this.totalTime / maxTrailSize;

				// max
				if (fade > maxTrailSize) {
					fade = maxTrailSize;
				}

				Color color = this.image.color;
				color.a = fade;

				this.image.color = color;

				if (fade >= maxTrailSize) {
					yield break;
				}
			}

		}

	}

}