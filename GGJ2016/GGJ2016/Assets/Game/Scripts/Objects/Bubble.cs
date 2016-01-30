﻿using UnityEngine;
using URandom = UnityEngine.Random;

using System;
using System.Collections;

using Common;
using Common.Signal;

public enum EBubbleType {
	Red,
	Orange,
	Yellow,
	Green,
};

namespace Game {

	public interface IBubble {
		EBubbleType BubbleType { get; }
	};

	public class Bubble : MonoBehaviour, IBubble {
		public const float CAP_Y_POSITION = 400.0f;

		[SerializeField]
		private EBubbleType bubbleType;

		[SerializeField]
		private float speed = 200.0f;
		[SerializeField]
		private float frequency = 5.0f;  // Speed of sine movement
		[SerializeField]
		private float magnitude = 100.0f;   // Size of sine movement

		// scale
		[SerializeField]
		private float scaleDuration;
		private float totalTime;

		[SerializeField]
		[Range(0, 1)]
		private int direction = -1;

		private Vector3 axis;
		private Vector3 pos;

		private void Awake() {
			this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}

		private void Start() {
			this.pos = this.transform.position;
			this.axis = this.transform.right;
			this.Move();
			this.Scale();
		}

		private void OnEnable() {
			// random values
			this.speed = URandom.Range(100.0f, 500.0f);
			this.frequency = URandom.Range(5.0f, 5.0f);
			this.magnitude = URandom.Range(50.0f, 100.0f);
			this.direction = URandom.Range(0, 2);
			//float ranScale = URandom.Range(1.0f, 2.5f);
			//this.transform.localScale = new Vector3(ranScale, ranScale, ranScale);
			this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

			this.totalTime = 0.0f;
			this.scaleDuration = URandom.Range(0.15f, 0.35f);

			// add to pool
			//Signal signal = GameSignals.ON_BUBBLE_ADDED_TO_POOL;
			//signal.AddParameter(GameParams.BUBBLE, this.gameObject);
			//signal.Dispatch();
		}

		private void FixedUpdate() {
			this.Move();
			this.Scale();
		}

		private void LateUpdate() {
			//Debug.LogFormat("Bubble::LateUpdate Y:{0}\n", this.transform.position.y);
			if (this.transform.position.y >= CAP_Y_POSITION) {
				// pop here
				GameObject.Destroy(this.gameObject);

				// remove to pool
				//Signal signal = GameSignals.ON_BUBBLE_REMOVED_TO_POOL;
				//signal.AddParameter(GameParams.BUBBLE, this.gameObject);
				//signal.Dispatch();
			}
		}

		private void OnDestroy() {
		}

		private void Move() {
			this.pos += this.transform.up * Time.fixedDeltaTime * 0.75f * this.speed;

			if (this.direction > 0) {
				this.transform.position = this.pos + this.axis * Mathf.Sin(Time.time * this.frequency) * this.magnitude;
			}
			else {
				this.transform.position = this.pos + this.axis * -Mathf.Sin(Time.time * this.frequency) * this.magnitude;
			}
		}

		private void Scale() {
			//this.transform.localScale = new Vector3(ranScale, ranScale, ranScale);
			this.totalTime += Time.fixedDeltaTime;
			float scale = this.totalTime / this.scaleDuration;

			// max
			if (scale > 2.5f) {
				scale = 2.5f;
			}

			this.transform.localScale = new Vector3(scale, scale, scale);
		}

		public EBubbleType BubbleType {
			get {
				return this.bubbleType;
			}
		}
	}

}