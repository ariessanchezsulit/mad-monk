using UnityEngine;
using System.Collections;
using Common.Signal;

namespace Game
{
    public class GestureManager : MonoBehaviour
    {
        TKSwipeRecognizer swipeRecognizer;
        TKTapRecognizer tapRecognizer;
        TKPinchRecognizer pinchRecognizer;
        TKLongPressRecognizer longPressRecognizer;

        private void Start()
        {
            // for testing
            OnGameStarted(null);
        }

        void InitializeRecognizers()
        {
            swipeRecognizer = new TKSwipeRecognizer();
            swipeRecognizer.gestureRecognizedEvent += OnSwipeRecognized;
            TouchKit.addGestureRecognizer(swipeRecognizer);

            tapRecognizer = new TKTapRecognizer();
            tapRecognizer.gestureRecognizedEvent += OnTapRecognized;
            TouchKit.addGestureRecognizer(tapRecognizer);
            
            pinchRecognizer = new TKPinchRecognizer();
            pinchRecognizer.gestureRecognizedEvent += OnPinchRecognized;
            TouchKit.addGestureRecognizer(pinchRecognizer);

            longPressRecognizer = new TKLongPressRecognizer();
            longPressRecognizer.gestureRecognizedEvent += OnLongTapRecognized;
            longPressRecognizer.gestureCompleteEvent += OnLongTapFinished;
            TouchKit.addGestureRecognizer(longPressRecognizer);

            Debug.Log("recognizers successfully initialized");
        }

        void DisableRecognizers()
        {
            TouchKit.removeAllGestureRecognizers();
        }

        void OnGameStarted(ISignalParameters parameters)
        {
            InitializeRecognizers();
        }

        void OnGameEnded(ISignalParameters parameters)
        {
            DisableRecognizers();
        }

        void OnSwipeRecognized(TKSwipeRecognizer r)
        {
            Debug.Log("swipe recognized");

            var signal = GameSignals.INPUT_SWIPE;
            signal.AddParameter(GameParams.INPUT_SWIPE_DIR, r.completedSwipeDirection);
            signal.Dispatch();
            signal.ClearParameters();
        }

        void OnTapRecognized(TKTapRecognizer r)
        {
            Debug.Log("tap recognized");

            GameSignals.INPUT_TAP.Dispatch();
        }

        void OnLongTapRecognized(TKLongPressRecognizer r)
        {
            Debug.Log("long press started");
        }

        void OnLongTapFinished(TKLongPressRecognizer r)
        {
            Debug.Log("long press finished");

            GameSignals.INPUT_LONG_PRESS.Dispatch();
        }

        void OnPinchRecognized(TKPinchRecognizer r)
        {
            Debug.Log("pinch recognized");

            GameSignals.INPUT_PINCH.Dispatch();
        }
    }
}