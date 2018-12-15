using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;


namespace WebApiCoreCode
{
    public class OptimizationModel
    {
        GeneralizedAssignmentProblem problem;

        public GeneralizedAssignmentProblem readJson(string path) 
        {
            problem = JsonConvert.DeserializeObject<GeneralizedAssignmentProblem>(System.IO.File.ReadAllText(path));
            return problem;
        }

        public string writeSol(int[] sol) {
            return String.Join(',', sol)+ " " + this.getCost(sol);
        }

        public bool isSolValid(int[] sol) 
        {
            if (problem == null || sol == null || problem.numcli != sol.Length) return false;

            int[] capused = new int[problem.numserv];
            for (int j = 0; j < problem.numcli; j++)
            {
                if (sol[j] < 0 || sol[j] >= problem.numserv)
                    return false;

                capused[sol[j]] += problem.req[sol[j],j];
                if (capused[sol[j]] > problem.cap[sol[j]]) 
                    return false;
            }
            return true;
        }
        public int getCost(int[] sol)
        {  
            if (!isSolValid(sol)) return Int32.MaxValue;

            int z = 0;
            for (int j = 0; j < problem.numcli; j++)
                z += problem.cost[sol[j],j];
                
            return z;
            
            //int j = 0;
            //return sol.Sum(x => problem.cost[x,j++]);
        }
        private int[] getCapLeft(int[] sol)
        {
            int[] capLeft = (int[])problem.cap.Clone();
            for(int j=0;j<problem.numcli;j++)
            {  
                capLeft[sol[j]] -= problem.req[sol[j],j];
            }
            return capLeft;
        }

        public int[] findSol()
        {
            int[] sol = new int[problem.numcli];

            int[] ind = new int[problem.numserv];
            float[] keys = new float[problem.numserv];

            int[] capLeft = (int[])problem.cap.Clone();

            //for(int i=0;i<problem.numserv;i++) dist[i] = new Array(2);
            for(int j=0;j<problem.numcli;j++)
            {  
                sol[j] = -1;

                for(int i=0;i<problem.numserv;i++)
                {  
                    keys[i] = problem.req[i,j];//problem.cost[i,j]; // problem.req[i,j];
                    ind[i] = i;
                }
                Array.Sort(keys, ind);   
                for(int ii = 0;ii<problem.numserv;ii++)
                {  
                    int i = ind[ii];
                    if(capLeft[i] >= problem.req[i,j])
                    { 
                        capLeft[i] -= problem.req[i,j];
                        sol[j] = i;
                        break;
                    }
                }
                
            }
            return sol;
        }
        public int[] Gap10(int[] sol)
        {
            int[] capLeft = getCapLeft(sol);
            int[] optSol = (int[])sol.Clone();
            int optCost = getCost(sol);
            for(int j=0;j<problem.numcli;j++)
            {   
                for(int i = 0;i<problem.numserv;i++)
                {  
                    if(optSol[j]==i) continue;
                    int[] tmpsol = (int[])optSol.Clone(); 
                    tmpsol[j]= i;
                    
                    if(capLeft[i] >= problem.req[i,j])
                    {
                        int actualCost = optCost - problem.cost[optSol[j],j] + problem.cost[i,j];
                        
                        if(actualCost < optCost){
                            capLeft[optSol[j]] += problem.req[optSol[j],j];
                            capLeft[i] -= problem.req[i,j];

                            optSol = tmpsol;
                            optCost = actualCost;
                        }
                    }
                }
            }       
            return optSol;
        }
        public int[] SimulatedAnnealing(int[] sol)
        {
            int[] capLeft = getCapLeft(sol);
            int[] optSol = (int[])sol.Clone();
            int optCost = getCost(sol);
            
            int[] currentSol = (int[])optSol.Clone();
            int currentCost = optCost;

            Random r = new Random();
            //bool firstStep = true;
            double p = 0.6;
            double? T = null;
            
            int totalSteps = 0, step = 0;

            while(step <= 50 || p > 0.0001)
            {
                int[] newSol = (int[])currentSol.Clone();
                int j = r.Next(problem.numcli);
                int i = r.Next(problem.numserv);
                newSol[j] = i;
                if (capLeft[i] >= problem.req[i,j])
                {
                    int newCost = currentCost - problem.cost[currentSol[j],j] + problem.cost[i,j];

                    if(newCost<=currentCost){
                        capLeft[currentSol[j]] += problem.req[currentSol[j],j];
                        capLeft[i] -= problem.req[i,j];

                        currentSol = newSol;
                        currentCost = newCost;

                        if (newCost < optCost) {
                            optSol = newSol;
                            optCost = newCost;
                            step = 0;
                        }
                    } else {
                        if (!T.HasValue) 
                            T = -(newCost-currentCost)/Math.Log(p);
                        else
                            p = Math.Exp(-(newCost-currentCost)/T.Value);

                        if(r.Next()/(float)Int32.MaxValue<p){
                            capLeft[currentSol[j]] += problem.req[currentSol[j],j];
                            capLeft[i] -= problem.req[i,j];
                            
                            currentSol = newSol;
                            currentCost = newCost;
                        }
                    }
                }
            
                step++;
                totalSteps++;
                if(totalSteps % (problem.numcli*(problem.numcli-1)) == 0) T *=0.98;
            }
            return optSol;
        }

