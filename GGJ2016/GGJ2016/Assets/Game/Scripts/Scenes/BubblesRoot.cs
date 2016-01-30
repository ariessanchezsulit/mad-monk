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

		[SerializeField]
		private List<Bubble> bubbles;

		protected override void Awake() {
			base.Awake();

			GameSignals.START_GAME.AddListener (OnStartGame);
			GameSignals.END_GAME.AddListener (OnEndGame);

			GameSignals.ON_BUBBLE_ADDED_TO_POOL.AddListener((ISignalParameters parameters) => {
				//this.pool.Add((GameObject)parameters.GetParameter(GameParams.BUBBLE));
				GameObject obj = (GameObject)parameters.GetParameter(GameParams.BUBBLE);
				Bubble bubble = obj.GetComponent<Bubble>();
				this.bubbles.Add(bubble);
			});

			GameSignals.ON_BUBBLE_REMOVED_TO_POOL.AddListener((ISignalParameters parameters) => {
				//this.pool.Remove((GameObject)parameters.GetParameter(GameParams.BUBBLE));
				GameObject obj = (GameObject)parameters.GetParameter(GameParams.BUBBLE);
				Bubble bubble = obj.GetComponent<Bubble>();
				this.bubbles.Remove(bubble);
				GameObject.Destroy(obj);
			});

			GameSignals.INPUT_TAP.AddListener((ISignalParameters parameters) => {
				//Debug.LogFormat("Tap\n");
				this.PopBubble(EBubbleType.Tap);
			});

			GameSignals.INPUT_SWIPE.AddListener((ISignalParameters parameters) => {
				TKSwipeDirection direction = (TKSwipeDirection)parameters.GetParameter(GameParams.INPUT_SWIPE_DIR);
				//Debug.LogFormat("Swipe {0}\n", direction);
				switch (direction) {
				case TKSwipeDirection.Up:
					this.PopBubble(EBubbleType.SwipeUp);
					break;
				case TKSwipeDirection.Down:
					this.PopBubble(EBubbleType.SwipeDown);
					break;
				case TKSwipeDirection.Left:
					this.PopBubble(EBubbleType.SwipeLeft);
					break;
				case TKSwipeDirection.Right:
					this.PopBubble(EBubbleType.SwipeRight);
					break;
				}
			});

			GameSignals.INPUT_PINCH.AddListener((ISignalParameters parameters) => {
				//Debug.LogFormat("Pinch\n");
				this.PopBubble(EBubbleType.Pinch);
			});

			GameSignals.INPUT_LONG_PRESS.AddListener((ISignalParameters parameters) => {
				//Debug.LogFormat("LongPress\n");
				this.PopBubble(EBubbleType.LongPress);
			});
		}

		private void OnStartGame(ISignalParameters @params) {
			this.StartCoroutine(this.GenerateBubble());
		}

		private void OnEndGame(ISignalParameters @params)
		{
			this.StopAllCoroutines ();
		}

		private void PopBubble(EBubbleType type) {
			if (this.bubbles.Exists(b => b.BubbleType == type)) {
				Bubble bubble = this.bubbles.Find(b => b.BubbleType == type);
				bubble.Pop();

				// remove to bubbles
				this.bubbles.Remove(bubble);
			}
		}

		private IEnumerator GenerateBubble()  {
			yield return new WaitForSeconds(this.interval);

			while (true) {
				// random number for bubbles
				int random = URandom.Range(1, 4);

				// TODO: Pool the bubbles here
				Transform template = this.pool.Tempalte().transform;

				// difficulty
				int numberOfBubbles = this.CalculateProbability();

				for (int i = 0; i <= numberOfBubbles; i++) {
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