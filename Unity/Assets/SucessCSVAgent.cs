using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SucessCSVAgent : MonoBehaviour
{
    //public GameObject parentObj;
    //[Tooltip("Effects 'H' in color")]
    public TextAsset emotionDataFile;
    //[Tooltip("Effects 'S' in color")]
    //public TextAsset dataFile2;
    //[Tooltip("Effects 'V' in color")]
    //public TextAsset dataFile3;
    //private int rsIdx;
    private float[] emotionValue;
    //private float[] emotionValue2;
    //private float[] emotionValue3;
    //private int counter;

    //data animation keyframe&curve
    AnimationCurve emotionCurve;
    //AnimationCurve dataCurve2;
    //AnimationCurve dataCurve3;

    Keyframe[] emotionKs;
    //Keyframe[] emotionKs2;
    //Keyframe[] emotionKs3;

    //modifier
    public bool scale = false;
    public bool position = false;
    public bool rotation = false;
    public bool changeColor = false;


    [Tooltip("effect object size")]
    public float scaleAmplifier = 1.0f;
    [Tooltip("effect rotation angleSize per update")]
    public float rotateAmplifier = 1.0f;
    [Tooltip("effect rising step per update")]
    public float stepMax = 7.0f;
    float risingStep = 1.0f;

    //public Vector3 emotionVector;
    //public Vector3 scaleVector3;
    //public Vector3 rotateVector3;

    [Tooltip("effect general changing speed")]
    public float emotionSpeedMultiplier = 1.0f;

    //avoid disappearance
    //private float emotionOffset = 2.0f;

    //mapping
    private float emoMin, emoMax, sinFactor;
    public float HRate = 1.0f, SRate = 1.0f, VRate = 1.0f, aRate = 0.5f;
    public float Hmin = 0.0f, Smin = 0.0f, Vmin = 0.0f, aMin = 0.0f;
    public float Hmax = 1.0f, Smax = 1.0f, Vmax = 1.0f;

    //color
    private Vector4 colorVector4;
    //[Tooltip("changing range for Hue, Saturation, Brightness")]
    //public float H_Min = 0.0f, H_Max = 1.0f, S_Min = 0.0f, S_Max = 1.0f, V_Min = 0.0f, V_Max = 1.0f;
    //public float a_Min = 0.0f, a_Max = 0.5f;

    //===========
    //Audio
    public AudioSource channel;


    // Update is called once per frame
    private void Start()
    {
        readCSV();

        if (channel != null)
        {
            channel = GetComponent<AudioSource>();
        }

        //channel.Play();
    }


    void readCSV()
    {
        string[] engagementRecords = emotionDataFile.text.Split('\n');
        //string[] engagementRecords2 = dataFile2.text.Split('\n');
        //string[] engagementRecords3 = dataFile3.text.Split('\n');

        emotionValue = new float[engagementRecords.Length - 2];
        //emotionValue2 = new float[engagementRecords.Length - 2];
        //emotionValue3 = new float[engagementRecords.Length - 2];

        for (int i = 1; i < engagementRecords.Length - 1; i++)
        {
            string[] fields = engagementRecords[i].Split(',');
            //string[] fields2 = engagementRecords2[i].Split(',');
            //string[] fields3 = engagementRecords3[i].Split(',');
            //Debug.Log(float.Parse(fields[1]));
            emotionValue[i - 1] = float.Parse(fields[1]);
            //emotionValue2[i - 1] = float.Parse(fields2[1]);
            //emotionValue3[i - 1] = float.Parse(fields3[1]);

            //Vector3 rs = Vector3.one * float.Parse(fields[1]);
            //parentObj.transform.localScale = rs;
            //parentObj.transform.Rotate(float.Parse(fields[1]), float.Parse(fields[1]), float.Parse(fields[1]));
            //GetComponent<Renderer>().material.color = Color.green;   
        }
        //Vector3 rs = Vector3.one * aggressionValue[rsIdx];

        emoMin = Mathf.Min(emotionValue);
        emoMax = Mathf.Max(emotionValue);

        //initialized keyframe arrays
        emotionKs = new Keyframe[emotionValue.Length];
        //emotionKs2 = new Keyframe[emotionValue.Length];
        //emotionKs3 = new Keyframe[emotionValue.Length];

        for (var i = 0; i < emotionKs.Length; i++)
        {
            emotionKs[i] = new Keyframe(i, Map(emotionValue[i], emoMin, emoMax, -1.0f, 1.0f));
            //emotionKs2[i] = new Keyframe(i, Map(emotionValue2[i], emoMin, emoMax, -1.0f, 1.0f));
            //emotionKs3[i] = new Keyframe(i, Map(emotionValue3[i], emoMin, emoMax, -1.0f, 1.0f));
        }

        //store keys in curve
        emotionCurve = new AnimationCurve(emotionKs);
        //dataCurve2 = new AnimationCurve(emotionKs2);
        //dataCurve3 = new AnimationCurve(emotionKs3);

        //parentObj.transform.localScale = rs/10;
    }

    float Map(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        return NewMin + (OldValue - OldMin) * (NewMax - NewMin) / (OldMax - OldMin);

    }


    void Update()
    {
        //update vector by time
        //mapping aggresion data -> sin(x) ranges from 1 - 3
        sinFactor = (1 + Mathf.Sin(Time.time * emotionSpeedMultiplier)) / 2; //0-1

        //emotionVector = (Vector3.one * (emotionOffset + Mathf.Sin(Time.time * emotionSpeedMultiplier)) * emotionCurve.Evaluate(Time.time)).normalized;


        float emotionSinValue = sinFactor * emotionCurve.Evaluate(Time.time);
        float HueSinValue = Mathf.Clamp(sinFactor * emotionCurve.Evaluate(Time.time), Hmin, Hmax);
        float SaturationSinValue = Mathf.Clamp(sinFactor * emotionCurve.Evaluate(Time.time), Smin, Smax);
        float AudioSinValue = Mathf.Clamp(sinFactor * emotionCurve.Evaluate(Time.time), aMin, 1.0f);
        float BrightnessSinValue = Mathf.Clamp(sinFactor * emotionCurve.Evaluate(Time.time), Vmin, Vmax);

        //Mathf.Clamp((Mathf.Sin(Time.time * emotionSpeedMultiplier)) * emotionCurve.Evaluate(Time.time), xMin, xMax), H_Min, H_Max);
        //float SaturationSinValue = Mathf.Clamp((Mathf.Sin(Time.time * emotionSpeedMultiplier)) * dataCurve2.Evaluate(Time.time), xMin, xMax), S_Min, S_Max);
        //float BrightnessSinValue = Mathf.Clamp((Mathf.Sin(Time.time * emotionSpeedMultiplier)) * dataCurve3.Evaluate(Time.time), xMin, xMax), V_Min, V_Max);
        //float AudioSinValue = (0.5f + Mathf.Sin(Time.time * emotionSpeedMultiplier)) * emotionCurve.Evaluate(Time.time), a_Min, a_Max);

        //transform float into vector
        Vector3 emotionVector = Vector3.one * (emotionSinValue);
        risingStep = Mathf.Clamp(sinFactor * emotionCurve.Evaluate(Time.time * emotionSpeedMultiplier), 0.0f, stepMax);

        if (changeColor == true)
        {
            colorVector4 = Color.HSVToRGB(HueSinValue, SaturationSinValue, BrightnessSinValue);
        }

        //risingStep = Mathf.Clamp(emotionCurve.Evaluate(Time.time * emotionSpeedMultiplier), 0.0f, stepMax);
        //risingStep = emotionVector * stepMax;
        //risingStep = emotionSinValue * stepMax;

        //amplifier
        //Vector3 scaleVector3 = emotionVector * scaleAmplifier;
        //Vector3 rotateVector3 = emotionVector * rotateAmplifier;

        //update transform
        if (scale == true)
        {
            //transform.localScale = scaleVector3;
            transform.localScale = emotionVector * scaleAmplifier;
        }
        if (rotation == true)
        {
            //transform.Rotate(rotateVector3);
            transform.Rotate(emotionVector * rotateAmplifier);
        }

        if (position == true)
        {
            
            transform.position = Vector3.up * risingStep + new Vector3(transform.position.x, 0, transform.position.z);
        }

        //update color
        GetComponent<Renderer>().material.color = colorVector4;


        //=======
        //Audio Mixer
        if (channel != null)
        {
            channel.volume = AudioSinValue;
            Debug.Log(AudioSinValue);
        }


        //update index
        /*counter++;
         if (counter % 20 == 0)
         {

             rsIdx++;
             if (rsIdx >= aggressionValue.Length)
             {
                 rsIdx = 0;
             }
         }*/

    }
}