        /* public int[] TabuSearch(int[] sol, int tabuTenure, int maxIteration, int[,] tabuList = null, int actualIteration = 0)
        {

            if (tabuList == null) 
            {
                tabuList = initializeTabuList();
            }

                        
            int[] lessWorseSol = null;
            int lessWorseCost = Int32.MaxValue;

             for(int j=0;j<problem.numcli;j++)
            {   
                for(int i = 0;i<problem.numserv;i++)
                {  
                    if(sol[j] == i) continue;

                    int[] tmpsol = (int[])sol.Clone(); 
                    tmpsol[j]= i;
                    try
                    {
                        int cost = checkSol(tmpsol);
                        int oldcost = checkSol(sol);
                       

                        if (cost < oldcost && tabuList[j,i] <= actualIteration){
                            actualIteration++;
                            if (actualIteration == maxIteration) {
                                break;
                            }
                            tabuList[j,i] = actualIteration + tabuTenure;
                            return TabuSearch(tmpsol, tabuTenure, maxIteration, tabuList, actualIteration);
                        } else if (tabuList[j,i] <= actualIteration) {
                            if (lessWorseSol == null || cost < lessWorseCost) {
                                lessWorseSol = tmpsol;
                            } 
                        }
                    }
                    catch
                    {}
                }
            }   

            if (maxIteration == actualIteration) {
                 return sol;
            } else {
                actualIteration++;
                return TabuSearch(lessWorseSol, tabuTenure, maxIteration, tabuList, actualIteration);
            }
        } */

        public int[,] initializeTabuList()
         {
             int[,] tabuList = new int[problem.numserv, problem.numcli];

            /* riempire la tabuList di tutti 0 */
            /* for(int j=0;j<problem.numcli;j++)
            {   
                for(int i = 0;i<problem.numserv;i++)
                {
                    tabuList[i,j] = 0;
                }
            }*/
            
            //sono già a 0 se non si fa nulla

            return tabuList; 
         }

        public int[] TabuSearch2(int[] sol, int tabuTenure=10, int maxIteration=1000, int[] bestSolution = null, int[,] tabuList = null, int currentIteration = 0)
        {
            //bestSol = soluzione migliore nel vicinato
            //bestCurrentSolution = soluzione migliore in assoluto
            int[] capLeft = getCapLeft(sol);
            int currentCost = getCost(sol);
            int bestCost;
            if (currentIteration == maxIteration)
                 return bestSolution;

            if (tabuList == null) 
                tabuList = initializeTabuList();
                        
            int[] neighbourhoodBestSol = null;
            int neighbourhoodBestCost = Int32.MaxValue;

            if (bestSolution == null) 
            { 
                bestCost= currentCost;
                bestSolution = sol;
            }
            else  bestCost = getCost(bestSolution);

            for(int j=0;j<problem.numcli;j++)
            {   
                for(int i = 0;i<problem.numserv;i++)
                {  
                    if(sol[j] == i) continue;

                    int[] newSol = (int[])sol.Clone(); 
                    newSol[j]= i;
                    if (capLeft[i]>= problem.req[i,j])
                    {
                        int newCost = currentCost - problem.cost[sol[j],j] + problem.cost[i,j];
                        
                        if (newCost < bestCost || (newCost < neighbourhoodBestCost && tabuList[i,j] <= currentIteration)) {
                            tabuList[i,j] = currentIteration + tabuTenure;
                            neighbourhoodBestSol = newSol;    
                            neighbourhoodBestCost = newCost; 
                        }
                    }
                }
            }

            /* int[] oldSol = sol;
            int oldCost = checkSol(sol);
         
            if (neighbourhoodBestCost < oldCost) {
                oldSol = neighbourhoodBestSol;
                oldCost = neighbourhoodBestCost; 
            }

            if (bestCost < oldCost) {
                bestSolution = oldSol;
            }*/
            if (neighbourhoodBestSol == null)
                neighbourhoodBestSol = sol;
            if (neighbourhoodBestCost < bestCost) {
                bestSolution = neighbourhoodBestSol;
            }

            return TabuSearch2(neighbourhoodBestSol, tabuTenure, maxIteration, bestSolution, tabuList, currentIteration + 1);
            
        }
    }
}