using UnityEngine;
using URandom = UnityEngine.Random;

using System;
using System.Collections;

using Common;
using Common.Signal;

public enum EBubbleType {
	Invalid		= 0x0,

	SwipeUp		= 0x1 << 0,
	SwipeDown	= 0x1 << 1,
	SwipeLeft	= 0x1 << 2,
	SwipeRight	= 0x1 << 3,

	Tap			= 0x1 << 4,
	Pinch		= 0x1 << 5,
	LongPress	= 0x1 << 6,

	Max			= 0x1 << 7,
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

		// fade
		[SerializeField]
		private float fadeDuration = 2.0f;

		[SerializeField]
		[Range(0, 1)]
		private int direction = -1;

		private Vector3 axis;
		private Vector3 pos;

		private bool popped = false;

		private void Awake() {
			this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}

		private void Start() {
			this.pos = this.transform.position;
			this.axis = this.transform.right;
			this.Move();
			this.Scale();
			this.StartCoroutine(this.Fade());
		}

		private void OnEnable() {
			// random values
			this.speed = URandom.Range(100.0f, 500.0f);
			this.frequency = URandom.Range(5.0f, 5.0f);
			this.magnitude = URandom.Range(50.0f, 100.0f);
			this.direction = URandom.Range(0, 2);
			
			this.totalTime = 0.0f;
			this.scaleDuration = URandom.Range(0.15f, 0.35f);
			this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

			this.bubbleType = (EBubbleType)(0x1 << URandom.Range(0, 7));

			// add to pool
			Signal signal = GameSignals.ON_BUBBLE_ADDED_TO_POOL;
			signal.AddParameter(GameParams.BUBBLE, this.gameObject);
			signal.Dispatch();
		}

		private void FixedUpdate() {
			if (!this.popped) {
				this.Move();
				this.Scale();
			}
			//else {
			//	this.Fade();
			//}
		}

		private void LateUpdate() {
			//Debug.LogFormat("Bubble::LateUpdate Y:{0}\n", this.transform.position.y);
			if (this.transform.position.y >= CAP_Y_POSITION) {
				// pop here
				//GameObject.Destroy(this.gameObject);

				// remove to pool
				Signal signal = GameSignals.ON_BUBBLE_REMOVED_TO_POOL;
				signal.AddParameter(GameParams.BUBBLE, this.gameObject);
				signal.Dispatch();
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

		private IEnumerator Fade() {
			//this.totalTime += Time.fixedDeltaTime;
			//float fade = (this.totalTime / this.fadeDuration) - this.fadeDuration;
			//Debug.LogFormat("Fade... {0}\n", fade);
			while (!this.popped) {
				yield return null;
			}

			yield return new WaitForSeconds(this.fadeDuration);
			GameObject.Destroy(this.gameObject);
		}

		public void Pop() {
			this.popped = true;
			this.totalTime = 0;
		}

		public EBubbleType BubbleType {
			get {
				return this.bubbleType;
			}
		}
	}

}