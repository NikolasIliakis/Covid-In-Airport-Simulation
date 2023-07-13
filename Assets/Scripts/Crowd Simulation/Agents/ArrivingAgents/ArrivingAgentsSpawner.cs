using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrivingAgentsSpawner : MonoBehaviour
{

    [Serializable]
    public class ArrivingAgentSettings
    {
        [Range(0f, 1f)]
        public float ChanceToUseRestroom;
        [Range(0f, 1f)]
        public float ChanceToCheckIn;
        [Range(0f, 1f)]
        public float ChanceToShop;
        [Range(0f, 1f)]
        public float ChanceToEat;
    }
    public ArrivingAgentSettings AgentSettings = new();

    [Serializable]
    public class ArrivingAgentSpawnerSettings
    {
        public GameObject agentPrefab; // Prefab of the agent to spawn
        public GameObject Parent;
        public float minSpawnDelay; // Minimum delay between spawns
        public float maxSpawnDelay; // Maximum delay between spawns
        public int minSpawnCount;
        public int maxSpawnCount;
        public float waitTime;
    }
    public ArrivingAgentSpawnerSettings ArrivingSpawnerSettings = new();

    private int FlightNo = 0;
    private List<GameObject> agents = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartAFlight());
    }
    private IEnumerator StartAFlight()
    {
        
        while (true)
        {
            if(agents.Count != 0 )
            {
                foreach (GameObject agent in agents)
                {
                    ArrivingAgentsMovement agentScript = agent.GetComponent<ArrivingAgentsMovement>();
                    agentScript.TimeToBoard = true;
                }

                agents.Clear();
            }

            int agentCount = Random.Range(ArrivingSpawnerSettings.minSpawnCount, ArrivingSpawnerSettings.maxSpawnCount);
            Color randomColor = Random.ColorHSV();
            FlightNo++;

            int agentGate = Random.Range(1, 4); 

            for (int i = 0; i < agentCount; i++)
            {
                Vector3 AgentPos = transform.position;
                AgentPos.x = AgentPos.x + Random.Range(-40f, 40f);
                GameObject newAgent = Instantiate(ArrivingSpawnerSettings.agentPrefab, AgentPos, Quaternion.identity);
                agents.Add(newAgent);

                //Set Parent for agents so that its organized
                newAgent.transform.parent = ArrivingSpawnerSettings.Parent.transform;

                //Agent Number and Flight Number
                newAgent.name = "FlightNo" + FlightNo.ToString() + "_AgentNo" + i.ToString();

                //Set color for this agent so all agents of this flight have the same color
                Renderer agentRenderer = newAgent.GetComponent<Renderer>();
                agentRenderer.material.color = randomColor;

                ArrivingAgentsMovement agentScript = newAgent.GetComponent<ArrivingAgentsMovement>();
                agentScript.EntryGateNumber = agentGate;
                agentScript.agentSettings = AgentSettings;

                //Wait a little bit until next Agent
                float spawnDelay = Random.Range(0.1f, 2f);
                yield return new WaitForSeconds(spawnDelay);
            }

            //Wait until next flight is ready
            float randomSpawnInterval = Random.Range(ArrivingSpawnerSettings.minSpawnDelay, ArrivingSpawnerSettings.maxSpawnDelay);
            yield return new WaitForSeconds(randomSpawnInterval);
            yield return new WaitForSeconds(ArrivingSpawnerSettings.waitTime);
        }
    }
}