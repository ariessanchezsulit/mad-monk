using UnityEngine;
using URandom = UnityEngine.Random;

using System;
using System.Collections;

using Common;
using Common.Signal;

namespace Game {

	public class BubbleMove : MonoBehaviour {
		//public const float CAP_Y_POSITION = 400.0f;
		
		[SerializeField]
		private float speed = 200.0f;
		[SerializeField]
		private float frequency = 5.0f;  // Speed of sine movement
		[SerializeField]
		private float magnitude = 100.0f;   // Size of sine movement
		private float capPositionY = 400.0f;

		// scale
		[SerializeField]
		private float scaleDuration;
		private float totalTime;
		private float capScale;

		[SerializeField]
		[Range(0, 1)]
		private int direction = -1;

		private Vector3 axis;
		private Vector3 pos;
		
		[SerializeField]
		private TrailRenderer trail;

		private void Awake() {
			Assertion.AssertNotNull(this.trail);
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
			if (Platform.IsMobileIOS() || Platform.IsMobileAndroid()) {
				float scaler = 4.0f;

				this.speed 			= URandom.Range(600.0f * 1.0f, 1200.0f * 1.0f);
				this.frequency 		= URandom.Range(5.0f * 1.0f, 5.0f * 1.0f);
				this.magnitude 		= URandom.Range(300.0f, 500.0f);
				this.direction 		= URandom.Range(0, 2);
				this.capPositionY 	= 2000.0f;

				/*
				this.speed *= scaler;
				this.frequency *= scaler;
				this.magnitude *= scaler;
				this.capPositionY *= scaler;
				*/
			}
			else {
				this.speed = URandom.Range(100.0f, 500.0f);
				this.frequency = URandom.Range(5.0f, 5.0f);
				this.magnitude = URandom.Range(50.0f, 100.0f);
				this.direction = URandom.Range(0, 2);
				this.capPositionY = 400.0f;
			}


			this.totalTime = 0.0f;
			this.scaleDuration = URandom.Range(0.15f, 0.35f);
			this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			//this.capScale = URandom.Range(0.5f, 1.0f);
			this.capScale = 1.0f;
		}

		private void FixedUpdate() {
			this.Move();
			this.Scale();
		}

		private void LateUpdate() {
			if (this.transform.position.y >= this.capPositionY) {
				// dispatch bubble missed
				Signal signal = GameSignals.ON_BUBBLE_MISSED;
				signal.Dispatch();

				// remove to pool
				signal = GameSignals.ON_BUBBLE_REMOVED_TO_POOL;
				signal.AddParameter(GameParams.BUBBLE, this.gameObject);
				signal.Dispatch();
			}
		}

		private void Move() {
			this.pos += this.transform.up * Time.fixedDeltaTime  * this.speed;

			if (this.direction > 0) {
				this.transform.position = this.pos + this.axis * Mathf.Sin(Time.time * this.frequency) * this.magnitude;
			}
			else {
				this.transform.position = this.pos + this.axis * -Mathf.Sin(Time.time * this.frequency) * this.magnitude;
			}
		}

		private void Scale() {
			this.totalTime += Time.fixedDeltaTime;
			float scale = this.totalTime / this.scaleDuration;

			// max
			if (scale >= this.capScale) {
				scale = this.capScale;
			}

			this.transform.localScale = new Vector3(scale, scale, scale);

			// scale trail
			float maxTrailSize = 30.0f;
			scale = this.totalTime / maxTrailSize;

			// max
			if (scale > maxTrailSize) {
				scale = maxTrailSize;
			}

			this.trail.startWidth = maxTrailSize;
		}
	}

}