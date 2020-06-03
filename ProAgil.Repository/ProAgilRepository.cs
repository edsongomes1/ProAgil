using System.Threading.Tasks;
using ProAgil.Domain;
using Microsoft.EntityFrameworkCore;

using System.Linq;
namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
      private readonly ProAgilContext _context;
      public ProAgilRepository(ProAgilContext context) 
      {
        _context = context;

        //de forma geral
        //_context.ChangeTracker.QueryTrackingBehavior  = QueryTrackingBehavior.NoTracking;
      }
        //GERAIS
      public void Add<T> (T entity) where T : class
      {
          _context.Add(entity);
      }

      public void Update<T>(T entity) where T : class
      {
        _context.Update(entity);
      }
       public void Delete<T> (T entity) where T : class
      {
         _context.Remove(entity);
      }
      public async Task<bool> SaveChangesAsync()
     {
        
           return(await  _context.SaveChangesAsync()) > 0;
      }

      public async Task<Evento[]> GetAllEventoAsync (bool includePalestrantes = false) {
           
           IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include( p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            //quando especificando o que estamos retornando. podemos fazer da seguinte forma
            // FORMA ANTIGA -> query = query.OrderByDescending(c => c.DataEvento);
            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();

      }

      public async  Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes) {
          IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include( p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento)
                         .Where( c => c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

       
        public async Task<Evento> GetAllEventoAsyncById(int eventoId, bool includePalestrantes = false) {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include( p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento)
                         .Where( c => c.Id == eventoId );

            return await query.FirstOrDefaultAsync();
        } 
        //PALESTRANTES
        public async Task<Palestrante> GetPalestanteAsyncById(int palestranteId, bool includeEventos = false) {
            
            IQueryable<Palestrante> query = _context.Palestrantes
                        .Include(c => c.RedesSociais);

            if(includeEventos){
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude( e => e.Evento);
            }
            query =  query.AsNoTracking().OrderBy( p => p.Nome)
                    .Where(p => p.Id == palestranteId );

            return await query.FirstOrDefaultAsync();

        }
         public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos) {
           IQueryable<Palestrante> query = _context.Palestrantes
                        .Include(c => c.RedesSociais);

            if(includeEventos){
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude( e => e.Evento);
            }
            query =  query.AsNoTracking().OrderBy( p => p.Nome).Where(p => p.Nome.ToLower().Contains(name.ToLower()));
            return await query.ToArrayAsync();
        }  


    }
}

//quando buscamos um recurso para depois ele ser deletado, pode ser que o entityframework esteja em tracking 
//ou seja, um ambiente onde ele trava o recurso para vc fazer a alteração.
//isso pode ocorrer muito, para não travar o recurso, podemos falar de forma geral que nosso ambiente é no tracking
//ou de forma especifica. O tracking só vai acontecer quando retornar um tipo especifico de recurso.
