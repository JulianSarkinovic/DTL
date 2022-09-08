using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    public class SurfaceController : ControllerBase<Surface>
    {
        private readonly ISurfaceService _surfaceService;

        public SurfaceController(
            IMapper mapper,
            ISurfaceService service) : base(mapper)
        {
            _surfaceService = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<SurfaceResponseModel>>>> GetAsync()
        {
            var result = await _surfaceService.GetRangeAsync();
            return Result<List<Surface>, List<SurfaceResponseModel>>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<SurfaceResponseModel>>> GetAsync(int id)
        {
            var result = await _surfaceService.GetAsync(id);
            return Result<Surface, SurfaceResponseModel>(result);
        }
    }
}