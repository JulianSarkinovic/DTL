using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Shared.Specifications;
using DikkeTennisLijst.WebAPI.Models;
using DikkeTennisLijst.WebAPI.Models.Request;
using DikkeTennisLijst.WebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DikkeTennisLijst.WebAPI.Controllers
{
    public class ClubController : ControllerBase<Club>
    {
        private readonly IClubService _service;

        public ClubController(
            IMapper mapper,
            IClubService clubService) : base(mapper)
        {
            _service = clubService ?? throw new ArgumentNullException(nameof(clubService));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ClubResponseModel>>>> GetAsync()
        {
            var spec = new SpecificationClubsWithSurfaces();
            var result = await _service.GetRangeAsync(spec);

            return Result<List<Club>, List<ClubResponseModel>>(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ClubResponseModel>>> GetAsync(int id)
        {
            var spec = new SpecificationClubsWithSurfaces();
            var result = await _service.GetAsync(id, spec);

            return Result<Club, ClubResponseModel>(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ClubResponseModel>>>> Search(string term)
        {
            var spec = new SpecificationClubsWithSurfaces(x => x.Name.Contains(term));
            var result = await _service.GetRangeAsync(spec);

            return Result<List<Club>, List<ClubResponseModel>>(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClubResponseModel>>> PostAsync(ClubRequestModel clubRequestModel)
        {
            var club = _mapper.Map<Club>(clubRequestModel);
            var result = await _service.AddAsync(club);

            return Result<Club, ClubResponseModel>(result);
        }
    }
}