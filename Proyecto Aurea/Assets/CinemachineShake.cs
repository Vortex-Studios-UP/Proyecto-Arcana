/* 
@Author: Christian Matos
@Date: 2023-07-02 10:33:49
@Last Modified by: Christian Matos
@Last Modified Date: 2023-07-02 10:33:49

* Functionality:
* Approach: Singleton pattern
* To Use: Call the ShakeCamera method from anywhere using the CinemachineShake.Instance
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        Instance = this;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Shake the camera with a given intensity and time
    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(ShakeCameraCoroutine(intensity, time));
    }

    private IEnumerator ShakeCameraCoroutine(float intensity, float time)
    {
        // Get the Cinemachine perlin component
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Set the amplitude gain to the intensity
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        // Wait for the time to pass before setting the amplitude gain to 0
        yield return new WaitForSeconds(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
