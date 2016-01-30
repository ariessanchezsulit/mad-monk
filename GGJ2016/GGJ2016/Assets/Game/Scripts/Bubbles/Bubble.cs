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
		
		private bool popped = false;

		private void Awake() {
			this.GetComponent<BubbleFade>().enabled = false;
		}

		private void Start() {
		}

		private void OnEnable() {
			this.bubbleType = (EBubbleType)(0x1 << URandom.Range(0, 7));

			// add to pool
			Signal signal = GameSignals.ON_BUBBLE_ADDED_TO_POOL;
			signal.AddParameter(GameParams.BUBBLE, this.gameObject);
			signal.Dispatch();
		}

		public void Pop() {
			this.popped = true;
			this.GetComponent<BubbleMove>().enabled = false;
			this.GetComponent<BubbleFade>().enabled = true;
		}

		public EBubbleType BubbleType {
			get {
				return this.bubbleType;
			}
		}
	}

}