using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Common.Signal;

namespace Game {
	
	public abstract class GameSignals {
		
		public static readonly Signal ON_CLICKED_BUTTON = new Signal("OnClickedButton");
		public static readonly Signal ON_BUBBLE_POPPED = new Signal("OnBubblePopped");
		public static readonly Signal ON_BUBBLE_MISSED = new Signal("OnBubbleMissed");
		public static readonly Signal ON_MONSTER_DEAD = new Signal("OnMonsterDead");
		public static readonly Signal START_GAME = new Signal("StartGame");
		public static readonly Signal END_GAME = new Signal("EndGame");
	}

}