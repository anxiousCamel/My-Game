using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [ReadOnly] public CinemachineVirtualCamera virtualCamera;
    [ReadOnly] public CinemachineBasicMultiChannelPerlin perlin;
    [ReadOnly] public Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float duration)
    {
        StartCoroutine(ShakeCoroutine(amplitude, duration));
    }

    private IEnumerator ShakeCoroutine(float amplitude, float duration)
    {
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                perlin.m_AmplitudeGain = amplitude * (1 - (elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }
            StopShake();
            ResetCamera();
        }
    }

    public void StopShake()
    {
        perlin.m_AmplitudeGain = 0;
    }

    public void ResetCamera()
    {
        // Desative o componente Cinemachine
        virtualCamera.enabled = false;

        // Agora você pode redefinir a rotação da câmera
        mainCamera.transform.rotation = Quaternion.identity;

        // Reative o componente Cinemachine
        virtualCamera.enabled = true;
    }
}
