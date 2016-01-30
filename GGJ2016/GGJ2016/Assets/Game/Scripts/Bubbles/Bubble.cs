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

		[SerializeField]
		private EBubbleType bubbleType;

		// fade
		[SerializeField]
		private float fadeDuration = 2.0f;

		private bool popped = false;

		private void Awake() {
		}

		private void Start() {
			this.StartCoroutine(this.Fade());
		}

		private void OnEnable() {
			this.bubbleType = (EBubbleType)(0x1 << URandom.Range(0, 7));

			// add to pool
			Signal signal = GameSignals.ON_BUBBLE_ADDED_TO_POOL;
			signal.AddParameter(GameParams.BUBBLE, this.gameObject);
			signal.Dispatch();
		}

		private IEnumerator Fade() {
			while (!this.popped) {
				yield return null;
			}

			yield return new WaitForSeconds(5.0f);
			GameObject.Destroy(this.gameObject);
		}

		public void Pop() {
			this.popped = true;
			this.GetComponent<BubbleMove>().enabled = false;
		}

		public EBubbleType BubbleType {
			get {
				return this.bubbleType;
			}
		}
	}

}