using UnityEngine;
using UnityEngine.UI;

public class FPSMeter : MonoBehaviour
{
    public GameObject Text;

    private float[] Samples;
    private int SamplesIndex;

	private void Start ()
    {
        Samples = new float[60];
        SamplesIndex = 0;
	}

    private void Update()
    {
        Samples[SamplesIndex] = 1 / Time.deltaTime;
        SamplesIndex = (SamplesIndex + 1) % 60;
        Text.GetComponent<Text>().text = "FPS = " + AverageFPS().ToString();
    }

    private int AverageFPS()
    {
        float sum = 0;
        for (int i = 0; i < Samples.Length; i++)
        {
            sum += Samples[i];
        }
        return Mathf.RoundToInt(sum / Samples.Length);
    }
}
