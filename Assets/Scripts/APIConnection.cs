using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIConnection : MonoBehaviour
{

    /// <summary>
    /// ORE ID API MAIN CONFIG
    /// </summary>
    public string apiBaseUrl = "https://service.oreid.io";
    private string serviceKey = "t_xxx...xxx";
    
    /// <summary>
    /// INFO DISPLAYABLE TO USER
    /// </summary>
    public Text apiEndpoint;
    public Text serviceResponseText;
    public Text successState;

    // 
    // VALUES THAT NEED INPUTS
    //

    // SignUp Menu
    private string userEmail;
    private string userPassword;
    private string userFullName;
    private string userName;
    private string accountType; // virtual or native
    private string delayWalletSetup;
    private string isTestUser;

    // GetUser Menu
    private string userNameQuery;

    // CreateWallet Menu
    private string walletAccountName;
    private string walletAccountType;
    private string walletPassword;
    private string walletChainNetwork;

    // Transaction Menu
    private string txnAccountName; // oreid account
    private string txnToAccount; // recipient
    private string txnFromAccount; // key must be owned by by txnAccountName
    private string txnAmount;
    private string txnWalletPassword;
    private string txnBroadcast;
    private string txnReturnSigned;
    private string txnNetwork;
    
    // 
    // AVAILABLE ENDPOINTS AND RESPONSE STRUCTURES
    // 
    private string healthEndpoint = "/health";
    public HealthResponseStruct healthResponse;

    private string getUserEndpoint = "/api/account/user?account=";
    public GetUserResponseStruct getUserResponse;

    private string createUserEndpoint = "/api/custodial/new-user";
    public CreateUserResponseStruct createUserResponse;

    private string createChainWalletEndpoint = "/api/custodial/new-chain-account";
    public CreateChainWalletResponseStruct createChainWalletResponse;

    private string signTransactionEndpoint = "/api/transaction/sign";
    public TransactionResponseStruct testTransactionResponse;

    // 
    // VARIABLE ACCESS
    //
 
    public void UpdateBaseUrl(string baseUrl)
    {
        apiBaseUrl = baseUrl;
    }

    // 
    // SignUpUser Menu variables
    // 
    public void UpdateUserEmail(string emailInput)
    {
        // refers to the parameter 'email'
        userEmail = emailInput;
    }

    public void UpdateUserPassword(string passwordInput)
    {
        // refers to the parameter 'user_password'
        userPassword = passwordInput;
    }

    public void UpdateUserFullName(string fullNameInput)
    {
        // refers to the parameter 'name'
        userFullName = fullNameInput;
    }

    public void UpdateUserName(string userNameInput)
    {
        // refers to the parameter 'user_name'
        userName = userNameInput;
    }

    public void UpdateAccountType(string accountTypeInput)
    {
        accountType = accountTypeInput;
    }

    public void UpdateIsTestUser(string isTestUserOption)
    {
        isTestUser = isTestUserOption;
    }

    public void UpdateDelayWalletSetup(string delayWalletSetupOption)
    {
        delayWalletSetup = delayWalletSetupOption;
    }

    //
    //  GetUser Menu Variables
    //
    public void UpdateUserNameQuery(string userNameQueryInput)
    {
        userNameQuery = userNameQueryInput;
    }

    // 
    // CreateWallet Menu Variables
    // 
    public void UpdateWalletChainNetwork(string walletChainNetworkInput)
    {
        walletChainNetwork = walletChainNetworkInput;
    }

    public void UpdateWalletAccountName(string walletAccountNameInput)
    {
        walletAccountName = walletAccountNameInput;
    }

    public void UpdateWalletPassword(string walletPasswordInput)
    {
        walletPassword = walletPasswordInput;
    }

    public void UpdateWalletAccountType(string walletAccountTypeInput)
    {
        walletAccountType = walletAccountTypeInput;
    }

    // 
    // Test Transaction Menu Variables
    //  
    public void UpdateTxnAccountName(string txnAccountNameInput)
    {
        txnAccountName = txnAccountNameInput;
    }

    public void UpdateTxToAccount(string txnToAccountInput)
    {
        txnToAccount = txnToAccountInput;
    }

    public void UpdateTxnFromAccount(string txnFromAccountInput)
    {
        txnFromAccount = txnFromAccountInput;
    }

    public void UpdateTxnAmount(string txnAmountInput)
    {
        txnAmount = txnAmountInput;
    }

    public void UpdateTxnWalletPassword(string txnWalletPasswordInput)
    {
        txnWalletPassword = txnWalletPasswordInput;
    }

    public void UpdateTxnBroadcast(string txnBroadcastInput)
    {
        txnBroadcast = txnBroadcastInput;
    }

    public void UpdateTxnReturnSigned(string txnReturnSignedInput)
    {
        txnReturnSigned = txnReturnSignedInput;
    }

    public void UpdateTxnNetwork(string txnNetworkInput)
    {
        txnNetwork = txnNetworkInput;
    }


    /// <summary>
    /// API MANAGER FUNCTION -- MAIN UNITY CONTROL PLANE
    /// </summary>
    /// <remarks>
    /// Attach this function to a Unity button and pass in the desired endpoint.
    /// </remarks>
    /// <param name="endpoint">the action to perform.</param>
    public void CallAPI(string endpoint)
    {
        switch (endpoint)
        {
            case "health":
                StartCoroutine(HealthRequest());
                break;

            case "getUser":
                StartCoroutine(GetUserRequest());
                break;

            case "signup":
                StartCoroutine(SignUpRequest());
                break;

            case "wallet":
                StartCoroutine(CreateWalletRequest());
                break;

            case "txn":
                StartCoroutine(TestTxnRequest());
                break;
        }
    }

    // 
    // GET and POST FUNCTIONS
    // 

    private IEnumerator GetRequest(string url, Action<string> callback)
    {
        string message = null;
        
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("api-key", serviceKey);
            yield return www.SendWebRequest();

            message = ParseWWWResult(url, www);
        }
        callback(message);
    }

    private IEnumerator PostRequest(string url, WWWForm form, Action<string> callback)
    {
        string message = null;

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("api-key", serviceKey);
            yield return www.SendWebRequest();

            message = ParseWWWResult(url, www);
        }
        callback(message);
    }

    private string ParseWWWResult(string url, UnityWebRequest www)
    {
        string message = null;

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
            string error = www.error + 
                           "\nEndpoint: " + url +
                           "\nResult: " + www.result + 
                           "\nRaw Data: " + www.downloadHandler.text;
                serviceResponseText.text = error;
                Debug.LogError(error);
                break;
            case UnityWebRequest.Result.Success:
                message = www.downloadHandler.text;
                Debug.Log("Recieved: " + message);
                break;
        }
        return message;
    }

    // 
    // ORE ID API REQUEST FUNCTIONS
    // 

    private IEnumerator HealthRequest()
    {
        apiEndpoint.text = this.apiBaseUrl + this.healthEndpoint;
        Action<string> callback = LoadJsonDataHealthCallBack;

        return GetRequest(apiEndpoint.text, callback);
    }

    private IEnumerator GetUserRequest()
    {
        apiEndpoint.text = this.apiBaseUrl + this.getUserEndpoint + this.userNameQuery;
        Action<string> callback = LoadJsonDataGetUserCallBack;

        return GetRequest(apiEndpoint.text, callback);
    }

    /// <summary>
    /// Sign Up a user on the ORE ID service using Email.
    /// </summary>
    /// <remarks>
    /// This request is for a new account to be created on the ORE ID Blockchain.
    /// Additional blockchain accounts are created for each blockchain the ORE ID dApp supports.
    /// Blockchain account creation can be indefinetly delayed if "delay_wallet_setup" is "true".
    /// Test users can be created if "is_test_user" is set "true".
    /// Test users can be deleted. 
    /// </remarks>
    /// <see>https://documenter.getpostman.com/view/7805568/SWE55yRe#c3709139-724f-482a-92e4-0f6b66b4cdb2</see>
    private IEnumerator SignUpRequest()
    {
        apiEndpoint.text = this.apiBaseUrl + this.createUserEndpoint;
        Action<string> callback = LoadJsonDataSignUpCallBack;

        SignUpRequestOptions(); // injects default values - for testing only

        WWWForm form = new WWWForm();
        form.AddField("name", this.userFullName);
        form.AddField("user_name", this.userName);
        form.AddField("email", this.userEmail);
        form.AddField("account_type", this.accountType);
        form.AddField("user_password", this.userPassword);
        form.AddField("delay_wallet_setup", this.delayWalletSetup);
        form.AddField("is_test_user", this.isTestUser);
        
        return PostRequest(apiEndpoint.text, form, callback);
    }

    private void SignUpRequestOptions()
    {
        if (this.userEmail == null)
        {
            this.userEmail = "aikondemo@gmail.com";
        }

        if (this.userFullName == null)
        {
            this.userFullName = "Cristy Wilson";
        }

        if (this.userName == null)
        {
            this.userName = "CWilson";
        }

        if (this.userPassword == null)
        {
            this.userPassword = "1231";
        }

        if (this.isTestUser == null)
        {
            this.isTestUser = "true";
        }

        if (this.delayWalletSetup == null)
        {
            this.delayWalletSetup = "false";
        }

        if (this.accountType == null)
        {
            this.accountType = "native";
        }
    }

    private IEnumerator CreateWalletRequest()
    {
        apiEndpoint.text = this.apiBaseUrl + this.createChainWalletEndpoint;
        Action<string> callback = LoadJsonDataWalletCallBack;

        CreateWalletRequestOptions(); // inject default values - for testing only

        //  Data form to pass into Post Request
        WWWForm form = new WWWForm();
        form.AddField("account_name", this.walletAccountName);
        form.AddField("account_type", this.walletAccountType);
        form.AddField("chain_network", this.walletChainNetwork);
        form.AddField("user_password", this.walletPassword);

        return PostRequest(apiEndpoint.text, form, callback);
    }

    private void CreateWalletRequestOptions()
    {
        if (this.walletAccountName == null)
        {
            this.walletAccountName = "ore1ts4ce5hr";
        }

        if (this.walletAccountType == null)
        {
            this.walletAccountType = "native";
        }

        if (this.walletChainNetwork == null)
        {
            this.walletChainNetwork = "telos_test";
        }

        if (this.walletPassword == null)
        {
            this.walletPassword = "1231";
        }
    }

    private IEnumerator TestTxnRequest()
    {
        apiEndpoint.text = this.apiBaseUrl + this.signTransactionEndpoint;
        Action<string> callback = LoadJsonDataTestTxnCallBack;

        TransactionOptions();

        string testTransactionRaw = (string) @"
        {
            ""actions"":[
                {
                    ""account"":""eosio.token"",
                    ""name"":""transfer"",
                    ""authorization"":[
                        {
                            ""actor"":""{0}"",
                            ""permission"":""active""
                        }
                    ],
                    ""data"":
                        {
                            ""from"":""{0}"",
                            ""to"":""{1}"",
                            ""quantity"":""{2} TLOS"",
                            ""memo"":""Sent with ORE ID""
                        }
                }
            ]
        }".Replace("\n", "").Replace("\r", "").Replace("  ", "")
          .Replace("{0}", (string) this.txnFromAccount)
          .Replace("{1}", (string) this.txnToAccount)
          .Replace("{2}", (string) this.txnAmount);

        string testTransactionB64 = EncodeTo64(testTransactionRaw);
        Debug.LogFormat("Test Transaction Encode B64: {0}", testTransactionB64);

        //  Data form to pass into Post Request
        WWWForm form = new WWWForm();
        form.AddField("broadcast", this.txnBroadcast); // not working when set
        form.AddField("account", this.txnAccountName);
        form.AddField("chain_account", this.txnFromAccount);
        form.AddField("chain_network", this.txnNetwork);
        form.AddField("transaction", testTransactionB64);
        form.AddField("user_password", this.txnWalletPassword);
        form.AddField("return_signed_transaction", this.txnReturnSigned);
        // form.AddField("expire_seconds", "3000"); // Causing the ORE ID service to Error

        return PostRequest(apiEndpoint.text, form, callback);
    }

    public void TransactionOptions()
    {
        if (this.txnAccountName == null)
        {
            this.txnAccountName = "ore1ts4ce5hr";
        }

        if (this.txnFromAccount == null)
        {
            this.txnFromAccount = "ore1tsczhlt3";
        }

        if (this.txnToAccount == null)
        {
            this.txnToAccount = "oreidfund111";
        }

        if (this.txnAmount == null)
        {
            this.txnAmount = "0.0001";
        }

        if (this.txnWalletPassword == null)
        {
            this.txnWalletPassword = "1231";
        }

        if (this.txnBroadcast == null)
        {
            this.txnBroadcast = "true";
        }

        if (this.txnReturnSigned == null)
        {
            this.txnReturnSigned = "true";
        }

        if (this.txnNetwork == null)
        {
            this.txnNetwork = "telos_test";
        }
    }



    // 
    // CALLBACK FUNCTIONS
    // 

    private void LoadJsonDataHealthCallBack(string res)
    {
        if (res != null)
        {

            healthResponse = JsonUtility.FromJson<HealthResponseStruct>(res);
            serviceResponseText.text = healthResponse.service.ToString();
        }

        SetSuccessState(res);

    }

    private void LoadJsonDataGetUserCallBack(string res)
    {
        if (res != null)
        {

            getUserResponse = JsonUtility.FromJson<GetUserResponseStruct>(res);
            serviceResponseText.text = getUserResponse.accountName.ToString() + "\n" + getUserResponse.email.ToString();
        }

        SetSuccessState(res);

    }

    private void LoadJsonDataSignUpCallBack(string res)
    {
        if (res != null)
        {
            createUserResponse = JsonUtility.FromJson<CreateUserResponseStruct>(res);
            serviceResponseText.text = createUserResponse.accountName.ToString();
        }

        SetSuccessState(res);
    }

    private void LoadJsonDataWalletCallBack(string res)
    {
        if (res != null)
        {
            createChainWalletResponse = JsonUtility.FromJson<CreateChainWalletResponseStruct>(res);
            serviceResponseText.text = createChainWalletResponse.chainAccount.ToString();
        }

        SetSuccessState(res);
    }

    private void LoadJsonDataTestTxnCallBack(string res)
    {
        if (res != null)
        {
            testTransactionResponse = JsonUtility.FromJson<TransactionResponseStruct>(res);
            serviceResponseText.text = testTransactionResponse.transaction_id.ToString() +
                "\n" + testTransactionResponse.signed_transaction.ToString();
        }

        SetSuccessState(res);
    }

    private void SetSuccessState(string res)
    {
        if (res != null)
        {
            successState.color = Color.green;
            successState.text = "Success!";
        }
        else
        {
            successState.color = Color.red;
            successState.text = "Fail!";
        }
    }

    // 
    // HELPER FUNCTIONS
    //
     
    static private string EncodeTo64(string toEncode)
    {
        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
        string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
        return returnValue;
    }

    // 
    // DATA STRUCTURES
    // 

    [Serializable]
    public struct HealthResponseStruct
    {
        public string service;
        public string datetime;
        public string envVersion;
        public string buildVersion;
        public string deployDate;
        public string envHash;
        public string gitBranch;
        public string gitCommit;
        public string createAppAccessTokenTime;
        public string mongoFindUserTime;
        public string getAccountBalanceOreTime;
    }

    [Serializable]
    public struct GetUserResponseStruct
    {
        public string accountName;
        public string email;
        public string picture;
        public string name;
        public string username;
        public List<GetUserPermissionResponseStruct> permissions;
    }

    [Serializable]
    public struct GetUserPermissionResponseStruct
    {
        public string chainAccount;
        public string chainNetwork;
        public string permission;
        public string isDefault;
    }

    [Serializable]
    public struct CreateUserStruct
    {
        public string access_token;
        public string access_token_provider;
        public string account_type;
        public string user_password;
        public string name;
        public string user_name;
        public string email;
        public string picture;
        public string phone;
        public string email_verified;
        public string is_test_user;
        public string delay_wallet_setup;
    }

    [Serializable]
    public struct CreateUserResponseStruct
    {
        public string processId;
        public string accountName;
    }

    [Serializable]
    public struct CreateUserErrorStruct
    {
        public string processId;
        public string message;
        public string errorCode;
        public string errorMessage;
    }

    [Serializable]
    public struct CreateChainWalletStruct
    {
        public string account_name;
        public string account_type;
        public string chain_network;
        public string user_password;
    }

    [Serializable]
    public struct CreateChainWalletResponseStruct
    {
        public string processId;
        public string chainAccount;
    }

    [Serializable]
    public struct TransactionStruct
    {
        public string account;
        public string chain_account;
        public string chain_network;
        public string transaction;
        public string signed_transaction;
        public string user_password;
        public string broadcast;
        public string return_signed_address;
        public string expire_seconds;
        public string multisig_chain_accounts;
    }

    [Serializable]
    public struct TransactionResponseStruct
    {
        public string signed_transaction;
        public string transaction_id;
    }
}