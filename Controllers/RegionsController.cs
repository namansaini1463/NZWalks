using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using WebApiNZwalks.CustomActionFilters;
using WebApiNZwalks.Data;
using WebApiNZwalks.Models.Domain;
using WebApiNZwalks.Models.DTO;
using WebApiNZwalks.Repositories;

namespace WebApiNZwalks.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS
        // GET : https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            // Get data from the database in the form of DOMAIN MODELS
            var regionsDomains = await regionRepository.GetAllAsync();

            //// Map Domain models to DTOs
            //var regionsDTO = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomains) {
            //    regionsDTO.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            // Mapping the Domain models to DTOs
            var regionsDTO = mapper.Map<List<RegionDTO>>(regionsDomains);

            // Return the DTOs

            return Ok(regionsDTO);
        }

        // GET SINGLE REGION (GET REGION BY ID)
        // GET : https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Find method only takes the primary key as a param
            //var region = dbContext.Regions.Find(id);

            // Get Region Domain Model from Database
            // Using a method in which we can use any field to find a particular record
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null) {
                return NotFound();
            }

            //// Map the region domain model to Region DTO
            //var regionsDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            var regionDTO = mapper.Map<RegionDTO>(regionDomain);

            // Return the DTO back to the client
            return Ok(regionDTO);
        }

        // POST To Create a New Region
        // POST : https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //// Map or Convert the DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

            // Use the Domain Model to create the Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //// Map the domain model back to DTO
            //var regionDTO = new RegionDTO {
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        // For UPDATEing a Region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            // Check if the region exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

            //if (regionDomainModel == null)
            //{
            //    return NotFound();
            //}

            //// Map the DTO to the Domain Model
            //regionDomainModel.Code = updateRegionRequestDTO.Code;
            //regionDomainModel.Name = updateRegionRequestDTO.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            //await dbContext.SaveChangesAsync();

            //// Map the DTO to the Domain Model
            //var regionDomainModel = new Region { 
            //    Code = updateRegionRequestDTO.Code,
            //    Name = updateRegionRequestDTO.Name,
            //    RegionImageUrl= updateRegionRequestDTO.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);


            // Check if the repository exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //// Convert Domain model back to DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);
        }


        // Delete a Region
        // DELETE : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //// Delete the region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            //// Mapping the region domain model to the DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            //Return the deleted region back to the user
            return Ok(regionDTO);


        }
    }
}
