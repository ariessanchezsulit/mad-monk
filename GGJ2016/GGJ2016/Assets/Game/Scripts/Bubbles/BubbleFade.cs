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
		private float fadeDuration = 2.0f;

		private void Awake() {
		}

		private void OnEnable() {
			this.StartCoroutine(this.Fade());
		}

		private void OnDisable() {
			this.StopAllCoroutines();
		}

		private IEnumerator Fade() {
			yield return new WaitForSeconds(5.0f);
			GameObject.Destroy(this.gameObject);
		}

	}

}