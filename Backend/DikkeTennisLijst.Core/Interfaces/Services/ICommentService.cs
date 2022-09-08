using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    // Todo, consider not using the IService this way, but just exposing what actually should/can be used;
    public interface ICommentService : IEntityService<Comment>
    {
        OperationResult<Comment> Update(Comment comment, int id);
    }
}