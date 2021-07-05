namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /*Proporciona imágenes de descubrimiento de planos que guían a los usuarios a escanear entornos y descubrir planos.
    Esto consiste en una animación de la mano y una barra de estado con instrucciones cortas. Si no se encuentra ningún plano
    después de una cierta cantidad de tiempo, la barra de estados muestra un botón que ofrece abrir una ventana de ayuda con
    instrucciones más detalladas sobre cómo encontrar un plano cuando se presiona.
    public class PlaneDiscoveryGuide : MonoBehaviour
    {
        public float DisplayGuideDelay = 3.0f; // El tiempo de demora, después de que ARCore pierde el seguimiento de cualquier plano, muestra la guía de descubrimiento del plano.
        public float OfferDetailedInstructionsDelay = 8.0f; // El tiempo de demora, después de mostrar la guía de descubrimiento del plano, que ofrece instrucciones más detalladas sobre cómo encontrar un plano
        private const float k_OnStartDelay = 1f; //El tiempo de demora, después de Unity Start, muestra la guía de descubrimiento del plano.
        private const float k_HideGuideDelay = 0.75f; //El tiempo de demora, después de que ARCore rastrea al menos un plano, oculta la guía de descubrimiento del plano.
        private const float k_AnimationFadeDuration = 0.15f; //La duración de la animación de la mano se desvanece.

        [SerializeField] private GameObject m_FeaturePoints = null;// Game Object que muestra los puntos de visualizacion detectados
        [SerializeField] private RawImage m_HandAnimation = null; //El RawImage que proporciona animación de mano giratoria
        [SerializeField] private GameObject m_SnackBar = null; // Alerta en la parte inferior de la pantalla
        [SerializeField] private Text m_SnackBarText = null; // Texto de la alerta SnackBar
        [SerializeField] private GameObject m_OpenButton = null; // Botón para abrir ayuda
        [SerializeField] private GameObject m_MoreHelpWindow = null; // Game Object para visualizar ayuda en pantalla
        [SerializeField] private Button m_GotItButton = null; //Botón para cerrar ayuda

        private float m_DetectedPlaneElapsed; //El tiempo transcurrido que ARCore ha estado detectando al menos un plano
        private float m_NotDetectedPlaneElapsed; //El tiempo transcurrido que ARCore ha estado siguiendo pero no ha detectado ningún plano.
        private bool m_IsLostTrackingDisplayed; //Indica si se muestra un motivo de seguimiento perdido.       
        private List<DetectedPlane> m_DetectedPlanes = new List<DetectedPlane>(); //Una lista para contener los planos detectados que ARCore está rastreando en el cuadro actual

        public void Start()
        {
            m_OpenButton.GetComponent<Button>().onClick.AddListener(_OnOpenButtonClicked);
            m_GotItButton.onClick.AddListener(_OnGotItButtonClicked);

            _CheckFieldsAreNotNull();
            m_MoreHelpWindow.SetActive(false);
            m_IsLostTrackingDisplayed = false;
            m_NotDetectedPlaneElapsed = DisplayGuideDelay - k_OnStartDelay;
        }

        public void OnDestroy()
        {
            m_OpenButton.GetComponent<Button>().onClick.RemoveListener(_OnOpenButtonClicked);
            m_GotItButton.onClick.RemoveListener(_OnGotItButtonClicked);
        }

        public void Update()
        {
            _UpdateDetectedPlaneTrackingState();
            _UpdateUI();
        }

        private void _OnOpenButtonClicked()
        {
            m_MoreHelpWindow.SetActive(true);

            enabled = false;
            m_FeaturePoints.SetActive(false);
            m_HandAnimation.enabled = false;
            m_SnackBar.SetActive(false);
        }

        private void _OnGotItButtonClicked()
        {
            m_MoreHelpWindow.SetActive(false);
            enabled = true;
        }

        private void _UpdateDetectedPlaneTrackingState()
        {
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            Session.GetTrackables<DetectedPlane>(m_DetectedPlanes, TrackableQueryFilter.All);
            foreach (DetectedPlane plane in m_DetectedPlanes)
            {
                if (plane.TrackingState == TrackingState.Tracking)
                {
                    m_DetectedPlaneElapsed += Time.deltaTime;
                    m_NotDetectedPlaneElapsed = 0f;
                    return;
                }
            }

            m_DetectedPlaneElapsed = 0f;
            m_NotDetectedPlaneElapsed += Time.deltaTime;
        }

        private void _UpdateUI()
        {
            if (Session.Status == SessionStatus.LostTracking && Session.LostTrackingReason != LostTrackingReason.None)
            {
                // The session has lost tracking.
                m_FeaturePoints.SetActive(false);
                m_HandAnimation.enabled = false;
                m_SnackBar.SetActive(true);
                switch (Session.LostTrackingReason)
                {
                    case LostTrackingReason.InsufficientLight:
                        m_SnackBarText.text = "Demasiado oscuro. Intenta moverte a un área bien iluminada.";
                        break;
                    case LostTrackingReason.InsufficientFeatures:
                        m_SnackBarText.text = "Apunte el dispositivo a una superficie con más textura o color.";
                        break;
                    case LostTrackingReason.ExcessiveMotion:
                        m_SnackBarText.text = "Moviéndose demasiado rápido.Ve más despacio.";
                        break;
                    default:
                        m_SnackBarText.text = "Se pierde el seguimiento de movimiento.";
                        break;
                }

                m_OpenButton.SetActive(false);
                m_IsLostTrackingDisplayed = true;
                return;
            }
            else if (m_IsLostTrackingDisplayed)
            {
                // The session has moved from the lost tracking state.
                m_SnackBar.SetActive(false);
                m_IsLostTrackingDisplayed = false;
            }

            if (m_NotDetectedPlaneElapsed > DisplayGuideDelay)
            {
                // The session has been tracking but no planes have been found by 'DisplayGuideDelay'.
                m_FeaturePoints.SetActive(true);

                if (!m_HandAnimation.enabled)
                {
                    m_HandAnimation.GetComponent<CanvasRenderer>().SetAlpha(0f);
                    m_HandAnimation.CrossFadeAlpha(1f, k_AnimationFadeDuration, false);
                }

                m_HandAnimation.enabled = true;
                m_SnackBar.SetActive(true);

                if (m_NotDetectedPlaneElapsed > OfferDetailedInstructionsDelay)
                {
                    m_SnackBarText.text = "¿Necesitas ayuda?";
                    m_OpenButton.SetActive(true);
                }
                else
                {
                    m_SnackBarText.text = "Apunte su cámara hacia donde desea colocar un objeto.";
                    m_OpenButton.SetActive(false);
                }
            }
            else if (m_NotDetectedPlaneElapsed > 0f || m_DetectedPlaneElapsed > k_HideGuideDelay)
            {
                // The session is tracking but no planes have been found in less than 'DisplayGuideDelay' or
                // at least one plane has been tracking for more than 'k_HideGuideDelay'.
                m_FeaturePoints.SetActive(false);
                m_SnackBar.SetActive(false);
                m_OpenButton.SetActive(false);

                if (m_HandAnimation.enabled)
                {
                    m_HandAnimation.GetComponent<CanvasRenderer>().SetAlpha(1f);
                    m_HandAnimation.CrossFadeAlpha(0f, k_AnimationFadeDuration, false);
                }

                m_HandAnimation.enabled = false;
            }
        }

        // Reviza que los objetos no sean nulos, y muestra una alerta preventiva.
        private void _CheckFieldsAreNotNull()
        {
            if (m_MoreHelpWindow == null)
            {
                Debug.LogError("MoreHelpWindow is null");
            }

            if (m_GotItButton == null)
            {
                Debug.LogError("GotItButton is null");
            }

            if (m_SnackBarText == null)
            {
                Debug.LogError("SnackBarText is null");
            }

            if (m_SnackBar == null)
            {
                Debug.LogError("SnackBar is null");
            }

            if (m_OpenButton == null)
            {
                Debug.LogError("OpenButton is null");
            }
            else if (m_OpenButton.GetComponent<Button>() == null)
            {
                Debug.LogError("OpenButton does not have a Button Component.");
            }

            if (m_HandAnimation == null)
            {
                Debug.LogError("HandAnimation is null");
            }

            if (m_FeaturePoints == null)
            {
                Debug.LogError("FeaturePoints is null");
            }
        }

        public void onClickAdd()
        {
            ToastMessage("Button add clicked");
        }

        /* Método para mostrar alertas en Android (Toast.makeText) 
        /// <param name="message">Mensaje de tipo String que se desea mostrar en la alerta TOAST</param>
        private void ToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }*/
}
