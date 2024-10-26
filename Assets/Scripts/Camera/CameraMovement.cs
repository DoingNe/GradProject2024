using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("CameraMovement");
                    instance = instanceContainer.AddComponent<CameraMovement>();
                }
            }
            return instance;
        }
    }
    private static CameraMovement instance;
    public GameObject Player;

    public Image fadeImage;
    float fadeDuration = 3f;

    float offsetY = 1f;
    float offsetZ = -10f;
    float smooth = 5f;

    float maxX1 = 406f;
    float minX1 = -23f;
    float maxY1 = 8f;
    float minY1 = 4f;

    float maxX2 = 406f;
    float minX2 = -23f;
    float maxY2 = -78f;
    float minY2 = -81f;

    float maxX3 = 20f;
    float minX3 = -16f;
    float maxY3 = -159f;
    float minY3 = -159f;

    Vector3 target;

    public bool cameraSmoothMoving = false;

    private void LateUpdate()
    {
        target = new Vector3(Player.transform.position.x, Player.transform.position.y + offsetY, Player.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);

        if(cameraSmoothMoving)
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);
        else
        {
            transform.position = target;
            cameraSmoothMoving = true;
        }

        if (GameManager.Instance.currentStage == 0)
        {
            if (transform.position.x > maxX1) transform.position = new Vector3(maxX1, transform.position.y, transform.position.z);
            if (transform.position.x < minX1) transform.position = new Vector3(minX1, transform.position.y, transform.position.z);
            if (transform.position.y > maxY1) transform.position = new Vector3(transform.position.x, maxY1, transform.position.z);
            if (transform.position.y < minY1) transform.position = new Vector3(transform.position.x, minY1, transform.position.z);
        }

        if (GameManager.Instance.currentStage == 1)
        {
            if (transform.position.x > maxX2) transform.position = new Vector3(maxX2, transform.position.y, transform.position.z);
            if (transform.position.x < minX2) transform.position = new Vector3(minX2, transform.position.y, transform.position.z);
            if (transform.position.y > maxY2) transform.position = new Vector3(transform.position.x, maxY2, transform.position.z);
            if (transform.position.y < minY2) transform.position = new Vector3(transform.position.x, minY2, transform.position.z);
        }

        if (GameManager.Instance.currentStage == 2)
        {
            if (transform.position.x > maxX3) transform.position = new Vector3(maxX3, transform.position.y, transform.position.z);
            if (transform.position.x < minX3) transform.position = new Vector3(minX3, transform.position.y, transform.position.z);
            if (transform.position.y > maxY3) transform.position = new Vector3(transform.position.x, maxY3, transform.position.z);
            if (transform.position.y < minY3) transform.position = new Vector3(transform.position.x, minY3, transform.position.z);
        }
    }

    public IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0f);
    }

    public IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
