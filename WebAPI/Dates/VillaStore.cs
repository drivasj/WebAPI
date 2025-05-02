using WebAPI.Models.Dto;

namespace WebAPI.Dates
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1, Name="Vista a la Piscina" },
            new VillaDto{Id=2, Name="Vista a la playa"}
        };
    }
}
