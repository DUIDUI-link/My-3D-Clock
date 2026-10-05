using UnityEngine;

public class ClockController : MonoBehaviour
{
    public Transform hourPivot;
    public Transform minutePivot;
    public Transform secondPivot;

    public float speedMultiplier = 1f;

    [Header("角度报时")]
    public float chimeAngle = 0f;
    public AudioSource audioSource;
    public AudioClip chimeClip;

    private float totalSeconds;
    private float lastSecondAngle;

    void Start()
    {
        totalSeconds = 0f;
        lastSecondAngle = GetSecondAngle();
    }

    void Update()
    {
        totalSeconds += Time.deltaTime * speedMultiplier;

        float sec = totalSeconds % 60f;
        float min = (totalSeconds / 60f) % 60f;
        float hour = (totalSeconds / 3600f) % 12f;

        if (secondPivot != null)
            secondPivot.localRotation = Quaternion.Euler(0, 0, -sec * 6f);

        if (minutePivot != null)
            minutePivot.localRotation = Quaternion.Euler(0, 0, -min * 6f);

        if (hourPivot != null)
            hourPivot.localRotation = Quaternion.Euler(0, 0, -hour * 30f);

        float currentSecondAngle = GetSecondAngle();
        if (CrossedAngle(lastSecondAngle, currentSecondAngle, chimeAngle))
        {
            PlayChime();
        }
        lastSecondAngle = currentSecondAngle;
    }

    float GetSecondAngle()
    {
        return Mathf.Repeat(totalSeconds * 6f, 360f);
    }

    bool CrossedAngle(float from, float to, float target)
    {
        from = Mathf.Repeat(from, 360f);
        to = Mathf.Repeat(to, 360f);
        target = Mathf.Repeat(target, 360f);

        if (from > to)
            return target > from || target <= to;
        else
            return target > from && target <= to;
    }

    void PlayChime()
    {
        if (audioSource != null && chimeClip != null)
            audioSource.PlayOneShot(chimeClip);

        Debug.Log("角度报时！当前秒针角度跨过：" + chimeAngle);
    }
}