using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPI.Dates;
using WebAPI.Models;
using WebAPI.Models.Dto;
using WebAPI.Repository.IRepository;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepository, IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las villas");

                IEnumerable<Villa> villaList = await _villaRepository.GetAll();

                _apiResponse.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _apiResponse.statusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSucces = false;
                _apiResponse.ErrorsMessages = new List<string> { ex.Message };
            }

            return _apiResponse;
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

        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer la villa con el id " + id);
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_apiResponse);
                }

                var villa = await _villaRepository.Get(x => x.Id == id);

                if (villa == null)
                {
                    _logger.LogError("La villa " + id + " no existe.");
                    _apiResponse.statusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSucces = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<VillaDto>(villa);
                _apiResponse.statusCode = HttpStatusCode.OK;
        
                return Ok(_apiResponse);
            }
            catch (Exception ex) 
            {
                _apiResponse.IsSucces = false;
                _apiResponse.ErrorsMessages = new List<string> { ex.Message };
            }   
            return Ok(_apiResponse);
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
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (await _villaRepository.Get(x => x.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Villa model = _mapper.Map<Villa>(createDto);

                model.RegisterDate = DateTime.Now;
                model.LastUpdate = DateTime.Now;

                await _villaRepository.Create(model);

                _apiResponse.Result = model;
                _apiResponse.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = model.Id }, _apiResponse);
            }
            catch(Exception ex) 
            {
                _apiResponse.IsSucces = false;
                _apiResponse.ErrorsMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
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
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_apiResponse);
                }
                var villa = await _villaRepository.Get(x => x.Id == id);

                if (villa == null)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _villaRepository.Remove(villa);

                _apiResponse.statusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSucces = false;
                _apiResponse.ErrorsMessages = new List<string> { ex.Message };
             
            }
            return BadRequest(_apiResponse);
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

        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto is null || id != updateDto.Id)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(ModelState);
                }

                Villa model = _mapper.Map<Villa>(updateDto);

                await _villaRepository.Update(model);

                _apiResponse.statusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSucces = false;
                _apiResponse.ErrorsMessages = new List<string> { ex.Message };
            }
            return Ok(_apiResponse);
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

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto is null || id == 0)
            {
                return BadRequest(ModelState);
            }

            var villa = await _villaRepository.Get(v => v.Id == id, tracked: false);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(villaDto);

            await _villaRepository.Update(model);
            _apiResponse.statusCode = HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }
    }
}
