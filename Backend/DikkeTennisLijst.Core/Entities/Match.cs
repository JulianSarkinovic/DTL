using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.Core.Entities
{
    public class Match : Entity
    {
#pragma warning disable CS8618

        private Match()
        {
        }

#pragma warning restore CS8618

        public Match(
            string playerOneId,
            string playerTwoId,
            Surface surface,
            Club? club,
            ICollection<Set> sets,
            Duration duration,
            MatchWinner winner,
            MatchFormat format,
            Status status = Status.Enabled,
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Awaiting,
            bool ranked = true)
        {
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            Surface = surface;
            SurfaceId = surface.Id;
            Club = club;
            ClubId = club?.Id;

            Sets = sets;
            Duration = duration;
            Winner = winner;
            Format = format;
            Status = status;
            ConfirmationStatus = confirmationStatus;
            Type = Shared.Enums.MatchType.Singles;
            Ranked = ranked;
        }

        public Match(
            string playerOneId,
            string playerOnePartnerId,
            string playerTwoId,
            string playerTwoPartnerId,
            Surface surface,
            Club? club,
            ICollection<Set> sets,
            Duration duration,
            MatchWinner winner,
            MatchFormat format,
            Status status = Status.Enabled,
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Awaiting,
            bool ranked = true) : this(playerOneId, playerTwoId, surface, club, sets, duration, winner, format, status, confirmationStatus, ranked)
        {
            Type = Shared.Enums.MatchType.Doubles;
            PlayerOnePartnerId = playerOnePartnerId;
            PlayerTwoPartnerId = playerTwoPartnerId;
        }

        public Player PlayerOne { get; set; } = null!;
        public string PlayerOneId { get; set; }
        public Player PlayerTwo { get; set; } = null!;
        public string PlayerTwoId { get; set; }
        public Player? PlayerOnePartner { get; set; }
        public string? PlayerOnePartnerId { get; set; }
        public Player? PlayerTwoPartner { get; set; }
        public string? PlayerTwoPartnerId { get; set; }

        public Surface Surface { get; set; }
        public int SurfaceId { get; set; }
        public Club? Club { get; set; }
        public int? ClubId { get; set; }

        public ICollection<Set> Sets { get; set; }
        public Duration Duration { get; set; }
        public MatchWinner Winner { get; set; }
        public MatchFormat Format { get; set; }
        public Shared.Enums.MatchType Type { get; set; }
        public Status Status { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public string ConfirmationToken { get; set; } = Guid.NewGuid().ToString().Replace("+", "%2B");
        public bool Ranked { get; set; }

        public static MatchFormat GetFormat(ICollection<Set> sets) => sets.Count switch
        {
            1 => MatchFormat.BestOf1,
            2 => MatchFormat.BestOf3,
            3 => GetSetsDifference(sets) < 2 ? MatchFormat.BestOf3 : MatchFormat.BestOf5,
            4 => GetSetsDifference(sets) < 3 ? MatchFormat.BestOf5 : MatchFormat.BestOf7,
            5 => GetSetsDifference(sets) < 2 ? MatchFormat.BestOf5 : MatchFormat.BestOf7,
            _ => MatchFormat.BestOf7,
        };

        private static int GetSetsDifference(ICollection<Set> sets)
        {
            int setsPlayerOne = 0;
            int setsPlayerTwo = 0;

            foreach (var set in sets)
            {
                setsPlayerOne += set.GamesP1 > set.GamesP2 ? 1 : 0;
                setsPlayerTwo += set.GamesP2 > set.GamesP1 ? 1 : 0;
                setsPlayerOne += set.IsSuperTieBreaker && set.PointsP1 > set.PointsP2 ? 1 : 0;
                setsPlayerTwo += set.IsSuperTieBreaker && set.PointsP2 > set.PointsP1 ? 1 : 0;
            }

            return Math.Abs(setsPlayerOne - setsPlayerTwo);
        }

        public void Update(Match match, bool newConfirmationToken)
        {
            SurfaceId = match.SurfaceId;
            ClubId = match.ClubId;
            Sets = match.Sets;
            Duration = match.Duration;
            Winner = match.Winner;
            Format = match.Format;
            Type = match.Type;
            Status = match.Status;
            ConfirmationStatus = match.ConfirmationStatus;
            Ranked = match.Ranked;

            if (newConfirmationToken)
            {
                ConfirmationToken = Guid.NewGuid().ToString();
            }
        }
    }
}