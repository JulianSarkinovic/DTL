using DikkeTennisLijst.Core.Entities.Owned;

namespace DikkeTennisLijst.Core.Interfaces.Entities
{
    public interface IElo : IEntity<int>, ITimestamps
    {
        int Rating { get; set; }
        ICollection<EloHistory> History { get; set; }
        bool IsProvisional { get; set; }
        int K { get; set; }

        //Player Player { get; set; }
        string PlayerId { get; set; }
    }
}