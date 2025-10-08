using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/* Unity Argon API
 * AccessTokenManager
 * This class is used to contact Passport and obtain access_tokens.
 * This class is responsible for handling user credentials.
 * It will keep access tokens up to date.
 * It also contains functions for obtaining access tokens directly if needed. 
 * TO DO: 
 * 1. Integrations for user credentials. Depending on the platform, you will get the correct UI for entering credentials.
 * 2. Implement a get function for access_token so it returns null if the access_token is not valid. 
 */

public class AccessTokenManager : MonoBehaviour
{
    public static event Action AccessTokenObtained;
    private bool firstAccessToken = true;

    private static string accessToken;
    private static string refreshToken;

    public static string AccessToken
    {
        get
        {
            if (DateTime.UtcNow>=tokenExpiryDate)
            {
                return null;
            }
            else
            {   
                return accessToken;
            }
        }
    }

    private static DateTime tokenExpiryDate;

    //How many seconds before must the refresh be done
    private int bufferTime = 60;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AccessTokenLoopCCFlow());
    }

    IEnumerator AccessTokenLoopCCFlow()
    {
        //Client Credentials Flow for Access Token
        string url = APIEnv.passportURL + "/o/token/";

        WWWForm form = new WWWForm();

        form.AddField("grant_type", "client_credentials");
        form.AddField("client_id", APIEnv.clientIdCC);
        form.AddField("client_secret", APIEnv.clientSecretCC);
        form.AddField("scope", "argonserver.write argonserver.read");
        form.AddField("audience", APIEnv.argonURL);


        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, form);

            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            Debug.Log("Making CC WebRequest");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                var json = JSON.Parse(request.downloadHandler.text);
                accessToken = json["access_token"];
                
                //Get the expiry time and use System datetime to set expirydate, don't forget the buffer
                tokenExpiryDate = DateTime.UtcNow.AddSeconds((int)json["expires_in"]-bufferTime);
                Debug.Log("Wait Time: " + (json["expires_in"] - bufferTime) + "s");

                if (firstAccessToken)
                {
                    AccessTokenObtained?.Invoke();
                    firstAccessToken = false;
                }

                //Put a clamp on the value so it doesn't go into the negatives and go out of control. 
                yield return new WaitForSeconds(json["expires_in"] - bufferTime);
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }


        }

    }
}
