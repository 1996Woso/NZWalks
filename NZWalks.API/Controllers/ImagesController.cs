using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IImageRepository imageService;

        public ImagesController(IMapper mapper, IImageRepository imageService)
        {
            this.mapper = mapper;
            this.imageService = imageService;
        }
        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
       
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO imageUploadRequestDTO)
        {
            ValidateFileUpload(imageUploadRequestDTO);
            if (ModelState.IsValid)
            {
                var imageDM = new Image
                {
                    File = imageUploadRequestDTO.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDTO.File.FileName),
                    FileSizeInBytes = imageUploadRequestDTO.File.Length,
                    FileName = imageUploadRequestDTO.FileName,
                    FileDescription = imageUploadRequestDTO.FileDescription,
                };

                await imageService.Upload(imageDM);
                return Ok(imageDM);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO imageUploadRequestDTO)
        {
            var allowdExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            
            if (!allowdExtensions.Contains(Path.GetExtension(imageUploadRequestDTO.File.FileName).ToLower())) 
            {
                ModelState.AddModelError("file","Unsupported file extension, please upload (jpg, jpeg, and png) files.");
            }

            if(imageUploadRequestDTO.File.Length > 10485760)//1024 * 1024 * 10 = 10MB 
            {
                ModelState.AddModelError("file", "File size is more tha 10MB, please upload a small size file.");
            }
        }
    }
}
