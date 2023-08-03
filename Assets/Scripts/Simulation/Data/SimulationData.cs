using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationData : MonoBehaviour
{
    [Header("Simulation Data")]
    public float StartingTime = 0f;
    public float EndingTime = 0f;
    public float TimeScale = 1f;

    public float TotalFlightsCreated = 0f;
    public float TotalFlightsInitiated = 0f;
    public List<IncomingFlight> IFlights = new();
    public List<OutgoingFlight> OFlights = new();

    public int totalRepeats = 0;
    public int currentRepeat = 1;

    private AirportData airportData = new();
    private List<VirusData> virusData = new();
    private List<RecordedData> RData = new();

    [Header("Reducing Capacity ")]
    public int Bathroom1_capacity = 0;

    public AirportData GetAirportData() { return airportData; }
    public VirusData GetVirusData() { return virusData[currentRepeat]; }
    public List<RecordedData> GetRecordedData() { return RData; }

    public void IncreasedTotalFlightsGenerated() { TotalFlightsCreated++; }
    public void IncreasedTotalFlightsInitiated() { TotalFlightsInitiated++; }

    public void UpdateTimeScale(float newTimeScale) { TimeScale = newTimeScale; Time.timeScale = TimeScale; }

    public void ResetData()
    {
        StartingTime = Time.time;
        EndingTime = 0f;
        TimeScale = 1f;

        TotalFlightsCreated = 0f;
        TotalFlightsInitiated = 0f;
        currentRepeat++;



        airportData = new();
        RData = new();
    }
        
}
