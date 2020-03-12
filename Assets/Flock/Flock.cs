using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : LifeManager
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBihavior behavior;

    [Range(2, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.025f;

    [Range(0f, 100f)]
    public float driveFactor = 10f;
    [Range(0f, 10f)]
    public float maxSpeed = 5f;
    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplayer = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    public float SquareNeighborRadius { get { return squareNeighborRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplayer * avoidanceRadiusMultiplayer;

        //CreateLife(1); //Méthode appelée depuis le GlobalLifeManager
    }

    public override void CreateLife(State currentState)
    {
        float health = 1; //TODO implémenter un truc qui fait du sens
        int realCount = (int)(health * startingCount);
        int delta = realCount - agents.Count;

        if (delta > 0)
        {
            GenerateAgents(delta);
        }
        else
        {
            RemoveAgents(-delta);
        }
    }

    public void GenerateAgents(int count)
    {
        int originalCount = agents.Count;

        //Crée tous les poissons et donne les noms
        for (int i = 0; i < count; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity + transform.position,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent" + (i + originalCount);
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    public void RemoveAgents(int count)
    {
        while (count > 0 && agents.Count != 0)
        {
            FlockAgent agent = agents[agents.Count - 1];
            Destroy(agent.gameObject);
            agents.Remove(agent);
            count--;
        }
    }


    void Update() // changé depuis FixedUpdate
    {
        //Fait bouger les poissons en fonctions des voisins
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    //Crée une liste de transformation en fct des voisins du poisson
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

}
