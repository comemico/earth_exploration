using UnityEngine;
using UnityEngine.UI;

public class FpsChecker : MonoBehaviour
{
    /*
    public Text fpsText;

    // Update()���Ă΂ꂽ�񐔂��J�E���g���܂��B
    int frameCount;

    // �O��t���[�����[�g��\�����Ă���̌o�ߎ��Ԃł��B
    float elapsedTime;

    void Update()
    {
        // �Ă΂ꂽ�񐔂����Z���܂��B
        frameCount++;

        // �O�̃t���[������̌o�ߎ��Ԃ����Z���܂��B
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1.0f)
        {
            // �o�ߎ��Ԃ�1�b�𒴂��Ă�����A�t���[�����[�g���v�Z���܂��B
            float fps = 1.0f * frameCount / elapsedTime;

            // �v�Z�����t���[�����[�g����ʂɕ\�����܂��B(�����_�ȉ�2�P�^�܂�)
            string fpsRate = $"FPS: {fps.ToString("F2")}";
            fpsText.text = fpsRate;

            // �t���[���̃J�E���g�ƌo�ߎ��Ԃ����������܂��B
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
    */

    /// <summary>
    /// Reflect measurement results every 'EveryCalcurationTime' seconds.
    /// EveryCalcurationTime �b���ƂɌv�����ʂ𔽉f����
    /// </summary>
    [SerializeField, Range(0.1f, 1.0f)]
    float EveryCalcurationTime = 0.5f;
    Text fpsText;

    /// <summary>
    /// FPS value
    /// </summary>
    public float Fps
    {
        get; private set;
    }

    int frameCount;
    float prevTime;

    void Start()
    {
        fpsText = GetComponent<Text>();
        frameCount = 0;
        prevTime = 0.0f;
        Fps = 0.0f;
    }
    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        // n�b���ƂɌv��
        if (time >= EveryCalcurationTime)
        {
            Fps = frameCount / time;
            string fpsRate = $"FPS: {Fps.ToString("F2")}";
            fpsText.text = fpsRate;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }



}
