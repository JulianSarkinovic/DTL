using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Surface : Entity
    {
#pragma warning disable CS8618

        private Surface()
        {
        }

#pragma warning restore CS8618

        public Surface(string name)
        {
            Name = name;
            SurfaceClubJoins = new List<SurfaceClubJoin>();
        }

        public string Name { get; set; }
        public ICollection<SurfaceClubJoin>? SurfaceClubJoins { get; set; }
    }
}