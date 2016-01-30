using UnityEngine;
using System.Collections;

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
            OnGameStarted();
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

        void OnGameStarted()
        {
            InitializeRecognizers();
        }

        void OnSwipeRecognized(TKSwipeRecognizer r)
        {
            Debug.Log("swipe recognized");
        }

        void OnTapRecognized(TKTapRecognizer r)
        {
            Debug.Log("tap recognized");
        }

        void OnLongTapRecognized(TKLongPressRecognizer r)
        {
            Debug.Log("long press started");
        }

        void OnLongTapFinished(TKLongPressRecognizer r)
        {
            Debug.Log("long press finished");
        }

        void OnPinchRecognized(TKPinchRecognizer r)
        {
            Debug.Log("pinch recognized");
        }
    }
}