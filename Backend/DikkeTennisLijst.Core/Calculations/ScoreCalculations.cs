using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Constants;
using DikkeTennisLijst.Core.Shared.Enums;
using Bonus = DikkeTennisLijst.Core.Shared.Constants.BonusConstants;

namespace DikkeTennisLijst.Core.Calculations
{
    public static class ScoreCalculations
    {
        /// <summary>
        /// Calculates the expected score, based on the players elo's and the match format.
        /// </summary>
        /// <param name="player">The elo of the player for which to calculate the expected score</param>
        /// <param name="opponent">The elo of the opponent.</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static double CalculateExpectedScore(int playerElo, int opponentElo, MatchFormat format)
        {
            var expectedScore = CalculateExpectedScorePerSet(playerElo, opponentElo);

            return format switch
            {
                MatchFormat.BestOf1 => expectedScore,
                MatchFormat.BestOf3 => BestOf3(expectedScore),
                MatchFormat.BestOf5 => BestOf5(expectedScore),
                MatchFormat.BestOf7 => BestOf7(expectedScore),
                _ => throw new ArgumentOutOfRangeException(nameof(format)),
            };
        }

        /// <summary>
        /// Calculates the expected score, based on the players elo's.
        /// </summary>
        /// <param name="player">The elo of the player for which to calculate the expected score</param>
        /// <param name="opponent">The elo of the opponent.</param>
        /// <param name="format"></param>
        /// <returns></returns>
        private static double CalculateExpectedScorePerSet(int playerElo, int opponentElo)
        {
            return 1 / (double)(1 + (double)Math.Pow(10, ((double)opponentElo - playerElo) / EloConstants.Diff));
        }

        /// <summary>
        /// Calculates the winning chance for a best of 3, when the chance to win a set is provided.
        /// q = 1-p;
        /// x = p*p*(p + 3*q);
        /// </summary>
        /// <param name="expectedScore">The chance to win a single set</param>
        /// <returns></returns>
        private static double BestOf3(double expectedScore)
        {
            double E = expectedScore;
            double I = 1 - expectedScore;
            return Math.Pow(E, 2) * (E + (3 * I));
        }

        /// <summary>
        /// Calculates the winning chance for a best of 5, when the chance to win a set is provided.
        /// q = 1-p;
        /// x = p*p*p*(p*p + 5*p*q + 10*q*q);
        /// </summary>
        /// <param name="expectedScore">The chance to win a single set</param>
        /// <returns></returns>
        private static double BestOf5(double expectedScore)
        {
            double E = expectedScore;
            double I = 1 - expectedScore;
            return Math.Pow(E, 3) * (Math.Pow(E, 2) + (5 * E * I) + (10 * Math.Pow(I, 2)));
        }

        /// <summary>
        /// Calculates the winning chance for a best of 7, when the chance to win a set is provided.
        /// q = 1-p;
        /// x = p*p*p*p*(p*p*p + 7*p*p*q + 21*p*q*q + 35*q*q*q);
        /// </summary>
        /// <param name="expectedScore">The chance to win a single set</param>
        /// <returns></returns>
        private static double BestOf7(double expectedScore)
        {
            double E = expectedScore;
            double I = 1 - expectedScore;
            return Math.Pow(E, 4) * (Math.Pow(E, 3) + (7 * Math.Pow(E, 2) * I) + (21 * E * Math.Pow(I, 2)) + (35 * Math.Pow(I, 3)));
        }

        public static (double PlayerOne, double PlayerTwo) CalculateActualScore(Match match)
        {
            var (scorePlayerOne, scorePlayerTwo) = CalculateScoreTotals(match);
            return CalculateActualScore(scorePlayerOne, scorePlayerTwo);
        }

        private static (int PlayerOne, int PlayerTwo) CalculateScoreTotals(Match match)
        {
            int scorePlayerOne = 0;
            int scorePlayerTwo = 0;

            foreach (var set in match.Sets)
            {
                if (set.GamesP1.HasValue && set.GamesP2.HasValue)
                {
                    scorePlayerOne += set.GamesP1.Value * Bonus.Game;
                    scorePlayerTwo += set.GamesP2.Value * Bonus.Game;

                    scorePlayerOne += set.GamesP1 > set.GamesP2 ? Bonus.Set : 0;
                    scorePlayerTwo += set.GamesP2 > set.GamesP1 ? Bonus.Set : 0;

                    scorePlayerOne += set.IsTieBreaker && set.GamesP1 > set.GamesP2 ? Bonus.TieBreaker : 0;
                    scorePlayerTwo += set.IsTieBreaker && set.GamesP2 > set.GamesP1 ? Bonus.TieBreaker : 0;
                }

                scorePlayerOne += set.IsSuperTieBreaker && set.PointsP1 > set.PointsP2 ? Bonus.SuperTieBreaker : 0;
                scorePlayerTwo += set.IsSuperTieBreaker && set.PointsP2 > set.PointsP1 ? Bonus.SuperTieBreaker : 0;
            }

            scorePlayerOne += match.Winner == MatchWinner.PlayerOne ? Bonus.Match : 0;
            scorePlayerTwo += match.Winner == MatchWinner.PlayerTwo ? Bonus.Match : 0;

            return (scorePlayerOne, scorePlayerTwo);
        }

        private static (double, double) CalculateActualScore(int scoreTotalP1, int scoreTotalP2)
        {
            var actualScoreP1 = scoreTotalP1 > scoreTotalP2
                ? (1 - (scoreTotalP2 / (double)scoreTotalP1 / 2))
                : (scoreTotalP1 / (double)scoreTotalP2 / 2);

            return ((double)actualScoreP1, 1 - (double)actualScoreP1);
        }
    }
}