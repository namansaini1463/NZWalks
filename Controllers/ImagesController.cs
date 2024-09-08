using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNZwalks.Models.Domain;
using WebApiNZwalks.Models.DTO;
using WebApiNZwalks.Repositories;

namespace WebApiNZwalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }


        //POST : /api/images/upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO imageUploadRequestDTO)
        {
            ValidateFileUpload(imageUploadRequestDTO);

            if (ModelState.IsValid)
            {
                // Converting the DTO to the Domain model
                var imageDomainModel = new Image
                {
                    File = imageUploadRequestDTO.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDTO.File.FileName),
                    FileSizeInBytes = imageUploadRequestDTO.File.Length,
                    FileName = imageUploadRequestDTO.FileName,
                    FileDescription = imageUploadRequestDTO.FileDescription,
                };

                // User repository to upload image
                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);


            }

            return BadRequest(ModelState);  
        }

        private void ValidateFileUpload(ImageUploadRequestDTO imageUploadRequestDTO)
        {
            var allowedExtensions = new String[] { ".jpg", "jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(imageUploadRequestDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file type"); 
            }

            if(imageUploadRequestDTO.File.Length > 10_485_760) 
            {
                ModelState.AddModelError("file", "File size more than 10Mb! Please upload a file with smaller size");
            }
        }
    }
}
