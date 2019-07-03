using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Game {

	public class MonkFace : MonoBehaviour {

		[SerializeField]
		private List<Image> emotions;

		[SerializeField]
		private float duration = 0.5f;

		private int index = 0;

		private void Awake() {
			Assertion.Assert(this.emotions.Count > 0);
		}

		private void Start() {
			this.StartCoroutine(this.Animate());
		}

		private void Hide() {
			foreach (Image image in this.emotions) {
				image.gameObject.SetActive(false);
			}
		}

		private IEnumerator Animate() {
			while (true) {
				yield return new WaitForSeconds(this.duration);
				this.index++;

				if (this.index >= this.emotions.Count) {
					this.index = 0;
				}

				this.Hide();
				this.emotions[this.index].gameObject.SetActive(true);
			}
		}
	}

}