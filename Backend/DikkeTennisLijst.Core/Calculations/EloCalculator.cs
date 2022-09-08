using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Interfaces.Entities;

namespace DikkeTennisLijst.Core.Calculations
{
    public static class EloCalculator
    {
        /// <summary> Updates the provided elo's.</summary>
        /// <remarks> Player's elo is not updated if his elo is not provisional, while his opponent's elo is.</remarks>
        /// <param name="match">Match for which to update the elo's</param>
        public static void UpdateElosForMatch(IElo eloPlayerOne, IElo eloPlayerTwo, Match match)
        {
            double expectedScoreP1 = ScoreCalculations.CalculateExpectedScore(eloPlayerOne.Rating, eloPlayerTwo.Rating, match.Format);
            double expectedScoreP2 = 1 - expectedScoreP1;

            var (actualScoreP1, actualScoreP2) = ScoreCalculations.CalculateActualScore(match);

            int newEloRatingP1 = CalculateNewElo(eloPlayerOne, eloPlayerTwo, expectedScoreP1, actualScoreP1);
            int newEloRatingP2 = CalculateNewElo(eloPlayerTwo, eloPlayerOne, expectedScoreP2, actualScoreP2);

            // 2 Update player 1 elo;
            eloPlayerOne.History.Add(new EloHistory(match.Id, newEloRatingP1));
            eloPlayerOne.Rating = newEloRatingP1;
            eloPlayerOne.K = EloCalculations.CalculateNewK(eloPlayerOne);
            if (eloPlayerOne.IsProvisional) eloPlayerOne.IsProvisional = eloPlayerOne.ShouldBeProvisional();

            // 3 Update player 2 elo;
            eloPlayerTwo.History.Add(new EloHistory(match.Id, newEloRatingP2));
            eloPlayerTwo.Rating = newEloRatingP2;
            eloPlayerTwo.K = EloCalculations.CalculateNewK(eloPlayerTwo);
            if (eloPlayerTwo.IsProvisional) eloPlayerTwo.IsProvisional = eloPlayerTwo.ShouldBeProvisional();
        }

        private static int CalculateNewElo(IElo currentElo, IElo opponentElo, double expectedScore, double actualScore)
        {
            return !currentElo.IsProvisional && opponentElo.IsProvisional
                ? currentElo.Rating
                : EloCalculations.CalculateNewElo(expectedScore, actualScore, currentElo.Rating, currentElo.K);
        }

        /// <summary>
        /// Stap 1. Zodra het systeem aan gaat, zijn alle elo's non-provisional.
        /// Stap 2. Zodra er een aantal spelers met een fatsoenlijk bepaalde elo zijn, worden elo's eerst provisional en wordt deze method geraakt.
        /// Stap 3. Zodra er veel spelers zijn met een fatsoenlijk bepaalde elo, moet deze worden uitgebreid, zodat je bijvoorbeeld in ieder geval
        /// een keer van een non-provisional moet hebben gewonnen EN hebben verloren, voordat je elo non-provisional wordt. Deze method zal tegen
        /// die tijd een stuk meer informatie nodig hebben.
        /// </summary>
        /// <param name="elo"></param>
        /// <returns></returns>
        private static bool ShouldBeProvisional(this IElo elo) => elo.History.Count < 3;
    }
}