using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.WebAPI.Data;
using ProAgil.WebAPI.Model;

namespace ProAgil.WebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
   
        public ValuesController(DataContext context)
        {
            this._context =  context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await  _context.Eventos.ToListAsync();
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                "Banco de dados falhou");
            }

        }
        // public ActionResult<IEnumerable<Evento>> Get()
        // {
        //     return  _context.Eventos.ToList();
          
        //     // return new Evento[] {
        //     //      new Evento(){
        //     //          EventoId = 1,
        //     //          Tema = "Angular e .NET Core",
        //     //          Local = "Belo Horizonte",
        //     //          Lote = "1ª Lote",
        //     //          QtdPessoas = 250,
        //     //          DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy")
        //     //      },
        //     //       new Evento(){
        //     //          EventoId = 2,
        //     //          Tema = "Angular e Suas Novidades",
        //     //          Local = "São Paulo",
        //     //          Lote = "2ª Lote",
        //     //          QtdPessoas = 350,
        //     //          DataEvento = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy")
        //     //      }
        //     //  };
           
        //  }
        [HttpGet("{id}")]
         public  async Task<IActionResult> Get(int id)
          {

            //   return new Evento[] {
            //      new Evento(){
            //          EventoId = 1,
            //          Tema = "Angular e .NET Core",
            //          Local = "Belo Horizonte",
            //          Lote = "1ª Lote",
            //          QtdPessoas = 250,
            //          DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy")
            //      },
            //       new Evento(){
            //          EventoId = 2,
            //          Tema = "Angular e Suas Novidades",
            //          Local = "São Paulo",
            //          Lote = "2ª Lote",
            //          QtdPessoas = 350,
            //          DataEvento = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy")
            //      }
            //  }
          
            try
            {
                var results = await _context.Eventos.FirstOrDefaultAsync( x => x.EventoId == id);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                "Banco de dados falhou");
            } 
         }
    }
}
