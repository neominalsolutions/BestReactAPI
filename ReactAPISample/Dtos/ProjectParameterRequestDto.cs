using System.ComponentModel.DataAnnotations;

namespace ReactAPISample.Dtos
{
  // Hesaplanmış projelerin çekildiği liste
  public class ProjectParameterRequestDto
  {
        public int ProjectId { get; set; }
        public string? ProjectNumber { get; set; }
        public string? StockCode { get; set; }

        public double TransformerPower { get; set; }
      
        public double HighVoltage { get; set; }

        public double LowVoltage { get; set; }
  }
}
