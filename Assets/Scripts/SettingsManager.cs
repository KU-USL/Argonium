using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle cylinderBufferToggle;
    public Toggle cesiumOSMToggle;
    public Toggle cacheModeToggle;

    public TMP_InputField passportURL;
    public TMP_InputField argonURL;
    public TMP_InputField clientIDCC;
    public TMP_InputField clientSecretCC;

    private void OnEnable()
    {
        cylinderBufferToggle.isOn = FlightDeclaration.enableCylinderBuffer;
        cesiumOSMToggle.isOn = GameManager.enableCesiumOSM;
        cacheModeToggle.isOn = GameManager.enableCacheMode;

        passportURL.text = APIEnv.passportURL;
        argonURL.text = APIEnv.argonURL;
        clientIDCC.text = APIEnv.clientIdCC;
        clientSecretCC.text = APIEnv.clientSecretCC;
    }

    public void EnableCylinderBuffer(bool toggle)
    {
        FlightDeclaration.enableCylinderBuffer = toggle;
    }

    public void EnableCesiumOSM(bool toggle) 
    { 
        GameManager.enableCesiumOSM = toggle;
    }

    public void EnableCacheMode(bool toggle)
    {
        GameManager.enableCacheMode = toggle;
    }

    public void SetPassportURL(string url)
    {
        Debug.Log("Set String!");
        APIEnv.passportURL = url;
    }

    public void SetArgonURL(string url)
    {
        Debug.Log("Set String!");
        APIEnv.argonURL = url;
    }

    public void SetClientIDCC(string token)
    {
        Debug.Log("Set String!");
        APIEnv.clientIdCC = token;
    }

    public void SetClientSecretCC(string token)
    {
        Debug.Log("Set String!");
        APIEnv.clientSecretCC = token;
    }
}
