using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private GameObject loadPanel;

    private UnityWebRequest webRequest;
    private string rootPath = "https://us-central1-restservice-89269.cloudfunctions.net/app/";
    // Start is called before the first frame update
    #region Login

    public void LogIn()
    {
        CuestionarioController.userEmail = emailInput.text;
        CuestionarioController.userPassword = passwordInput.text;

        loadPanel.SetActive(true);
        StartCoroutine(GetLoginRequest());
    }

    private IEnumerator GetLoginRequest()
    {
        string path = "api/login/" + "?user=" + CuestionarioController.userEmail + "&password=" + CuestionarioController.userPassword;
        webRequest = UnityWebRequest.Get(rootPath + path);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        //User data = new User();
        //data.user = CuestionarioController.userEmail;
        //data.password = CuestionarioController.userPassword;



        //string jsonData = JsonUtility.ToJson(data);

        //if (jsonData != null)
        //{
        //    byte[] datos = System.Text.Encoding.UTF8.GetBytes(jsonData);
        //    UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
        //    upHandler.contentType = "application/json";
        //    webRequest.uploadHandler = upHandler;
        //}
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
            loadPanel.SetActive(false);
        }
        else
        {
            print(webRequest.downloadHandler.text);
            User deserializeJson = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
            print(deserializeJson);
            loadPanel.SetActive(false);
            SceneManager.LoadScene("ARMixerScene");



        }

    }

    #endregion
}
