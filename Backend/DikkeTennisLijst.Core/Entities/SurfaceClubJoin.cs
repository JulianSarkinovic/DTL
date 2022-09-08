using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class SurfaceClubJoin : Entity
    {
        private SurfaceClubJoin()
        {
        }

        public SurfaceClubJoin(int clubId, int surfaceId)
        {
            SurfaceId = surfaceId;
            ClubId = clubId;
        }

        public int SurfaceId { get; set; }
        public Surface Surface { get; set; } = null!;
        public int ClubId { get; set; }
        public Club Club { get; set; } = null!;
    }
}