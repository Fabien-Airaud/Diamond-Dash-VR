using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    public float sectionSize = 187.5f;
    public int nbSections = 5;
    
    private Queue<GameObject> sections;


    void Start()
    {
        if (sectionPrefabs.Length == 0)
        {
            Debug.LogError("No section prefabs found");
            return;
        }
        if (nbSections < 1)
        {
            Debug.LogError("Number of sections must be greater than 0");
            return;
        }

        sections = new();

        int middle = (nbSections - 1) / 2;
        // Generate the first sections (back)
        for (int i = middle; i > 0; i--)
        {
            int randomIndex = Random.Range(0, sectionPrefabs.Length);
            Debug.Log("i = " + i + ";\t\tVector3(0, 0, -i * sectionSize) = (0, 0, " + (-i * sectionSize) + ")");
            sections.Enqueue(Instantiate(sectionPrefabs[randomIndex], new Vector3(0, 0, -i * sectionSize), Quaternion.identity));
        }

        // Add the middle section (startSection from the scene)
        sections.Enqueue(GameObject.FindGameObjectWithTag("Section"));

        // Generate the last sections (front)
        for (int i = 1; i < middle + 1; i++)
        {
            int randomIndex = Random.Range(0, sectionPrefabs.Length);
            Debug.Log("i = " + i + ";\t\tVector3(0, 0, -i * sectionSize) = (0, 0, " + (i * sectionSize) + ")");
            sections.Enqueue(Instantiate(sectionPrefabs[randomIndex], new Vector3(0, 0, i * sectionSize), Quaternion.identity));
        }
    }

    void Update()
    {
        
    }
}
