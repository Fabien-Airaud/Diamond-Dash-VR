using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject player;
    public GameObject[] sectionPrefabs;
    public float sectionSize = 187.5f;
    public int nbSections = 5;
    
    private Queue<GameObject> sections;
    private float middleSectionZPosition;


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
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        sections = new();
        GenerateInitialSections();
    }

    void Update()
    {
        UpdateSections();
    }


    private void GenerateInitialSections()
    {
        int middle = (nbSections - 1) / 2;

        // Generate the first sections (back)
        for (int i = middle; i > 0; i--)
        {
            int randomIndex = Random.Range(0, sectionPrefabs.Length);
            sections.Enqueue(Instantiate(sectionPrefabs[randomIndex], new Vector3(0, 0, -i * sectionSize), Quaternion.identity));
        }

        // Add the middle section (startSection from the scene)
        GameObject middleSection = GameObject.FindGameObjectWithTag("Section");
        middleSectionZPosition = 0;
        middleSection.transform.position = new Vector3(0, 0, middleSectionZPosition);
        sections.Enqueue(middleSection);

        // Generate the last sections (front)
        for (int i = 1; i < middle + 1; i++)
        {
            int randomIndex = Random.Range(0, sectionPrefabs.Length);
            sections.Enqueue(Instantiate(sectionPrefabs[randomIndex], new Vector3(0, 0, i * sectionSize), Quaternion.identity));
        }
    }

    private void UpdateSections()
    {
        if (player.transform.position.z > middleSectionZPosition + sectionSize / 2)
        {
            // Move the back section to the front
            GameObject section = sections.Dequeue();
            float newZPosition = section.transform.position.z + nbSections * sectionSize;
            section.transform.position = new Vector3(0, 0, newZPosition);
            sections.Enqueue(section);

            // Update the middle section z position
            middleSectionZPosition += sectionSize;
        }
    }
}
