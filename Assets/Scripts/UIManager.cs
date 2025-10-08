using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI info;
    public GameObject InfoDisplay;
    public InputActionReference escape;
    public InputActionReference infoPanel;
    public InputActionReference droneMode;
    public InputActionReference flightDeclarationMode;
    public InputActionReference geofenceMode;

    public static event Action<int> LayerMaskChange;

    private void OnEnable()
    {
        RayCastSystem.OnObjectHit += DisplayText;
        escape.action.started += BackToMainMenu;
        infoPanel.action.started += ToggleDisplay;
        droneMode.action.started += ChangeLayerMask;
        flightDeclarationMode.action.started += ChangeLayerMask;
        geofenceMode.action.started += ChangeLayerMask;
    }

    private void OnDisable()
    {
        RayCastSystem.OnObjectHit -= DisplayText;
        escape.action.started -= BackToMainMenu;
        infoPanel.action.started -= ToggleDisplay;
        droneMode.action.started -= ChangeLayerMask;
        flightDeclarationMode.action.started -= ChangeLayerMask;
        geofenceMode.action.started -= ChangeLayerMask;
    }

    public void SetViewBox(string inputText)
    {
        Debug.Log(inputText);
    }

    void BackToMainMenu(InputAction.CallbackContext obj)
    {
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    void ChangeLayerMask(InputAction.CallbackContext obj)
    {
        if (obj.action.name == "DroneMode")
        {
            Debug.Log("DroneMode Changed");
            LayerMaskChange?.Invoke(10);
        }
        else if (obj.action.name == "FlightDeclarationMode")
        {
            Debug.Log("FlightDeclarationMode Changed");
            LayerMaskChange?.Invoke(12);
        }
        else if (obj.action.name == "GeofenceMode")
        {
            Debug.Log("GeofenceMode Changed");
            LayerMaskChange?.Invoke(11);
        }
        else
        {
            Debug.Log("Unknown Action Attempted to Change the LayerMask");
        }
    }

    void DisplayText(GameObject obj)
    {
        if (obj.GetComponentInParent<RID>())
        {
            //Call loop for RID
            info.text = obj.GetComponentInParent<RID>().ToString();
        }
        else if (obj.GetComponent<Geofence>())
        {
            info.text = obj.GetComponent<Geofence>().ToString();
        }
        else if (obj.GetComponentInParent<FlightDeclaration>())
        {
            info.text = obj.GetComponentInParent<FlightDeclaration>().ToString();
        }
        else
        {
            Debug.Log("No object was returned from the raycast containing the necessary components");
        }

    }

    public void ToggleDisplay(InputAction.CallbackContext obj)
    {
        InfoDisplay.SetActive(!InfoDisplay.activeInHierarchy);
    }

    public void ToggleDisplay()
    {
        InfoDisplay.SetActive(!InfoDisplay.activeInHierarchy);
    }
}
