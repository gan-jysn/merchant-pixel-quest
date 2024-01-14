using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : Singleton<CameraHandler> {
    [SerializeField] CameraFocus focusType;
    //Temp: Make sure to have same amount of CameraFocus Type to Virtual Cameras 
    //Note: Convert to Dictionary 
    [SerializeField] List<GameObject> virtualCameras = new List<GameObject>();

    private int focusTypeIndex = 0;

    public void SetCameraFocus(CameraFocus focus) {
        focusType = focus;
        focusTypeIndex = (int)focusType;

        //Make sure to Enable Camera First
        virtualCameras[focusTypeIndex].SetActive(true);

        //Disable other Cameras
        for (int i = 0; i < 4; i++) {
            if (focusTypeIndex != i) {
                virtualCameras[i].SetActive(false);
            }
        }
    }

}

public enum CameraFocus {
    PlayerFocus,
    ArmoryShopFocus,
    MeatShopFocus,
    PotionShopFocus
}