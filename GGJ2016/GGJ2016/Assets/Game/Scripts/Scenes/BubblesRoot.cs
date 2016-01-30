using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

using System;
using System.Collections;
using System.Collections.Generic;

using Common;
using Common.Signal;

namespace Game {

	public class BubblesRoot : GameScene {

		[SerializeField]
		private Pool pool;
		
		[SerializeField]
		private float interval = 1.5f;

		// probability
		[SerializeField]
		[Range(1, 100)]
		private int[] numberOfBubbles;

		protected override void Awake() {
			base.Awake();
		}

		private void Start() {
			this.StartCoroutine(this.GenerateBubble());
		}

		private IEnumerator GenerateBubble()  {
			yield return new WaitForSeconds(this.interval);

			while (true) {
				// random number for bubbles
				int random = URandom.Range(1, 4);

				// TODO: Pool the bubbles here
				Transform template = this.pool.Tempalte().transform;

				// difficulty
				int difficulty = this.CalculateProbability();

				for (int i = 0; i <= difficulty; i++) {
					GameObject bubble = this.pool.Get();
					bubble.transform.SetParent(template.parent);
					bubble.transform.localPosition = template.localPosition;
					bubble.transform.localScale = template.localScale;
					bubble.SetActive(true);
				}

				yield return new WaitForSeconds(this.interval);
			}
		}

		private int CalculateProbability() {
			int initialValue = 0;
			int len = this.numberOfBubbles.Length;
			int random = URandom.Range(1, this.SumOfDifficulties());

			for (int i = 0; i < len; i++) {
				initialValue += this.numberOfBubbles[i];
				if (random <= initialValue && random >= 1) {
					return i;
				}
			}

			Assertion.Assert(false);
			return -1;
		}

		private int SumOfDifficulties() {
			int totalValue = 0;
			foreach (int value in this.numberOfBubbles) {
				totalValue += value;
			}

			return totalValue;
		}
	}

}