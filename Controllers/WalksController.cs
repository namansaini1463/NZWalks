using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebApiNZwalks.CustomActionFilters;
using WebApiNZwalks.Models.Domain;
using WebApiNZwalks.Models.DTO;
using WebApiNZwalks.Repositories;

namespace WebApiNZwalks.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // Create Walk
        // POST : /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO) {
            

            // Map the DTO to the Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);

            await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain Model to DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);

            return Ok();
        }

        // GET Walks
        // GET : /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
                                                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var walksDomainModels = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Creating an exception
            throw new Exception("This is my exception in the walks controller");

            // Map Domain Model to DTO
            var walksDTO = mapper.Map<List<WalkDTO>>(walksDomainModels);

            return Ok(walksDTO);

        }

        // GET Walk by ID
        // GET : /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id);

            if(walksDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walksDomainModel));

        }

        // UPDATE Walk by ID
        // PUT : /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            

            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDTO);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);

            return Ok(walkDTO);
        }

        // DELETE WALK by ID
        // DELETE: /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id) { 
            var deletedWalkDomain = await walkRepository.DeleteAsync(id);

            if(deletedWalkDomain == null){
                return NotFound();
            }

            // Map the domain model to the DTO
            return Ok(mapper.Map<WalkDTO>(deletedWalkDomain));
        }
    }
}
