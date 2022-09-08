using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.Core.Shared.Extensions
{
    public static class SetExtensions
    {
        public static string ToReadableString(this ICollection<Set> sets, MatchWinner winner)
        {
            string message = "";

            foreach (var set in sets)
            {
                if (set.IsSuperTieBreaker)
                {
                    message += winner == MatchWinner.PlayerOne
                        ? $"({set.PointsP1}-{set.PointsP2})"
                        : $"({set.PointsP2}-{set.PointsP1})";
                }
                else
                {
                    message += winner == MatchWinner.PlayerOne
                        ? $"{set.GamesP1}-{set.GamesP2}"
                        : $"{set.GamesP2}-{set.GamesP1}";

                    if (set.IsTieBreaker)
                    {
                        message += winner == MatchWinner.PlayerOne
                            ? $" ({set.PointsP1}-{set.PointsP2})"
                            : $" ({set.PointsP2}-{set.PointsP1})";
                    }
                }

                message += ", ";
            }

            return message.Remove(message.Length - 2);
        }
    }
}