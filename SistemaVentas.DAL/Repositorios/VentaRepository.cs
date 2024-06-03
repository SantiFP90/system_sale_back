using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.DBContext;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly QualitySantiagoFernandezContext _dbContext;

        public VentaRepository(QualitySantiagoFernandezContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            //Esta transaccion nos permitira realizar la logica temporalmente, asegurandonos, que todo va a salir bien al momento de retornar la venta
            using (var trasaction = _dbContext.Database.BeginTransaction())
            {
                try 
                {
                    //Recorremos en el detalle de venta cada producto, por ende creamos un objeto producto encontrado, buscamos su id y en base de datos restamos el stock del mismo x la cantidad de productos que se detallan en la venta
                    foreach(DetalleVenta dv in modelo.DetalleVenta) {

                        Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Catnidad;
                       //Actualizamos el producto en base de datos
                        _dbContext.Productos.Update(producto_encontrado);
                    }

                    //Guardamos los cambios
                    await _dbContext.SaveChangesAsync();
                    
                    //Registramos el documento de venta
                    NumeroDocumento correlativo = _dbContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    //Updateamos el objeto
                    _dbContext.NumeroDocumentos.Update(correlativo);

                    //Guardamos los cambios
                    await _dbContext.SaveChangesAsync();

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    //Por ejemplo 00001
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);
                    //0001

                    modelo.NumeroDocumento = numeroVenta;

                    //Actualizamos en la venta el numero de la misma
                    await _dbContext.Venta.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    //Guarda los cambios temporalmente "si todo salio bien"
                    trasaction.Commit();

                }
                catch
                {
                    //Si existe algun error en el proceso de la transaccion, restablecera todo nuevamente
                    trasaction.Rollback();
                    throw;
                }
            }
            return ventaGenerada;
        }
    }
}
