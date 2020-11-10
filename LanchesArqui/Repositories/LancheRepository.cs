using LanchesArqui.Context;
using LanchesArqui.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesArqui.Repositories
{
    public class LancheRepository : ILancheRepository
    {
        public readonly AppDbContext _context;
        public LancheRepository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public IEnumerable<Lanche> Lanches => _context.Lanches.Include(c => c.Categoria); // retorna lanche e categoria

        public IEnumerable<Lanche> LanchesPreferidos => _context.Lanches.Where(p => p.IsLanchePreferido)
                                                                        .Include(c => c.Categoria);

        public Lanche GetLancheById(int lancheId)
        {
            return _context.Lanches.FirstOrDefault(l => l.Id == lancheId);
        }
    }
}
