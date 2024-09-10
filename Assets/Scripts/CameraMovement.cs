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
    public float fadeDuration = 1f;

    public float offsetY = 1f;
    public float offsetZ = -10f;
    public float smooth = 5f;

    public float maxX1 = 406f;
    public float minX1 = -23f;
    public float maxY1 = 8f;
    public float minY1 = 4f;

    public float maxX2 = 406f;
    public float minX2 = -23f;
    public float maxY2 = -78f;
    public float minY2 = -81f;

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
