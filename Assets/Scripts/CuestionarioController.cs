using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CuestionarioController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtStatus;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject panelQuestions;
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private GameObject answerPrefab;


    public static string userEmail = "carlos.tapia.condor@gmail.com";
    public static string userPassword;

    private UnityWebRequest webRequest;
    private string rootPath = "http://localhost:5000/restservice-89269/us-central1/app/api/";

    public void showQuestions(List<Pregunta> preguntas)
    {
        GameObject panel = Instantiate(panelQuestions, content);

        Pregunta pregunta = preguntas[0];
            
        GameObject box = Instantiate(questionPrefab, panel.transform);
        box.GetComponent<TextMeshProUGUI>().text = pregunta.pregunta;
        GameObject box1 = Instantiate(answerPrefab, panel.transform);
        box1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r1;
        GameObject box2 = Instantiate(answerPrefab, panel.transform);
        box2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r2;
        GameObject box3 = Instantiate(answerPrefab, panel.transform);
        box3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r3;
        GameObject box4 = Instantiate(answerPrefab, panel.transform);
        box4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r4;
        
    }



    #region CarlosApi Rest FireBase
    public void GetCuestionario()
    {
        StartCoroutine(GetRequestCuestionario());
    }

    private IEnumerator GetRequestCuestionario()
    {
        string path = "cuestionario/01";
        webRequest = UnityWebRequest.Get(rootPath + path);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError && webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");

        }
        else
        {
            print(webRequest.downloadHandler.text);
            GetCuestionarioData deserializeJson = JsonUtility.FromJson<GetCuestionarioData>(webRequest.downloadHandler.text);
            showQuestions(deserializeJson.preguntas);
        }
    }

    public void GetStatusCuestionario()
    {
        StartCoroutine(GetRequestCuestionarioStatus());
    }

    private IEnumerator GetRequestCuestionarioStatus()
    {
        string path = "cuestionario_status/01";
        webRequest = UnityWebRequest.Get(rootPath + path);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        GetSetCuestionarioStatus data = new GetSetCuestionarioStatus();
        data.user = userEmail;

        string jsonData = JsonUtility.ToJson(data);

        if (jsonData != null)
        {
            byte[] datos = System.Text.Encoding.UTF8.GetBytes(jsonData);
            UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
            upHandler.contentType = "application/json";
            webRequest.uploadHandler = upHandler;
        }

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError && webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
            txtStatus.text = "No se ha presentado este Cuestionario";

        }
        else
        {
            print(webRequest.downloadHandler.text);
            GetSetCuestionarioStatus deserializeJson = JsonUtility.FromJson<GetSetCuestionarioStatus>(webRequest.downloadHandler.text);

            txtStatus.text = "Usuario : " + deserializeJson.user + "\n" +
                "Tiempo : " + deserializeJson.tiempo.ToString() + "\n" +
                "Completado : " + deserializeJson.completo.ToString() + "\n" +
                "Calificacion : " + deserializeJson.calificacion.ToString() + "\n" +
                "Intentos : " + deserializeJson.intentos.ToString() + "\n" +
                "Pasos completos : " + deserializeJson.pasos_completos.ToString();



            print(deserializeJson);
        }
    }

    public void PostStatusCuestionario()
    {
        StartCoroutine(PostCuestionarioStatus());
    }

    private IEnumerator PostCuestionarioStatus()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = "01";
        myObject.tiempo = Random.Range(0, 1000);
        myObject.completo = true;
        myObject.calificacion = 90;
        myObject.intentos = 1;
        myObject.pasos_completos = 4;

        string serializeJson = JsonUtility.ToJson(myObject);



        using (webRequest = UnityWebRequest.Post(rootPath + path, serializeJson))
        {

            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");


            if (serializeJson != null)
            {
                byte[] datos = System.Text.Encoding.UTF8.GetBytes(serializeJson);
                UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
                upHandler.contentType = "application/json";
                webRequest.uploadHandler = upHandler;
            }

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " + webRequest.downloadHandler.text);
            }
            else
            {
                print("Registro exitoso");
                print(webRequest.downloadHandler.text);
            }
        }
    }

    public void PutStatusCuestionario()
    {
        StartCoroutine(PutCuestionarioStatus());
    }

    private IEnumerator PutCuestionarioStatus()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = "01";
        myObject.tiempo = Random.Range(0, 1000);
        myObject.completo = true;
        myObject.calificacion = 10;
        myObject.intentos = 1;
        myObject.pasos_completos = 2;

        string serializeJson = JsonUtility.ToJson(myObject);



        using (webRequest = UnityWebRequest.Put(rootPath + path, serializeJson))
        {

            webRequest.method = UnityWebRequest.kHttpVerbPUT;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");


            if (serializeJson != null)
            {
                byte[] datos = System.Text.Encoding.UTF8.GetBytes(serializeJson);
                UploadHandlerRaw upHandler = new UploadHandlerRaw(datos);
                upHandler.contentType = "application/json";
                webRequest.uploadHandler = upHandler;
            }

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                print("Error." + webRequest.error + ", " + webRequest.downloadHandler.text);
            }
            else
            {
                print("Registro exitoso");
                print(webRequest.downloadHandler.text);
            }
        }
    }
    #endregion
}

public class User
{
    public string user;
    public string password;
    public string mensaje;

}


[System.Serializable]
public class GetCuestionarioData
{
    public string name;
    public int no_preguntas;
    public string id;
    public List<Pregunta> preguntas;

}

[System.Serializable]
public class Pregunta { 

    public string pregunta;
    public string r1;
    public string r2;
    public string r3;
    public string r4;

}

[System.Serializable]
public class GetSetCuestionarioStatus
{
    public string user;
    public int tiempo;
    public bool completo;
    public float calificacion;
    public int intentos;
    public int pasos_completos;


}
