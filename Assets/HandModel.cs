using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; 

public class HandModel : MonoBehaviour
{
    // Start is called before the first frame update
    public InputDeviceCharacteristics controllerChrateristics;
    private bool isValid = false;
    private InputDevice targetDevice;
    public GameObject controllerPrefab;
    private GameObject spawnedController;

    public GameObject handModelPrefab;
    private GameObject spawnedHandModel;
    public bool showController = false;

    private Animator handAnimator;
    void Start()
    {
        getDevices();

    }
    void Update()
    {
        if (!isValid)
        {
            getDevices();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateAnimator();
            }
/*            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

            if (primaryButtonValue)
                Debug.Log("Pressing Primary Button");

            targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);

            if (triggerValue > 0.1)
                Debug.Log("Trigger pressed " + triggerValue);

            targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);

            if (primary2DAxisValue != Vector2.zero)
                Debug.Log("Primary Thumb Stick " + primary2DAxisValue);*/
        }
    }
    void getDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

//       InputDeviceCharacteristics rightControllerChracteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(controllerChrateristics, devices);

        Debug.Log(devices.Count);
        if (devices.Count > 0)
        {
            isValid = true;
            foreach (var item in devices)
            {
                Debug.Log(item.name + ". " + item.characteristics);
            }
            targetDevice = devices[0];
            spawnedController = Instantiate(controllerPrefab, transform);
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateAnimator()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    // Update is called once per frame

}
