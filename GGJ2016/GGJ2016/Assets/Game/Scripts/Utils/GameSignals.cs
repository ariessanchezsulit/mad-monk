using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Common.Signal;

namespace Game {
	
	public abstract class GameSignals {
		
		public static readonly Signal ON_CLICKED_BUTTON = new Signal("OnClickedButton");

		public static readonly Signal ON_BUBBLE_ADDED_TO_POOL = new Signal("OnBubbleAddedToPool");
		public static readonly Signal ON_BUBBLE_REMOVED_TO_POOL = new Signal("OnBubbleRemovedToPool");
	}

}