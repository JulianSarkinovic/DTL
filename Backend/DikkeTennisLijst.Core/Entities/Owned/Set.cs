using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities.Owned
{
    public class Set : Entity
    {
        private Set()
        {
        }

        public Set(int? gamesP1, int? gamesP2, int? pointsP1 = null, int? pointsP2 = null)
        {
            ValidateArguments(gamesP1, gamesP2, pointsP1, pointsP2);

            GamesP1 = gamesP1;
            GamesP2 = gamesP2;
            PointsP1 = pointsP1;
            PointsP2 = pointsP2;
        }

        public int? GamesP1 { get; set; }
        public int? GamesP2 { get; set; }
        public int? PointsP1 { get; set; }
        public int? PointsP2 { get; set; }
        public int MatchId { get; set; }

        public bool IsTieBreaker
        {
            get { return GamesP1 != null && GamesP2 != null && PointsP1 != null && PointsP2 != null; }
        }

        public bool IsSuperTieBreaker
        {
            get { return GamesP1 == null && GamesP2 == null && PointsP1 != null && PointsP2 != null; }
        }

        private static void ValidateArguments(int? gamesP1, int? gamesP2, int? pointsP1, int? pointsP2)
        {
            if ((gamesP1 == null && gamesP2 != null) ||
                (gamesP1 != null && gamesP2 == null))
            {
                throw new ArgumentException("Player cannot play games by himself.");
            }
            if ((pointsP1 == null && pointsP2 != null) ||
                (pointsP1 != null && pointsP2 == null))
            {
                throw new ArgumentException("Player cannot play points by himself");
            }
            if (gamesP1 == null && pointsP1 == null)
            {
                throw new ArgumentException("Set must have games or points");
            }
        }
    }
}