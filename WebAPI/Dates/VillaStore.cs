using WebAPI.Models.Dto;

namespace WebAPI.Dates
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1, Name="Vista a la Piscina", Ocupantes =3, MetrosCuadrados=50 },
            new VillaDto{Id=2, Name="Vista a la playa", Ocupantes = 4, MetrosCuadrados=80}
        };
    }
}
