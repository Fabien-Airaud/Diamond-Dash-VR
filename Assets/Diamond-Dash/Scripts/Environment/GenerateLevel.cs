using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    public float sectionSize = 187.5f;
    public int nbSections = 5;

    public float packsMinDistance = 10f;
    public GameObject[] pack1LaneAvoidable;
    public GameObject[] pack1LaneNonAvoidable;
    public GameObject[] pack2LanesAvoidable;
    public GameObject[] pack2LanesNonAvoidable;
    public GameObject[] pack3LanesAvoidable;
    public GameObject[] pack3LanesNonAvoidable;
    
    private GameObject player;
    private Queue<GameObject> sections;
    private float middleSectionZPosition;
    private float lastPackZPosition;

    private enum PackChoice
    {
        PACK1_LANE,
        PACK2_LANES,
        PACK3_LANES,
        PACK2_1_LANE,
        PACK3_1_LANE_MIN1_AVOIDABLE/*,
        PACK3_1_2_LANE_MIN1_AVOIDABLE*/
    }


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
        player = GameObject.FindGameObjectWithTag("Player");

        sections = new();
        GenerateInitialSections();
        GeneratePack();
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
            int randomIndex = UnityEngine.Random.Range(0, sectionPrefabs.Length);
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
            int randomIndex = UnityEngine.Random.Range(0, sectionPrefabs.Length);
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

    private PackChoice GetPackChoice()
    {
        PackChoice[] packChoices = (PackChoice[])Enum.GetValues(typeof(PackChoice));
        int random = UnityEngine.Random.Range(0, packChoices.Length);
        return packChoices[random];
    }

    private RoadPosition GetRandomRoadPosition(RoadPosition[] except)
    {
        List<RoadPosition> roadPositions = new((RoadPosition[])Enum.GetValues(typeof(RoadPosition)));
        foreach (RoadPosition roadPosition in except) roadPositions.Remove(roadPosition);
        int random = UnityEngine.Random.Range(0, roadPositions.Count);
        return roadPositions[random];
    }

    private void GeneratePack1Lane()
    {
        List<GameObject> pack = new(pack1LaneAvoidable);
        pack.AddRange(pack1LaneNonAvoidable);
        if (pack.Count == 0) return;

        float zPosition = lastPackZPosition + packsMinDistance;
        RoadPosition roadPosition = GetRandomRoadPosition(new RoadPosition[0]);
        GameObject gameObject = pack[UnityEngine.Random.Range(0, pack.Count)];
        Vector3 position = new Vector3(LevelBoundary.laneSize * (int)roadPosition, 0, zPosition) + gameObject.transform.position;

        Instantiate(gameObject, position, gameObject.transform.rotation);
        lastPackZPosition = zPosition;
    }

    private void GeneratePack()
    {
        if (player.transform.position.z + nbSections * sectionSize < lastPackZPosition) return;

        PackChoice packChoice = PackChoice.PACK1_LANE;
        //PackChoice packChoice = GetPackChoice();
        switch (packChoice)
        {
            case PackChoice.PACK1_LANE:
                GeneratePack1Lane();
                break;
            //case PackChoice.PACK2_LANES:
            //    GeneratePack2Lanes();
            //    break;
            //case PackChoice.PACK3_LANES:
            //    GeneratePack3Lanes();
            //    break;
            //case PackChoice.PACK2_1_LANE:
            //    GeneratePack2Lanes1Lane();
            //    break;
            //case PackChoice.PACK3_1_LANE_MIN1_AVOIDABLE:
            //    GeneratePack3Lanes1LaneMin1Avoidable();
            //    break;
            default:
                Debug.LogError("Pack choice not found");
                break;
        }
    }
}
