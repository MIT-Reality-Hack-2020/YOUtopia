using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CSVFileReader : MonoBehaviour
{
    public GameObject parentObj;
    public TextAsset csvFile;

    // Update is called once per frame
    void Update()
    {
        readCSV();
    }

    void readCSV()
    {
        string[] records = csvFile.text.Split('\n');
        for (int i = 1; i < records.Length; i=i+100)
        {
            string[] fields = records[i].Split(',');
            parentObj.transform.Rotate(float.Parse(fields[1]), float.Parse(fields[1]), float.Parse(fields[1]));
            parentObj.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
