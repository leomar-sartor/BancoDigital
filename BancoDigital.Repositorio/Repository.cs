using BancoDigital.Entidades.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Carteira.Repository
{
    public class Repository<T> where T : class
    {
        #region Variaveis
        protected Context _ctx;
        #endregion

        #region Construtores
        public Repository(bool Homolagacao = false) => _ = Homolagacao == true ? _ctx = new Context(Homolagacao) : _ctx = new Context();

        public Repository(Context contexto) => _ctx = contexto;
        #endregion

        #region Atualizar
        public void Atualizar(T obj) =>
            _ctx.Entry(obj).State = EntityState.Modified;

        public async Task AtualizarAsync(T obj) =>
            await Task.Run(() => _ctx.Entry(obj).State = EntityState.Modified);
        #endregion

        #region Buscar
        public virtual List<T> Listar() => _ctx.Set<T>().ToList();
        public async Task<List<T>> ListarAsync() =>
             await Task.Run(() => Listar().ToList());

        public IQueryable<T> QueriableList() => _ctx.Set<T>();

        public virtual async Task<IQueryable<T>> QueriableListAsync() =>
             await Task.Run(() => QueriableList());

        public IQueryable<T> Buscar(Expression<Func<T, bool>> where)
            => _ctx.Set<T>().Where<T>(where);

        public async Task<IQueryable<T>> BuscarAsync(Expression<Func<T, bool>> where)
            => await Task.Run(() => Buscar(where));

        public T BuscarPorChave(params object[] key)
            => _ctx.Set<T>().Find(key);

        public async Task<T> BuscarPorChaveAsync(params object[] key)
            => await _ctx.Set<T>().FindAsync(key);

        #endregion

        #region Adicionar 
        public void Adicionar(T obj) =>
            _ctx.Set<T>().Add(obj);

        public async Task AdicionarAsync(T obj) =>
            await _ctx.Set<T>().AddAsync(obj);

        public void Adicionar(List<T> objts) =>
            _ctx.Set<T>().AddRange(objts);

        public async Task AdicionarAsync(List<T> objts) =>
            await _ctx.Set<T>().AddRangeAsync(objts);
        #endregion

        #region Salvar
        public void Salvar() =>
            _ctx.SaveChanges();

        public Task<int> SalvarAsync() =>
             _ctx.SaveChangesAsync();
        #endregion

        #region Excluir
        public void Excluir(Func<T, bool> predicate) =>
            _ctx.Set<T>().Where(predicate).ToList().ForEach(del => _ctx.Set<T>().Remove(del));

        public virtual async Task ExcluirAsync(Func<T, bool> predicate) =>
            await Task.Run(() => _ctx.Set<T>().Where(predicate).ToList().ForEach(del => _ctx.Set<T>().Remove(del)));
        #endregion
    }
}
