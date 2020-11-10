using LanchesArqui.Context;
using LanchesArqui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesArqui.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;
        public CategoriaRepository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
