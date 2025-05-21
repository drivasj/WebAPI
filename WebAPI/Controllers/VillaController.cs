using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dates;
using WebAPI.Models;
using WebAPI.Models.Dto;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _context;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");

            var listVillas = await _context.Villas.ToListAsync();

            return Ok(listVillas);
        }

        /// <summary>
        /// GET(id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa con el id " + id);
                return BadRequest();
            }

            //  var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                _logger.LogError("La villa " + id + " no existe.");
                return NotFound();
            }

            return Ok(villa);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="villaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto villaDto)
        {
            if (await _context.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            Villa model = new()
            {
                Name = villaDto.Name,
                Detail = villaDto.Detail,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Price = villaDto.Price,
                SquareMeters = villaDto.SquareMeters,
                RegisterUser = "drivasj",
                RegisterDate = DateTime.Now
            };

            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = model.Id }, villaDto);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest(ModelState);
            }

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Update all
        /// </summary>
        /// <param name="id"></param>
        /// <param name="villaDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {
            if (villaDto is null || id != villaDto.Id)
            {
                return BadRequest(ModelState);
            }

            Villa model = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Detail = villaDto.Detail,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Price = villaDto.Price,
                SquareMeters = villaDto.SquareMeters,
                LastUpdateUser = "drivasj",
                LastUpdate = DateTime.Now
            };

           await _context.Villas.AddAsync(model);
           await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        ///  Update partial
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDto"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto is null || id == 0)
            {
                return BadRequest(ModelState);
            }

            var villa =  await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Detail = villa.Detail,
                ImageUrl = villa.ImageUrl,
                Occupants = villa.Occupants,
                Price = villa.Price,
                SquareMeters = villa.SquareMeters,
                LastUpdateUser = "drivasj",
                LastUpdate = DateTime.Now
            };

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa model = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Detail = villaDto.Detail,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Price = villaDto.Price,
                SquareMeters = villaDto.SquareMeters,
                LastUpdateUser = "drivasj",
                LastUpdate = DateTime.Now
            };

                 _context.Villas.Update(model);
           await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
