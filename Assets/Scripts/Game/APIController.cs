using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

class SearchUserItemResponse {
    public string collectionId;
    public string collectionName;
    public string id;
    public string username;
    public string created;
    public string updated; 
}
class SearchUserResponse {
    public int page;
    public int perPage;
    public int totalItems;
    public int totalPages;
    public SearchUserItemResponse [] items;
}

class CreateUserRequest {
    public string username;
}

class CreateUserResponse {
    public string collectionId;
    public string collectionName;
    public string id;
    public string username;
    public string created;
    public string updated; 
}

class SaveRecordRequest {
    public string tester;
    public string session_id;
    public string emocion;
    public int nodos_guardados;
    public float brillo_calculado;
}

public class APIController {

    const string API_URL = "https://black-forest-8259.fly.dev/api/collections";
    const string Token = "5zQH59JcpiZJfgmPb9K0yKnP5JQT0y";
    public void SearchUser(string username) {
        GameController.instance.IniciarRutina(SearchUserCoroutine(username));
    }

    private IEnumerator SearchUserCoroutine(string username) {
        string url = $"{API_URL}/tester/records" + $"?filter=(username=\"{username}\")";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {   
            www.SetRequestHeader("GK-Token", Token);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            } else {
                Debug.Log("Consulta SearchUser exitosa");
                string responseData = www.downloadHandler.text;
                SearchUserResponse data = JsonConvert.DeserializeObject<SearchUserResponse>(responseData);
                if(data.totalItems > 0) {
                    SearchUserItemResponse userData = data.items[0];
                    GameController.instance.setUser(userData.id, userData.username);
                    GameController.instance.feedBackController.SetGoodMessage("Usuario enlazado correctamente");
                    GameController.instance.dialogController.showButton();
                } else {
                    CreateUser(username);
                }
            }
        }
    }

    public void CreateUser(string username) {
        GameController.instance.IniciarRutina(CreateUserCoroutine(username));
    }

    public IEnumerator CreateUserCoroutine(string username) {
        string url = $"{API_URL}/tester/records";
        CreateUserRequest request = new CreateUserRequest {
            username = username
        };
        string json = JsonUtility.ToJson(request);
        using (UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json"))
        {
            www.SetRequestHeader("GK-Token", Token);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error + ", Response: " + www.downloadHandler.text);
            } else {
                Debug.Log("Consulta CreateUser exitosa");
                string responseData = www.downloadHandler.text;
                CreateUserResponse data = JsonUtility.FromJson<CreateUserResponse>(responseData);
                GameController.instance.setUser(data.id, data.username);
                GameController.instance.feedBackController.SetGoodMessage("Usuario creado correctamente");
                GameController.instance.dialogController.showButton();
            }
        }
    }

    public void SaveRecord() {
        GameController.instance.IniciarRutina(SaveRecordCoroutine("data_vgg", GameController.instance.emotions.vggLabel.text, float.Parse(GameController.instance.emotions.brightnessLabel.text)));
        GameController.instance.IniciarRutina(SaveRecordCoroutine("data_fernet", GameController.instance.emotions.fetnetLabel.text, -1f));
    }

    private IEnumerator SaveRecordCoroutine(string collectionName, string emotion, float brillo_calculado) {
        string url = $"{API_URL}/{collectionName}/records";
        SaveRecordRequest data = new SaveRecordRequest {
            tester = GameController.instance.getUser().id,
            session_id = GameController.instance.sessionId,
            emocion = emotion ?? "",
            nodos_guardados = GameController.instance.getNodosGuardados(),
            brillo_calculado = brillo_calculado
        };
        string json = JsonUtility.ToJson(data);
        using (UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json"))
        {
            www.SetRequestHeader("GK-Token", Token);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error + ", Response: " + www.downloadHandler.text);
            } else {
                Debug.Log("Consulta SaveRecord exitosa");
            }
        }
    }

}