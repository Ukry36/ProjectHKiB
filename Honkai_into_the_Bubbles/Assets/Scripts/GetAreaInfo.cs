using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAreaInfo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CameraBound"))
        {
            if (other.TryGetComponent(out AreaInfo areaInfo))
            {
                CameraManager.instance.SetBound(areaInfo);

                CameraManager.instance.SetBG(areaInfo);

                AudioManager.instance.ChangeAreaBGMs(areaInfo.areaBGMs, areaInfo.fadeTime);

                WeatherManager.instance.ChangeAreaWeathers(areaInfo.areaWhetherTypes, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CameraBound"))

            Debug.Log(Time.time);
    }
}
