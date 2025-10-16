using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactAPISample.Dtos;
using ReactAPISample.Services;

namespace ReactAPISample.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  // projects controllera jwt göndermeden giremeyiz
  [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
  public class ProjectsController : ControllerBase
  {
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
      _projectService = projectService;
    }

    // api/projects -> GET

    [HttpGet]
    [Authorize(Roles ="Manager")] // jwt içinde böyle bir role sahip değilizi 403 almamız lazım.
    public IActionResult getAllProjectParameters()
    {
      var data  = _projectService.getAllProjectParameters();
      // servise gidip dönden değeri json formatında apiden dışırı sunduğumuz endpoint
      return Ok(data);
    }

    // api/projects/1
    [HttpGet("{id}")]
    public IActionResult getParametersById(int id)
    {
      var data = _projectService.getParametersById(id);

      return Ok(data);
    }

    [HttpPost]
    public IActionResult calculate([FromBody] ProjectParameterRequestDto requestDto)
    {
      var response = _projectService.calculate(requestDto);


      return Ok(response);      
    }

  }
}
