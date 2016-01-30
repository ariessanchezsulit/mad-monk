using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Common;
using Common.Signal;

public enum EButtonType {
	Invalid,


};
	
namespace Game {
	
	public class Button : MonoBehaviour {

		[SerializeField]
		private EButtonType button;

		private void Awake() {
			Assertion.Assert(this.button != EButtonType.Invalid);
		}

		public void OnClickedButton() {
			Signal signal = GameSignals.ON_CLICKED_BUTTON;
			signal.ClearParameters();
			signal.AddParameter(GameParams.BUTTON_TYPE, this.button);
			signal.Dispatch();
		}

		public void OnClickedButton(object data) {
			Signal signal = GameSignals.ON_CLICKED_BUTTON;
			signal.ClearParameters();
			signal.AddParameter(GameParams.BUTTON_TYPE, this.button);
			signal.AddParameter(GameParams.BUTTON_DATA, data);
			signal.Dispatch();
		}
	}

}