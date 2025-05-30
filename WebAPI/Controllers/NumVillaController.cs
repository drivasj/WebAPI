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
    public class NumVillaController : ControllerBase
    {
        private readonly ILogger<NumVillaController> _logger;
        private readonly IVillaRepository _villaRepository;
        private readonly INumVillaRepository _numVillaRepository;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public NumVillaController(ILogger<NumVillaController> logger, IVillaRepository villaRepository, INumVillaRepository numVillaRepository, IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaRepository;
            _numVillaRepository = numVillaRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumVillas()
        {
            try
            {
                _logger.LogInformation("Obtener numeros villas");

                IEnumerable<NumVilla> villaList = await _numVillaRepository.GetAll();

                _apiResponse.Result = _mapper.Map<IEnumerable<NumVillaDto>>(villaList);
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

        [HttpGet("id:int", Name = "GetNumVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetNumVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer el numero villa con el id " + id);
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_apiResponse);
                }

                var numVilla = await _numVillaRepository.Get(x => x.VillaNo == id);

                if (numVilla == null)
                {
                    _logger.LogError("El numero villa " + id + " no existe.");
                    _apiResponse.statusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSucces = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<NumVillaDto>(numVilla);
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
        public async Task<ActionResult<APIResponse>> CrearNumVilla([FromBody] NumVillaCreateDto createDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                if (await _numVillaRepository.Get(x => x.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El numero de villa con ese numero ya existe");
                    return BadRequest(ModelState);
                }

                if(await _villaRepository.Get(x=>x.Id==createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de la villa no existe!");
                    return BadRequest(ModelState);
                }


                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                NumVilla model = _mapper.Map<NumVilla>(createDto);

                model.RegisterDate = DateTime.Now;
                model.LastUpdate = DateTime.Now;

                await _numVillaRepository.Create(model);

                _apiResponse.Result = model;
                _apiResponse.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumVilla", new { id = model.VillaNo }, _apiResponse);
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
        public async Task<IActionResult> DeleteNumVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_apiResponse);
                }
                var numVilla = await _numVillaRepository.Get(x => x.VillaNo == id);

                if (numVilla == null)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _numVillaRepository.Remove(numVilla);

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

        public async Task<IActionResult> UpdateNumVilla(int id, [FromBody] NumVillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto is null || id != updateDto.VillaNo)
                {
                    _apiResponse.IsSucces = false;
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;

                    return BadRequest(ModelState);
                }

                if(await _villaRepository.Get(x=>x.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe!");
                    return BadRequest(ModelState);
                }

                NumVilla model = _mapper.Map<NumVilla>(updateDto);

                await _numVillaRepository.Update(model);

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
        //[HttpPatch("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]

        //public async Task<IActionResult> UpdatePartialNumVilla(int id, JsonPatchDocument<NumVillaUpdateDto> patchDto)
        //{
        //    if (patchDto is null || id == 0)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var numVilla = await _numVillaRepository.Get(v => v.VillaNo == id, tracked: false);

        //    NumVillaUpdateDto villaDto = _mapper.Map<NumVillaUpdateDto>(numVilla);

        //    if (numVilla == null) return BadRequest();

        //    patchDto.ApplyTo(villaDto, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    NumVilla model = _mapper.Map<NumVilla>(villaDto);

        //    await _numVillaRepository.Update(model);
        //    _apiResponse.statusCode = HttpStatusCode.NoContent;

        //    return Ok(_apiResponse);
        //}
    }
}
