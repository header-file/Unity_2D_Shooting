using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class Server : MonoBehaviour
{
    // 스레드 작업에 안전한 싱글톤 패턴
    private static bool shuttingDown = false;
    private static object Lock = new object();
    private static Server instance;

    public static Server Instance
    {
        get
        {
            if (shuttingDown)
            {
                //Debug.LogWarning("Server Instance already destroyed. return null");
                return null;
            }

            lock (Lock)
            {
                if (instance == null)
                {
                    instance = (Server)FindObjectOfType(typeof(Server));

                    if (instance == null)
                    {
                        var serverObject = new GameObject();
                        instance = serverObject.AddComponent<Server>();
                        serverObject.name = "ServerManager";

                        DontDestroyOnLoad(serverObject);
                    }
                }

                return instance;
            }
        }
    }

    // 영수증 저장이 될때까지 갱신을 대기 트리거
    private static bool receiptSave = false;

    // 어플리케이션이 꺼지면....
    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }
    // 파괴되면....
    private void OnDestroy()
    {
        shuttingDown = true;
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /* 테스트 용 함수들 */
    public void Get()
    {
        StartCoroutine(SendGet());
    }
    private IEnumerator SendGet()
    {
#if UNITY_EDITOR
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/chat?test=asd"))
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        // Firebase Hosting 후 발급받는 도메인을 입력한다.
        using (UnityWebRequest www = UnityWebRequest.Get("http://unityshtdef.web.app/chat?test=asd"))
#endif
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                //Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }

    public void Post()
    {
        StartCoroutine(SendPost());
    }
    private IEnumerator SendPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("test", "myPostTest");
#if UNITY_EDITOR
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/chatpost", form))
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        // Firebase Hosting 후 발급받는 도메인을 입력한다.
        using (UnityWebRequest www = UnityWebRequest.Post("http://unityshtdef.web.app/chatpost", form))
#endif
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                //Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }
/* 테스트 함수 끝 */


    // 현재 클라이언트에 접속된 유저 UID 로 서버 DB에 저장된 유저 데이터를 가지고 온다.
    public void GetUserDataDB(string uid, Action callback)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(UserDataGet("http://localhost:5000/main/userDataReturn?uid=", uid, callback));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(UserDataGet("http://unityshtdef.web.app/main/userDataReturn?uid=", uid, callback));
#endif
        }
        catch (Exception err)
        {
            //Debug.Log(err);
        }
    }
    private IEnumerator UserDataGet(string url, string uid, Action callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + uid))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                //Debug.LogFormat("Received: ({0}) {1}", url, www.downloadHandler.text);

                var user = JsonUtility.FromJson<User.userLoginData>(www.downloadHandler.text);

                //Debug.Log(user);

                User.Instance.mainUser = user;

                callback();
            }
        }
    }

    // 현재 클라이언트에 접속된 유저 UID로 서버 DB에 저장된 유저 인벤 정보를 가지고 온다.
    public void GetUserInvenDB(string uid, Action callback)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(UserInvenDataGet("http://localhost:5000/main/userInvenDataReturn?uid=", uid, callback));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(UserInvenDataGet("http://unityshtdef.web.app/main/userInvenDataReturn?uid=", uid, callback));
#endif
        }
        catch (Exception err)
        {
            //Debug.Log(err);
        }
    }
    private IEnumerator UserInvenDataGet(string url, string uid, Action callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + uid))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                //Debug.LogFormat("Received: ({0}) {1}", url, www.downloadHandler.text);

                var user = JsonUtility.FromJson<User.userGoodsData>(www.downloadHandler.text);

                //Debug.Log(user);

                User.Instance.mainInventory = user;

                callback();
            }
        }
    }


    // 새로 생성된 유저 데이터(json)을 서버에 저장한다.
    public void NewUserJsonDBSave(string json, Action callback = null)
    {
        try
        {

#if UNITY_EDITOR
            StartCoroutine(JsonDBSavePost("http://localhost:5000/main/newUserCreate", json, callback));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(JsonDBSavePost("http://unityshtdef.web.app/main/newUserCreate", json, callback));
#endif
        }
        catch (Exception err)
        {
            //Debug.Log(err);
        }
    }

    // 영수증 정보 저장
    public void ReceiptSave(string json)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(JsonDBSavePost("http://localhost:5000/main/userReceiptSave", json));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(JsonDBSavePost("http://unityshtdef.web.app/main/userReceiptSave", json));
