using DikkeTennisLijst.Core.Calculations;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Constants;
using DikkeTennisLijst.Core.Shared.Enums;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DikkeTennisLijst.Tests.UnitTests.CalculationsTests
{
    [TestFixture]
    public class ScoreCalculationsTests
    {
        private Match Match { get; set; }
        private double Epsilon { get; } = 0.001;

        [SetUp]
        public void SetUp()
        {
            var playerOne = new Player("Player", "One", "one@email") { Id = "player1" };
            var playerTwo = new Player("Player", "Two", "two@email") { Id = "player2" };
            var surface = new Surface("Clay");
            var sets = new List<Set>();
            var duration = new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(60));

            Match = new Match(playerOne.Id, playerTwo.Id, surface, null, sets, duration, MatchWinner.Tie, MatchFormat.BestOf1, ranked: true);
        }

        /// <summary>
        /// Expected score should be the same as follows from the shadowmodel in Scoring_System.xlsx
        /// </summary>
        public static List<TestCaseData> MatchResultsWithExpectedScores =
            new()
            {
                new TestCaseData(new List<Set>() { new Set(6, 0), new Set(6, 0) }, 1.000),
                new TestCaseData(new List<Set>() { new Set(6, 1), new Set(6, 0) }, 0.983),
                new TestCaseData(new List<Set>() { new Set(6, 2), new Set(6, 0) }, 0.967),
                new TestCaseData(new List<Set>() { new Set(6, 3), new Set(6, 0) }, 0.950),
                new TestCaseData(new List<Set>() { new Set(6, 4), new Set(6, 0) }, 0.933),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(6, 0) }, 0.919),
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(6, 0) }, 0.900),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(6, 1) }, 0.903),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(6, 2) }, 0.887),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(6, 3) }, 0.871),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(6, 4) }, 0.855),
                new TestCaseData(new List<Set>() { new Set(7, 5), new Set(7, 5) }, 0.844),
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 5) }, 0.823),
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) }, 0.800),
                new TestCaseData(new List<Set>() { new Set(6, 0), new Set(0, 6), new Set(6, 0) }, 0.783),
                new TestCaseData(new List<Set>() { new Set(6, 3), new Set(4, 6), new Set(6, 3) }, 0.721),
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(6, 7, 5, 7), new Set(7, 6, 7, 5) }, 0.653)
            };

        [Test]
        [TestCaseSource("MatchResultsWithExpectedScores")]
        public void CalculateActualScore_WhenPlayer1WinsAndSets_ThenResult(List<Set> sets, double result)
        {
            Match.Sets = sets;
            Match.Winner = MatchWinner.PlayerOne;
            var (PlayerOne, _) = ScoreCalculations.CalculateActualScore(Match);

            Assert.That(PlayerOne, Is.EqualTo(result).Within(Epsilon));
        }

        [Test]
        public void CalculateActualScore_WhenPlayer2WinsAndSetsAreInFavorOfPlayerOne_ThenMethodWorks()
        {
            Match.Sets = new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) };
            Match.Winner = MatchWinner.PlayerTwo;
            var (PlayerOne, _) = ScoreCalculations.CalculateActualScore(Match);

            Assert.That(PlayerOne, Is.EqualTo(0.692).Within(Epsilon));
        }

        [Test]
        public void CalculateActualScore_WhenNoWinnerAndSets_ThenMethodWorks()
        {
            Match.Sets = new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) };
            Match.Winner = MatchWinner.Tie;
            var (PlayerOne, _) = ScoreCalculations.CalculateActualScore(Match);

            Assert.That(PlayerOne, Is.EqualTo(0.769).Within(Epsilon));
        }

        [Test]
        public void CalculateExpectedScore_BestOf1_WhenPlayersHaveSameElo_ThenResultIs50Percent()
        {
            const double expected = 0.5;

            var score = ScoreCalculations.CalculateExpectedScore(1500, 1500, MatchFormat.BestOf1);

            Assert.That(score, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateExpectedScore_BestOf1_WhenPlayersEloDifferDIFF_ThenResultIs91PerMille()
        {
            const double expected = 0.091;
            const int elo1 = 1500 - EloConstants.Diff;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf1);

            Assert.That(score, Is.EqualTo(expected).Within(Epsilon));
        }

        [Test]
        public void CalculateExpectedScore_BestOf1_WhenPlayersEloDifferHalfDiff_ThenResultIs24Percent()
        {
            const double expected = 0.240;
            const int elo1 = 1500 - (EloConstants.Diff / 2);
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf1);

            Assert.That(score, Is.EqualTo(expected).Within(Epsilon));
        }

        [Test]
        public void CalculateExpectedScore_BestOf1_WhenPlayersEloDifferStartK_ThenResultIs24Percent()
        {
            const double expected = 0.429;
            const int elo1 = 1500 - EloConstants.StartK;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf1);

            Assert.That(score, Is.EqualTo(expected).Within(Epsilon));
        }

        [Test]
        public void CalculateExpectedScore_BestOf1_WhenPlayersEloDifferHalfStartK_ThenResultIs24Percent()
        {
            const double expected = 0.464;
            const int elo1 = 1500 - (EloConstants.StartK / 2);
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf1);

            Assert.That(score, Is.EqualTo(expected).Within(Epsilon));
        }

        [Test]
        public void CalculateExpectedScore_BestOf3_WhenPlayersHaveSameElo_ThenResultIs50Percent()
        {
            const double expected = 0.5;
            const int elo1 = 1500;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf3);

            Assert.That(score, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateExpectedScore_BestOf3_WhenPlayersEloDifferDIFF_ThenResultIs()
        {
            const double expected = 0.0233;
            const int elo1 = 1500 - EloConstants.Diff;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf3);

            Assert.That(score, Is.EqualTo(expected).Within(0.0001));
        }

        [Test]
        public void CalculateExpectedScore_BestOf5_WhenPlayersHaveSameElo_ThenResultIs50Percent()
        {
            const double expected = 0.5;
            const int elo1 = 1500;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf5);

            Assert.That(score, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateExpectedScore_BestOf5_WhenPlayersEloDifferDIFF_ThenResultIs()
        {
            const double expected = 0.0065;
            const int elo1 = 1500 - EloConstants.Diff;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf5);

            Assert.That(score, Is.EqualTo(expected).Within(0.0001));
        }

        [Test]
        public void CalculateExpectedScore_BestOf7_WhenPlayersHaveSameElo_ThenResultIs50Percent()
        {
            const double expected = 0.5;
            const int elo1 = 1500;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf7);

            Assert.That(score, Is.EqualTo(expected));
        }

        [Test]
        public void CalculateExpectedScore_BestOf7_WhenPlayersEloDifferDIFF_ThenResultIs()
        {
            const double expected = 0.0019;
            const int elo1 = 1500 - EloConstants.Diff;
            const int elo2 = 1500;

            var score = ScoreCalculations.CalculateExpectedScore(elo1, elo2, MatchFormat.BestOf7);

            Assert.That(score, Is.EqualTo(expected).Within(0.0001));
        }
    }
}