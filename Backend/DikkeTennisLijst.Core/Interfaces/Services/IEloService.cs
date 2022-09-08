using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IEloService<TElo> : IEntityService<TElo> where TElo : Elo
    {
    }
}