#endif
        }
        catch (Exception err)
        {
            //Debug.LogError(err);
        }
    }

    // 새로운 유저 인벤토리 생성
    public void NewUserInventoryJsonDBSave(string json, Action callback = null)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(JsonDBSavePost("http://localhost:5000/main/newUserInvenCreate", json, callback));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(JsonDBSavePost("http://unityshtdef.web.app/main/newUserInvenCreate", json, callback));
#endif
        }
        catch (Exception err)
        {
            //Debug.LogError(err);
        }
    }

    private IEnumerator JsonDBSavePost(string url, string json, Action callback = null)
    {
        using (var uwr = new UnityWebRequest(url, "POST"))
        {
            //Debug.Log(json);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                //Debug.LogFormat("Received: ({0}) {1}", url, uwr.downloadHandler.text);

                receiptSave = bool.Parse(uwr.downloadHandler.text);

                if (callback != null)
                    callback();
            }
        }
    }

    // 결제 아이템이 지급이 되면 영수증 id로 검색 후 지급명령 체크
    private IEnumerator ReceiptGiveCheckDB(string url, string uid, string tid, string giveCheck)
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", uid);
        form.AddField("tid", tid);
        form.AddField("givecheck", giveCheck);

        using (var uwr = UnityWebRequest.Post(url, form))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                //Debug.LogFormat("Received: ({0}) {1}", url, uwr.downloadHandler.text);
            }
        }
    }


    // 유저 코인의 양을 증가시킨다.
    public void userCoinMount(string json)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(JsonDBSavePost("http://localhost:5000/main/userRenewCoin", json));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(JsonDBSavePost("http://unityshtdef.web.app/main/userRenewCoin", json));
#endif
        }
        catch (Exception err)
        {
            //Debug.LogError(err);
        }
    }


    // 구매 후 서버 DB의 유저 코인 정보를 갱신한다.
    public void RenewUserCoin(string uid, int coin, string tid)
    {
        try
        {
#if UNITY_EDITOR
            StartCoroutine(userRenewCoin("http://localhost:5000/main/userRenewCoin", uid, coin, tid));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(userRenewCoin("http://unityshtdef.web.app/main/userRenewCoin", uid, coin, tid));
#endif
        }
        catch (Exception err)
        {
            //Debug.LogError(err);
        }
    }
    private IEnumerator userRenewCoin(string url, string uid, int coin, string tid)
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", uid);
        form.AddField("coin", coin);

        using (var uwr = UnityWebRequest.Post(url, form))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                //Debug.LogFormat("Received: ({0}) {1}", url, uwr.downloadHandler.text);

                if (bool.Parse(uwr.downloadHandler.text))
                {
                    if (!receiptSave)
                    {
                        StartCoroutine(waitSave(uid, tid));
                    }

                    if (receiptSave)
                    {
#if UNITY_EDITOR
                        StartCoroutine(ReceiptGiveCheckDB("http://localhost:5000/main/userReceiptUpdate", uid, tid, "1"));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			            StartCoroutine(ReceiptGiveCheckDB("http://unityshtdef.web.app/main/userReceiptUpdate", uid, tid, "1"));
#endif
                        receiptSave = false;
                    }
                    else
                    {
                        //Debug.Log("영수증 정보를 DB에 저장하지 못했습니다!");
                    }
                }
                
            }
        }
    }

    
    private IEnumerator waitSave(string uid, string tid)
    {
        int count = 0;
        while (!receiptSave)
        {
            yield return new WaitForSeconds(0.1f);

            if (receiptSave)
                break;

            count++;
            if (count > 10)
                break;
        }

        if (receiptSave)
        {
#if UNITY_EDITOR
            StartCoroutine(ReceiptGiveCheckDB("http://localhost:5000/main/userReceiptUpdate", uid, tid, "1"));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
		    StartCoroutine(ReceiptGiveCheckDB("http://unityshtdef.web.app/main/userReceiptUpdate", uid, tid, "1"));
#endif
            receiptSave = false;
        }
        else
        {
            //Debug.Log("영수증 갱신 실패!");
        }
    }





    public void TestInit(string uid)
    {
        StartCoroutine(instance.Init(uid));
    }
    // 테스트 초기화
    private IEnumerator Init(string uid)
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", uid);

        using (UnityWebRequest www = UnityWebRequest.Post("http://unityshtdef.web.app/main/testinit", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                //Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                //Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }
}

//unityshtdef.web.app
//unityshtdef.firebaseapp.com