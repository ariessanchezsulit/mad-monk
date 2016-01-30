using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

using System;
using System.Collections;
using System.Collections.Generic;

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

public enum EBubbleColor {
	Invalid		= 0x0,

	Red			= 0x1 << 0,
	Orange		= 0x1 << 1,
	Yellow		= 0x1 << 2,
	Green		= 0x1 << 3,
	Blue		= 0x1 << 4,
	Indigo		= 0x1 << 5,
	Violet		= 0x1 << 6,
	Pink		= 0x1 << 7,

	Max			= 0x1 << 8,
}

namespace Game {

	public interface IBubble {
		EBubbleType BubbleType { get; }
		EBubbleColor BubbleColor { get; }
	};

	public class Bubble : MonoBehaviour, IBubble {

		[SerializeField]
		private EBubbleType bubbleType;

		[SerializeField]
		private EBubbleColor bubbleColor;

		[SerializeField]
		private Image[] colors;

		private Dictionary<EBubbleColor, Image> images;

		private bool popped = false;

		private void Awake() {
			Assertion.Assert(this.colors.Length > 0);
			this.GetComponent<BubbleFade>().enabled = false;
			this.images = new Dictionary<EBubbleColor, Image>();

			// populate bubbles
			for (int i = 0; i < this.colors.Length; i++) {
				this.images.Add((EBubbleColor)(0x1 << i), this.colors[i]);
			}
		}

		private void Start() {
			this.images[this.bubbleColor].gameObject.SetActive(true);
		}

		private void OnEnable() {
			this.bubbleType = (EBubbleType)(0x1 << URandom.Range(0, 7));
			this.bubbleColor = (EBubbleColor)(0x1 << URandom.Range(0, 8));

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

		public EBubbleColor BubbleColor {
			get {
				return this.bubbleColor;
			}
		}
	}

}