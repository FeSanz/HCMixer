    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;
    using UnityEngine.XR.ARFoundation;
    
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(VideoPlayer))]
    public class RawImageVideoPlayer : MonoBehaviour
    {
        [SerializeField] private RawImage RawImage;
        [SerializeField] private VideoPlayer VideoPlayer;
        [SerializeField] private ARSession session;
        [SerializeField] private GameObject TextAnim;
        
        private Texture m_RawImageTexture;
        
        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            VideoPlayer.enabled = false;
            m_RawImageTexture = RawImage.texture;
            VideoPlayer.prepareCompleted += _PrepareCompleted;
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            if (!session.isActiveAndEnabled || !session.enabled)
            {
                VideoPlayer.Stop();
                return;
            }

            if (RawImage.enabled && !VideoPlayer.enabled)
            {
                TextAnim.SetActive(true);
                VideoPlayer.enabled = true;
                VideoPlayer.Play();
            }
            else if (!RawImage.enabled && VideoPlayer.enabled)
            {
                // Detiene la animación en video al detectar superficie.
                TextAnim.SetActive(false);
                VideoPlayer.Stop();
                RawImage.texture = m_RawImageTexture;
                VideoPlayer.enabled = false;
            }
        }

        private void _PrepareCompleted(VideoPlayer player)
        {
            RawImage.texture = player.texture;
        }
    }
