using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrivingAgentsMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public ArrivingAgentsSpawner.ArrivingAgentSettings agentSettings;
    public ArrivingAgentsPathGenerator.AgentState agentState;
    public int EntryGateNumber;

    private ArrivingAgentsPathGenerator AgentPath;
    public bool TimeToBoard = false;
    private bool NeedsRestroom = false;
    private bool NeedsCheckIn = false;
    private bool NeedsShop = false;
    private bool NeedsEat = false;

    private float startTimeOfWaiting;
    private float CurrentWaitingTime = 0f;

    List<Vector3> destinations;
    List<float> waitTimes;
    ArrStatsData ArrStats;
    void Start()
    {
        //Initial State
        agentState = ArrivingAgentsPathGenerator.AgentState.None;

        //Randomly make them need to use the bathroom or not
        if (Random.value < agentSettings.ChanceToUseRestroom) NeedsRestroom = true;
        if (Random.value < agentSettings.ChanceToCheckIn) NeedsCheckIn = true;
        if (Random.value < agentSettings.ChanceToShop) NeedsShop = true;
        if (Random.value < agentSettings.ChanceToEat) NeedsEat = true;

        //Generate the path the agents will walk on based on the parameters
        AgentPath = new ArrivingAgentsPathGenerator(name, NeedsRestroom, NeedsCheckIn, NeedsShop, NeedsEat);
        destinations = AgentPath.Destinations[0];
        waitTimes = AgentPath.WaitTimes[0];
        agentState = AgentPath.States[0];
    }

    // Update is called once per frame
    void Update()
    {
        IsTimeToBoard();
        if (IsWaitingOver() && TimeToBoard == false) PathExecution();
    }

    void PathExecution()
    {
        if (destinations.Count != 0)
        {
            if (navMeshAgent.hasPath == false)
            {
                navMeshAgent.destination = destinations[0];
            }
            else
            {
                if (Vector3.Distance(transform.position, navMeshAgent.destination) < 2.5f)
                {
                    //Special Cases
                    if (agentState == ArrivingAgentsPathGenerator.AgentState.CheckIn) navMeshAgent.areaMask |= 1 << NavMesh.GetAreaFromName("AfterCheckIn");

                    navMeshAgent.ResetPath();
                    destinations.Remove(destinations[0]);
                    startTimeOfWaiting = Time.time;
                    CurrentWaitingTime = waitTimes[0];
                    waitTimes.Remove(waitTimes[0]);
                }
            }
        }
        else if (destinations.Count == 0)
        {
            if (AgentPath.Destinations.Count != 0)
            {
                AgentPath.Destinations.Remove(AgentPath.Destinations[0]);
                AgentPath.WaitTimes.Remove(AgentPath.WaitTimes[0]);
                AgentPath.States.Remove(AgentPath.States[0]);
            }

            if (AgentPath.Destinations.Count != 0)
            {
                destinations = AgentPath.Destinations[0];
                waitTimes = AgentPath.WaitTimes[0];
                agentState = AgentPath.States[0];
            }

        }
    }

    void IsTimeToBoard()
    {
        if (TimeToBoard == true)
        {
            agentState = ArrivingAgentsPathGenerator.AgentState.Board;
            navMeshAgent.ResetPath();
            navMeshAgent.destination = GameObject.Find("GatesArr/EntryGates/Gate" + EntryGateNumber.ToString()).transform.position;
            Destroy(gameObject);
        }
    }

    bool IsWaitingOver()
    {
        return (Time.time - startTimeOfWaiting >= CurrentWaitingTime);
    }
}
