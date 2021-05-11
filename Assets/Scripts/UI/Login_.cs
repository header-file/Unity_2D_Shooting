using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*using Google;
//using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;*/

public class Login_ : MonoBehaviour
{
    /*// Auth 용 instance
    FirebaseAuth auth = null;

    // 사용자 계정
    FirebaseUser user = null;

    // 로그인 선택 화면
    public GameObject LoginPanel;

    // 임시 로딩 패널
    public GameObject LoadingPanel;

    // 닉네임 설정 패널
    public GameObject NicknamePanel;

    // 이메일 생성 패널
    public GameObject EmailCreatePanel;

    // 연동 관련 패널
    public GameObject DataInterlinkPanel;

    // 기기 연동이 되어 있는 상태인지 체크한다.
    private bool signedIn = false;

    // 임시저장용 클래스
    private User.userLoginData.LoginType tempLoginType = User.userLoginData.LoginType.None;
    private string tempemail = string.Empty;
    private string temppw = string.Empty;

    private void Awake()
    {
        // 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // 유저의 로그인 정보에 어떠한 변경점이 생기면 실행되게 이벤트를 걸어준다.
        auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);

        LoadingPanel.SetActive(false);
        LoadingPanel.SetActive(false);
        NicknamePanel.SetActive(false);
    }

    private void Start()
    {
        //#if UNITY_EDITOR
        //TestInit();
        //#endif
    }

    // 테스트용 초기화 함수
    public void TestInit()
    {
        if (auth.CurrentUser != null)
        {
            // 로그아웃 처리한다.
            auth.SignOut();
            //Server.Instance.TestInit(auth.CurrentUser.UserId);
            //auth.CurrentUser.DeleteAsync();
        }
    }


    // 계정 로그인에 어떠한 변경점이 발생시 진입.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            // 연동된 계정과 기기의 계정이 같다면 true를 리턴한다. 
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                UnityEngine.Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                UnityEngine.Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // 로그인 선택 패널을 열며 로그인한 user가 있는지 확인한다.
    // 없으면 계정 생성 시작
    public void LoginCheck()
    {
        // 연동 상태가 아니라면...
        if (!signedIn)
        {
            LoginPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CurrentUserDataGet());
        }
    }

    // 기존 유저 정보 서버에서 가져온다
    public IEnumerator CurrentUserDataGet()
    {
        LoadingPanel.SetActive(true);

        // 유저 정보
        User.Instance.GetUserData(auth.CurrentUser.UserId, new System.Action(() => {
            Debug.Log("유저 정보 로드 완료!");
            // 유저 인벤 정보
            User.Instance.GetUserInven(auth.CurrentUser.UserId, new System.Action(() => {
                // 다음 씬으로 넘긴다.
                NextSecne();
            }));
        }));

        yield return null;
    }

    // 게임씬으로 넘어감
    public void NextSecne()
    {
        Debug.Log("GameScene 으로...");

        SceneManager.LoadSceneAsync(1);
    }

    // 익명 로그인
    public async void AnonyLogin()
    {
        LoadingPanel.SetActive(true);

        // 익명 로그인 진행
        await auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            // 익명 로그인 연동 결과
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        // 신규 유저 데이터 임시 저장
        userDataInit();
        userDataTempSave(User.userLoginData.LoginType.anony);

        LoadingPanel.SetActive(false);
        // 닉네임 생성 창 켜주기
        NicknamePanel.SetActive(true);
    }

    // 구글 로그인
    public void GoogleLogin()
    {
        LoadingPanel.SetActive(true);

        try
        {
            // 구글 로그인 처리부분
            // 구글 로그인 팝업창이 꺼지면 실행될 콜백함수를 선언한다.
            GoogleLoginProcessing(new System.Action<bool>((bool chk) =>
            {
                if (chk)
                {
                    // 신규 유저 데이터 임시 저장
                    userDataInit();
                    userDataTempSave(User.userLoginData.LoginType.google, auth.CurrentUser.Email);

                    LoadingPanel.SetActive(false);
                    // 닉네임 생성 창 켜주기
                    NicknamePanel.SetActive(true);
                }
                else
                {
                    LoadingPanel.SetActive(false);
                }
            }));
        }
        catch (System.Exception err)
        {
            Debug.LogError(err);
            LoadingPanel.SetActive(false);
        }
    }
    // 구글 로그인 구동 부분
    private async void GoogleLoginProcessing(System.Action<bool> callback)
    {
        if (GoogleSignIn.Configuration == null)
        {
            // 설정
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "1014486542095 - am2i13r5meem0qsjho8e3trhqul8632d.apps.googleusercontent.com"
            };

        }

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        await signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google Login task.IsCanceled");
                callback(false);
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Google Login task.IsFaulted");
                callback(false);
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                        Debug.Log("Google Login authTask.IsCanceled");
                        callback(false);
                        return;
                    }
                    if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        Debug.Log("Google Login authTask.IsFaulted");
                        callback(false);
                        return;
                    }

                    user = authTask.Result;
                    Debug.LogFormat("Google User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
                    callback(true);
                    return;
                });
            }
        });
    }

    // 이메일 로그인
    public async void EmailLogin()
    {
        var email = EmailCreatePanel.transform.Find("email").Find("Text").GetComponent<UnityEngine.UI.Text>().text;
        var pw = EmailCreatePanel.transform.Find("pw").Find("Text").GetComponent<UnityEngine.UI.Text>().text;

        if (email.Length < 1 || pw.Length < 1)
        {
            Debug.Log("이메일 ID 나 PW 가 비어있습니다.");
            return;
        }

        EmailCreatePanel.SetActive(false);
        LoadingPanel.SetActive(true);

        await auth.CreateUserWithEmailAndPasswordAsync(email, pw).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityEngine.Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                UnityEngine.Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // firebase email user create
            Firebase.Auth.FirebaseUser newUser = task.Result;
            UnityEngine.Debug.LogFormat("Firebase Email user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            return;
        });


        // 신규 유저 데이터 임시 저장
        userDataInit();
        userDataTempSave(User.userLoginData.LoginType.email, email, pw);

        // 로딩 패널 꺼주ㄱ
        LoadingPanel.SetActive(false);
        // 닉네임 생성 창 켜주기
        NicknamePanel.SetActive(true);
    }


    // 유저 닉네임 입력 시작
    private void InsertNewUserData()
    {
        var nickname = NicknamePanel.transform.Find("InputField").Find("NickNameInput").GetComponent<UnityEngine.UI.Text>().text;

        if (nickname.Length > 0)
        {
            // 신규 유저 데이터 입력
            loginDataSave(tempLoginType, nickname, tempemail, temppw);
        }
        else
        {
            Debug.Log("별명을 입력해주세요.");
            return;
        }
    }

    // 유저 임시 데이터 초기화
    private void userDataInit()
    {
        tempLoginType = User.userLoginData.LoginType.None;
        tempemail = string.Empty;
        temppw = string.Empty;
    }

    // 유저 데이터 임시 저장
    private void userDataTempSave(User.userLoginData.LoginType loginType, string email = null, string pw = null)
    {
        tempLoginType = loginType;
        tempemail = email;
        temppw = pw;
    }

    // 신규 유저 데이터 입력
    private void loginDataSave(User.userLoginData.LoginType loginType, string nickname = null, string email = null, string pw = null)
    {
        // 유저 데이터
        var newUser = new User.userLoginData();

        // DB에 저장될 유저데이터 초기화
        newUser.loginType = tempLoginType;
        newUser.nickname = nickname;
        newUser.uid = user.UserId;
        newUser.email = email;
        newUser.pw = pw;
        newUser.deviceModel = SystemInfo.deviceModel;
        newUser.deviceName = SystemInfo.deviceName;
        newUser.deviceType = SystemInfo.deviceType;
        newUser.deviceOS = SystemInfo.operatingSystem;
        newUser.createDate = auth.CurrentUser.Metadata.CreationTimestamp;
        string json = JsonUtility.ToJson(newUser);


        // 코인 저장용
        var newUserInventory = new User.userGoodsData();

        // DB에 저장될 유저데이터 초기화
        newUserInventory.uid = user.UserId;
        newUserInventory.coin = 0;

        LoadingPanel.SetActive(true);
        LoginPanel.SetActive(false);
        NicknamePanel.SetActive(false);

        // 위에 정리한 json을 서버에 보내 DB에 저장한다.
        // 새로운 유저에 대한 데이터를 DB에 보낸다.
        Server.Instance.NewUserJsonDBSave(json, () => {
            // DB에 저장 후 디바이스 user정보에도 저장한다.
            User.Instance.mainUser = newUser;

            // 새로운 유저의 인벤토리 데이터를 DB에 보낸다.
            Server.Instance.NewUserInventoryJsonDBSave(JsonUtility.ToJson(newUserInventory), () => {
                // DB에 저장 후 디바이스 user정보에도 저장한다.
                User.Instance.mainInventory = newUserInventory;
                // 다음씬으로 이동
                NextSecne();
            });
        });
    }

    // 연동 해제
    public void SignOut()
    {
        if (auth.CurrentUser != null)
            auth.SignOut();

        DataInterlinkPanel.SetActive(false);
    }

    // 연동 계정 삭제
    public void UserDelete()
    {
        if (auth.CurrentUser != null)
            auth.CurrentUser.DeleteAsync();

        DataInterlinkPanel.SetActive(false);
    }*/
}