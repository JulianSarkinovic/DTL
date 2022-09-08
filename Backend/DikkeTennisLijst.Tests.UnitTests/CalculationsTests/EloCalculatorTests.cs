using DikkeTennisLijst.Core.Calculations;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Constants;
using DikkeTennisLijst.Core.Shared.Enums;
using NUnit.Framework;

namespace DikkeTennisLijst.Tests.UnitTests.CalculationsTests
{
    [TestFixture]
    public class EloCalculatorTests
    {
        private Match Match { get; set; }
        private EloRanked EloP1 { get; set; }
        private EloRanked EloP2 { get; set; }

        [SetUp]
        public void SetUp()
        {
            var playerOne = new Player("Player", "One", "one@email") { Id = "player1" };
            var playerTwo = new Player("Player", "Two", "two@email") { Id = "player2" };
            var surface = new Surface("Clay");
            var sets = new List<Set>();
            var duration = new Duration(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(60));

            Match = new Match(playerOne.Id, playerTwo.Id, surface, null, sets, duration, MatchWinner.Tie, MatchFormat.BestOf3, ranked: true);
            EloP1 = new EloRanked(playerOne.Id) { IsProvisional = false };
            EloP2 = new EloRanked(playerTwo.Id) { IsProvisional = false };
        }

        private static readonly List<TestCaseData> MatchResultsWithExpectedScores_WhenElosAre1500 =
            new()
            {
                // even strength player 1 wins in  2 straight sets and gains 25 elo:
                new TestCaseData(new List<Set>() { new Set(6, 0), new Set(6, 0) }, MatchWinner.PlayerOne, 1525, 1475),
                // even strength player 1 loses in 2 straight sets and loses 25 elo:
                new TestCaseData(new List<Set>() { new Set(0, 6), new Set(0, 6) }, MatchWinner.PlayerTwo, 1475, 1525),
                // even strength player 1 wins in  2 close    sets and gains  9 elo
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) }, MatchWinner.PlayerOne, 1515, 1485),
                // even strength player 1 loses in 2 straight sets and loses  9 elo:
                new TestCaseData(new List<Set>() { new Set(6, 7, 5, 7), new Set(6, 7, 5, 7) }, MatchWinner.PlayerTwo, 1485, 1515)
            };

        [Test]
        [TestCaseSource(nameof(MatchResultsWithExpectedScores_WhenElosAre1500))]
        public void UpdateForMatch_WhenElosAre1500_ThenResult(List<Set> sets, MatchWinner winnerId, int expectedEloP1, int expectedEloP2)
        {
            Match.Sets = sets;
            Match.Winner = winnerId;

            EloCalculator.UpdateElosForMatch(EloP1, EloP2, Match);

            Assert.Multiple(() =>
            {
                Assert.That(EloP2.Rating, Is.EqualTo(expectedEloP2));
                Assert.That(EloP1.Rating, Is.EqualTo(expectedEloP1));
            });
        }

        private static readonly List<TestCaseData> MatchResultsWithExpectedScores_WhenP1Elo1500MinusDiffAndP2EloIs1500 =
            new()
            {
                // full diff underdog player 1 wins in  2 straight sets and gains 49 elo:
                new TestCaseData(new List<Set>() { new Set(6, 0), new Set(6, 0) }, MatchWinner.PlayerOne, 1149, 1451),
                // full diff underdog player 1 loses in 2 straight sets and loses  1 elo:
                new TestCaseData(new List<Set>() { new Set(0, 6), new Set(0, 6) }, MatchWinner.PlayerTwo, 1099, 1501),
                // full diff underdog player 1 wins in  2 close    sets and gains 33 elo:
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) }, MatchWinner.PlayerOne, 1139, 1461),
                // full diff underdog player 1 loses in 2 close    sets and gains  15 elo:
                new TestCaseData(new List<Set>() { new Set(6, 7, 5, 7), new Set(6, 7, 5, 7) }, MatchWinner.PlayerTwo, 1109, 1491)
            };

        [Test]
        [TestCaseSource(nameof(MatchResultsWithExpectedScores_WhenP1Elo1500MinusDiffAndP2EloIs1500))]
        public void UpdateForMatch_WhenP1Elo1500MinusDiffAndP2EloIs1500_ThenResult(List<Set> sets, MatchWinner winnerId, int expectedEloP1, int expectedEloP2)
        {
            Match.Sets = sets;
            Match.Winner = winnerId;
            EloP1.Rating = 1500 - EloConstants.Diff;

            EloCalculator.UpdateElosForMatch(EloP1, EloP2, Match);

            Assert.Multiple(() =>
            {
                Assert.That(EloP1.Rating, Is.EqualTo(expectedEloP1));
                Assert.That(EloP2.Rating, Is.EqualTo(expectedEloP2));
            });
        }

        private static readonly List<TestCaseData> MatchResultsWithExpectedScores_WhenP1Elo1500MinusHalfDiffAndP2EloIs1500 =
            new()
            {
                // half diff underdog player 1 wins in  2 straight sets and gains 43 elo:
                new TestCaseData(new List<Set>() { new Set(6, 0), new Set(6, 0) }, MatchWinner.PlayerOne, 1343, 1457),
                // half diff underdog player 1 loses in 2 straight sets and loses  7 elo:
                new TestCaseData(new List<Set>() { new Set(0, 6), new Set(0, 6) }, MatchWinner.PlayerTwo, 1293, 1507),
                // half diff underdog player 1 wins in  2 close    sets and gains 27 elo:
                new TestCaseData(new List<Set>() { new Set(7, 6, 7, 5), new Set(7, 6, 7, 5) }, MatchWinner.PlayerOne, 1333, 1467),
                // half diff underdog player 1 loses in 2 close    sets and gains  9 elo:
                new TestCaseData(new List<Set>() { new Set(6, 7, 5, 7), new Set(6, 7, 5, 7) }, MatchWinner.PlayerTwo, 1303, 1497),
            };

        [Test]
        [TestCaseSource(nameof(MatchResultsWithExpectedScores_WhenP1Elo1500MinusHalfDiffAndP2EloIs1500))]
        public void UpdateForMatch_WhenP1Elo1500MinusHalfDiffAndP2EloIs1500_ThenResult(List<Set> sets, MatchWinner winnerId, int expectedEloP1, int expectedEloP2)
        {
            Match.Sets = sets;
            Match.Winner = winnerId;
            EloP1.Rating = 1500 - (EloConstants.Diff / 2);

            EloCalculator.UpdateElosForMatch(EloP1, EloP2, Match);

            Assert.Multiple(() =>
            {
                Assert.That(EloP1.Rating, Is.EqualTo(expectedEloP1));
                Assert.That(EloP2.Rating, Is.EqualTo(expectedEloP2));
            });
        }
    }
}