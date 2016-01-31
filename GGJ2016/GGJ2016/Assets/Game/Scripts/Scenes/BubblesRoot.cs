﻿using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

using System;
using System.Collections;
using System.Collections.Generic;

using Common;
using Common.Signal;

namespace Game {

	public class BubblesRoot : MonoBehaviour {

		[SerializeField]
		private Pool pool;
		
		[SerializeField]
		private float interval = 2.0f;

		private float totalTime;

		[SerializeField]
		private int levelInterval = 10;

		[SerializeField]
		private int totalTimeInt;

		[SerializeField]
		private int level;

		// probability
		[SerializeField]
		[Range(1, 100)]
		private int[] numberOfBubbles;

		private bool started = false;

		[SerializeField]
		private List<Bubble> bubbles;

		private void Awake() {
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

			GameSignals.INPUT_GENERIC.AddListener((ISignalParameters parameters) => ProcessInput(parameters, true));

			GameSignals.INPUT_NETWORK.AddListener ((ISignalParameters parameters) => ProcessInput(parameters, false));
		}

		private void ProcessInput(ISignalParameters parameters, bool isLocal)
		{
			GestureType gesture = (GestureType)parameters.GetParameter(GameParams.INPUT_TYPE);
			if (gesture == GestureType.SWIPE) {
				SwipePayload swipePayload = (SwipePayload)parameters.GetParameter(GameParams.INPUT_SWIPE_PAYLOAD);
				this.PopBubble(gesture, swipePayload.direction);
			}
			else {
				this.PopBubble(gesture);
			}
		}

		private void LateUpdate() {
			//	2.5
			//	Bubbles[0] = 100
			// -
			//	2
			//	Bubbles[0] = 100
			//	Bubbles[1] = 5
			// -
			//	1.75
			//  Bubbles[0] = 100
			//	Bubbles[1] = 10
			// -
			//	1.50
			//	Bubbles[0] = 100
			//	Bubbles[1] = 20
			// -
			//	1.25
			//	Bubbles[0] = 100
			//	Bubbles[1] = 30
			//	Bubbles[2] = 5
			// -
			//	1.0
			//	Bubbles[0] = 50
			//	Bubbles[1] = 50
			//	Bubbles[2] = 10

			if (!this.started) {
				return;
			}

			this.totalTime += Time.deltaTime;
			this.totalTimeInt = Mathf.RoundToInt(this.totalTime);

			// interval every 20 sec
			this.level = (this.totalTimeInt - (this.totalTimeInt % this.levelInterval)) / this.levelInterval;
			if (this.level >= 6) {
				this.AdjustLevel(1.0f, 50, 50, 10);
			}
			else if (this.level >= 5) {
				this.AdjustLevel(1.0f, 100, 30, 5);
			}
			else if (this.level >= 4) {
				this.AdjustLevel(1.25f, 100, 20);
			}
			else if (this.level >= 3) {
				this.AdjustLevel(1.25f, 100, 20);
			}
			else if (this.level >= 2) {
				this.AdjustLevel(1.5f, 100, 10);
			}
			else if (this.level >= 1) {
				this.AdjustLevel(1.5f, 100, 5);
			}
			else {
				this.AdjustLevel(2.0f, 100);
			}
		}

		private void AdjustLevel(float interval, params int[] values) {
			if (this.numberOfBubbles.Length != values.Length) {
				this.numberOfBubbles = new int[values.Length];
			}

			// set interval
			this.interval = interval;

			// set progression
			for (int i = 0; i < values.Length; i++) {
				this.numberOfBubbles[i] = values[i];
			}
		}

		private void OnStartGame(ISignalParameters @params) {
			this.StopAllCoroutines ();
			this.StartCoroutine(this.GenerateBubble());
			this.started = true;
			this.totalTime = 0.0f;
			this.totalTimeInt = 0;
			this.level = 0;
		}

		private void OnEndGame(ISignalParameters @params) {
			this.StopAllCoroutines ();
			this.started = false;
			this.totalTime = 0.0f;
			this.totalTimeInt = 0;
			this.level = 0;
		}

		private void PopBubble(GestureType type) {
			switch (type) {
			case GestureType.LONG_PRESS:
				this.PopBubble(EBubbleType.LongPress);
				break;
			case GestureType.PINCH:
				this.PopBubble(EBubbleType.Pinch);
				break;
			case GestureType.TAP:
				this.PopBubble(EBubbleType.Tap);
				break;
			}
		}

		private void PopBubble(GestureType type, TKSwipeDirection direction) {
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

					yield return new WaitForSeconds(URandom.Range(0.15f, 0.5f));
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