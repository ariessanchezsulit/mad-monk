using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Game {

	public class Animator : MonoBehaviour {

		[SerializeField]
		private List<GameObject> sprites;

		[SerializeField]
		private float duration = 0.5f;

		private int index = 0;

		private void Awake() {
			Assertion.Assert(this.sprites.Count > 0);
		}

		private void Start() {
			this.StartCoroutine(this.Animate());
		}

		private void Hide() {
			foreach (GameObject image in this.sprites) {
				image.gameObject.SetActive(false);
			}
		}

		private IEnumerator Animate() {
			while (true) {
				yield return new WaitForSeconds(this.duration);
				this.index++;

				if (this.index >= this.sprites.Count) {
					this.index = 0;
				}

				this.Hide();
				this.sprites[this.index].gameObject.SetActive(true);
			}
		}
	}

}