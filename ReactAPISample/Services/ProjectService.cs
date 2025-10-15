using ReactAPISample.Dtos;

namespace ReactAPISample.Services
{
  public class ProjectService
  {

    public ProjectParameterRequestDto getParametersById(int id)
    {
      return new ProjectParameterRequestDto
      {
        ProjectId = id,
        ProjectNumber = $"PRJ-{id:000}",
        TransformerPower = id * 1.5,
        HighVoltage = 110 + id * 10,
        LowVoltage = 10 + id * 2,
        StockCode = $"STK-{id:000}"
      };

    }

    // göndeirlen parameterelere göre bir reponse döner
    public ProjectParameterRequestDto calculate(ProjectParameterRequestDto dto)
    {
      return dto;
    }


    public List<ProjectParameterRequestDto> getAllProjectParameters()
    {
      var result = new List<ProjectParameterRequestDto>();

      for (int i = 1; i <= 50; i++)
      {
        result.Add(new ProjectParameterRequestDto
        {
          ProjectId = i,
          ProjectNumber = $"PRJ-{i:000}",
          TransformerPower = i * 1.5,
          HighVoltage = 110 + i * 10,
          LowVoltage = 10 + i * 2,
          StockCode = $"STK-{i:000}"
        });
      }

      return result;
    }
  }
}
