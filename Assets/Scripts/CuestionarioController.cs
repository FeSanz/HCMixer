using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CuestionarioController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtStatus;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject panelQuestions;
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject btnGetCuestionario;
    [SerializeField] private GameObject statusWindow;




    public static string userEmail = "carlos.tapia.condor@gmail.com";
    public static string userPassword;

    private UnityWebRequest webRequest;
    private string rootPath = "http://localhost:5000/restservice-89269/us-central1/app/api/";

    private List<Pregunta> listaPreguntas;
    public int intento;
    private float timeBegin;
     
    public float calculateCalif() {
        int suma = 0;
        
        foreach (Pregunta pregunta in listaPreguntas) {
            if (pregunta.toggles[0].isOn)
            {
                suma += 1;
            }
        }
        Debug.Log(((float)suma) / ((float)listaPreguntas.Count));
        return 10f * ((float)suma ) / ((float) listaPreguntas.Count);
    }

    private void disableToggle(Pregunta pregunta, int position, int questionNumber) {


            foreach (Toggle toggle in pregunta.toggles)
            {
                if (pregunta.toggles.IndexOf(toggle) != position)
                {
                    toggle.isOn = false;
                }

            }
      

    }
    public void showQuestions(List<Pregunta> preguntas)
    {

        content.GetComponent<RectTransform>().offsetMax = new Vector2((preguntas.Count -1) * 250, 0);
        scrollbar.numberOfSteps = preguntas.Count;

        foreach (Pregunta pregunta in preguntas) {

            GameObject panel = Instantiate(panelQuestions, content);


            GameObject box = Instantiate(questionPrefab, panel.transform);
            box.GetComponent<TextMeshProUGUI>().text = pregunta.pregunta;
            
            GameObject box1 = Instantiate(answerPrefab, panel.transform);
            box1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r1;
            Toggle toggle1 = box1.transform.GetComponentInChildren<Toggle>();
            pregunta.toggles.Add(toggle1);

            GameObject box2 = Instantiate(answerPrefab, panel.transform);
            box2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r2;
            Toggle toggle2 = box2.transform.GetComponentInChildren<Toggle>();
            pregunta.toggles.Add(toggle2);

            GameObject box3 = Instantiate(answerPrefab, panel.transform);
            box3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r3;
            Toggle toggle3 = box3.transform.GetComponentInChildren<Toggle>();
            pregunta.toggles.Add(toggle3);

            GameObject box4 = Instantiate(answerPrefab, panel.transform);
            box4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pregunta.r4;
            Toggle toggle4 = box4.transform.GetComponentInChildren<Toggle>();
            pregunta.toggles.Add(toggle4);


            box1.transform.SetSiblingIndex(Random.Range(1, 5));

            foreach (Toggle toggle in pregunta.toggles)
            {
                toggle.onValueChanged.AddListener(delegate {

                    disableToggle(pregunta,pregunta.toggles.IndexOf(toggle), preguntas.IndexOf(pregunta));
                });
            }


        }

        
    }



    #region CarlosApi Rest FireBase
    public void GetStatusCuestionario()
    {
        StartCoroutine(GetRequestCuestionarioStatus());
    }
    public void GetCuestionario()
    {
        StartCoroutine(GetRequestCuestionario());
    }

    public void PostStatusCuestionario()
    {
        StartCoroutine(PostCuestionarioStatus());
    }
    public void PostIntentos()
    {
        StartCoroutine(PostRequestIntentos());
    }
    public void PutStatusCuestionario()
    {
        StartCoroutine(PutCuestionarioStatus());
    }
    public void PutStatusCuestionarioIntentos()
    {
        StartCoroutine(PutCuestionarioStatusIntentos());
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

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Ocurrio un error");
            txtStatus.text = "No se ha presentado este Cuestionario";
            btnGetCuestionario.SetActive(true);


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
                "Pasos completos : " + deserializeJson.pasos_completos.ToString() + "\n";

            intento = deserializeJson.intentos;

            if (deserializeJson.intentos < 3)
            {
                btnGetCuestionario.SetActive(true);
            }
            else
            {
                btnGetCuestionario.SetActive(false);
                txtStatus.text += "Se han superado los intentos de realizar el examen";
            }

            print(deserializeJson);
        }
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
            listaPreguntas = deserializeJson.preguntas;
            showQuestions(listaPreguntas);
            intento += 1;
            if(intento >1)
            {
                PutStatusCuestionarioIntentos();
            }
            else
            {
                PostIntentos();
            }

            timeBegin = Time.time;
        }
    }

    private IEnumerator PostCuestionarioStatus()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = userEmail;
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

 

    private IEnumerator PostRequestIntentos() {
        
        string path = "cuestionario_status/intentos/01";
        
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = userEmail;
        myObject.intentos = intento;

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

    private IEnumerator PutCuestionarioStatusIntentos()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = userEmail;
        myObject.tiempo = (int)(Time.time - timeBegin);
        myObject.completo = true;
        myObject.calificacion = calculateCalif();
        myObject.intentos = intento;
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

    private IEnumerator PutCuestionarioStatus()
    {

        string path = "cuestionario_status/01";
        GetSetCuestionarioStatus myObject = new GetSetCuestionarioStatus();
        myObject.user = userEmail;
        myObject.tiempo = (int) (Time.time - timeBegin);
        myObject.completo = true;
        myObject.calificacion = calculateCalif();
        myObject.intentos = intento;
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
                content.gameObject.SetActive(false);
                GetStatusCuestionario();
                statusWindow.SetActive(true);
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
    public List<Toggle> toggles;


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
