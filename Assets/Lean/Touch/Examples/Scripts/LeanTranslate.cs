using UnityEngine;
using TMPro;

namespace Lean.Touch
{
	/// <summary>This script allows you to translate the current GameObject relative to the camera.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanTranslate")]
	public class LeanTranslate : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
		public Camera Camera;

		[SerializeField] private GameObject TranslationIndicator, SelectedIndicators;
		
		private float positionY;
        private bool axisY = true;
        private bool modeEdition;

        [SerializeField] private TextMeshProUGUI DebugText;

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Start();
		}
#endif

		protected virtual void Start()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponent<LeanSelectable>();
			}
		}

		protected virtual void Update()
		{
            modeEdition = ScaleBehavior.isEditionMode;
            // Get the fingers we want to use
            var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);
           

			// Calculate the screenDelta value based on these fingers
			var screenDelta = LeanGesture.GetScreenDelta(fingers);

			if (screenDelta != Vector2.zero)
			{
				// Perform the translation
				if (transform is RectTransform)
				{
					TranslateUI(screenDelta);
				}
				else
				{
					Translate(screenDelta);
				}
			}
			else
			{
				TranslationIndicator.SetActive(false);
			}
		}

		protected virtual void TranslateUI(Vector2 screenDelta)
		{
			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, transform.position);

			// Add the deltaPosition
			screenPoint += screenDelta;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, Camera, out worldPoint) == true)
			{
				transform.position = worldPoint;
			}
		}

		protected virtual void Translate(Vector2 screenDelta)
		{
            // Make sure the camera exists
            if (modeEdition)
            {
                var camera = LeanTouch.GetCamera(Camera, gameObject);

                if (camera != null)
                {
	                positionY = ScaleBehavior.PositionY;
	                if (positionY.Equals(0))
	                {
		                positionY = ScanBehaviour.PosYHit;
		                //DebugText.SetText("Y: " + positionY);
	                }

	                var screenPoint = camera.WorldToScreenPoint(new Vector3(transform.position.x, positionY, transform.position.z));
                    axisY = false;
                    
                    TranslationIndicator.SetActive(true);
                    SelectedIndicators.SetActive(false);
                    // Add the deltaPosition
                    screenPoint += (Vector3)screenDelta;

                    // Convert back to world space
                    transform.position = camera.ScreenToWorldPoint(screenPoint);
                }
                else
                {
                    Debug.LogError("Failed to find camera. Either tag your camera as MainCamera, or set one in this component.", this);
                }
            }
		}
	}
}