using DikkeTennisLijst.Core.Interfaces.Entities;

namespace DikkeTennisLijst.Core.Calculations
{
    public static class EloCalculations
    {
        public static int CalculateNewElo(double expectedScore, double actualScore, int currentElo, int K)
        {
            return (int)Math.Round(currentElo + (K * (actualScore - expectedScore)));
        }

        // Todo: implement a dampening/boosting system for K.
        // For example: if a player has been getting results within expected ranges for a while, lower K.
        public static int CalculateNewK(IElo elo) => elo.K;
    }
}