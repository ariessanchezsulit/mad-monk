using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Common.Signal;

namespace Game {
	
	public abstract class GameSignals {
		
		public static readonly Signal ON_CLICKED_BUTTON = new Signal("OnClickedButton");

	}

}