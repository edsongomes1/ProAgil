using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using ProAgil.Domain;
namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController: ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllEventoAsync(true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }
        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncById(eventoId, true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }
       
        [HttpGet ("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncByTema(tema, true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }
         [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
               _repo.Add(model);
               if(await _repo.SaveChangesAsync()) {
                   return Created($"/api/evento/{model.Id}", model);
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
            return BadRequest();

        }
         [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);
                if(evento == null) {
                    return NotFound();
                }
                else
                {
                     _repo.Update(model);
                }
              
               if(await _repo.SaveChangesAsync()) {
                   return Created($"/api/evento/{model.Id}", model);
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
            return BadRequest();

        }
        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);
                if(evento == null) {
                    return NotFound();
                }
                else
                {
                     _repo.Delete(evento);
                }
              
               if(await _repo.SaveChangesAsync()) {
                   return Ok();
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
            return BadRequest();

        }
    }
}