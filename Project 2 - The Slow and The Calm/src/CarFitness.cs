using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarFitness : IFitness
    {
        public CarFitness()
        {
            ChromosomesToBeginEvaluation = new BlockingCollection<CarChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<CarChromosome>();
        }

        public BlockingCollection<CarChromosome> ChromosomesToBeginEvaluation { get; private set; }
        public BlockingCollection<CarChromosome> ChromosomesToEndEvaluation { get; private set; }
        public double Evaluate(IChromosome chromosome)
        {
            var c = chromosome as CarChromosome;
            ChromosomesToBeginEvaluation.Add(c);

            float fitness = 0;
            float best = 0; 
            do{
                Thread.Sleep(1000);

                /*YOUR CODE HERE: You should define de fitness function here!!
                 * 
                 * 
                 * You have access to the following information regarding how the car performed in the scenario:
                 * MaxDistance: Maximum distance reached by the car;
                 * MaxDistanceTime: Time taken to reach the MaxDistance;
                 * MaxVelocity: Maximum Velocity reached by the car;
                 * NumberOfWheels: Number of wheels that the cars has;
                 * CarMass: Weight of the car;
                 * IsRoadComplete: This variable has the value 1 if the car reaches the end of the road, 0 otherwise.
                 * 
                */
                float MaxDistance = c.MaxDistance;
                float MaxDistanceTime = c.MaxDistanceTime;
                float MaxVelocity = c.MaxVelocity;
                float NumberOfWheels = c.NumberOfWheels;
                float CarMass = c.CarMass;
                int IsRoadComplete = c.IsRoadComplete ? 1 : 0;

                // We have 6 different fitness functions to use.

                // Fitness 1:
                // fitness = IsRoadComplete;

                // Fitness 2:
                /*
                if(IsRoadComplete==1){
                    fitness = 700;

                } else
                    fitness = MaxDistance;
                */

                // Fitness 3:
                /*
                if(IsRoadComplete==1){
                    fitness = 700 + (200-MaxDistanceTime);

                } else
                    fitness = MaxDistance;
                */

                // Fitness 4:
                // fitness = 0.8f * MaxDistance + 0.2f * MaxVelocity - 0.2f * CarMass - 20 * NumberOfWheels + 200 * IsRoadComplete;

                // Fitness 5:
                // fitness = 0.4f * MaxDistance + 0.2f * MaxVelocity - 0.4f * CarMass - 10 * NumberOfWheels + 200 * IsRoadComplete;

                // Fitness 6:
                fitness = MaxDistance + 0.2f * MaxVelocity - 0.4f * CarMass - 10 * NumberOfWheels + 200 * IsRoadComplete;

                c.Fitness = fitness;
                

            } while (!c.Evaluated);

            ChromosomesToEndEvaluation.Add(c);

            do{
                Thread.Sleep(1000);
            } while (!c.Evaluated);


            return fitness;
        }

    }
}