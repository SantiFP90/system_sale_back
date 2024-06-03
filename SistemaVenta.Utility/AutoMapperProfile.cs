using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVentas.DTO;
using SistemaVentas.Model;


namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        //Con ctor podemos crear rapidamente el constructor de las clases
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion

            #region Usuario
            //Mapeo con propiedades diferentes 
                CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                //aqui estamos diciendo que la propiedad rol descripcion que se encuentra en destino, vamos a optener la propiedad desde el origen, con IdRolnavegaition.Nombre
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.EsActivo,
                op => op.MapFrom(
                origen => (origen.EsActivo == true ? 1 : 0)
                )
                );

                CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                );

                CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                destino.IdRolNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.EsActivo,
                op => op.MapFrom(
                origen => (origen.EsActivo == 1 ? true : false)
                )
                );
            #endregion

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion


            #region Producto
            CreateMap<Producto, ProductoDTO>()
            .ForMember(destino =>
            destino.DescripcionCategoria,
            opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
            )
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.EsActivo,
            op => op.MapFrom(
            origen => (origen.EsActivo == true ? 1 : 0))
            );
            //Aca lo que hacemos es el "reverse" de estos dtos personalizados
            CreateMap<ProductoDTO, Producto>()
            .ForMember(destino =>
            destino.IdCategoriaNavigation,
            opt => opt.Ignore()
            )
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.EsActivo,
            op => op.MapFrom(
            origen => (origen.EsActivo == 1 ? true : false))
            );
            #endregion

            #region Venta
            CreateMap<Venta, VentaDTO>()
            .ForMember(destino =>
            destino.TotalTexto, 
            opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.FechaRegistro,
            opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("ddMMyyyy")
            ));
            CreateMap<VentaDTO, Venta>()
            .ForMember(destino =>
            destino.Total,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR")))
            );
            #endregion

            #region DetalleVenta
            //Recortdar que destino siempre es el dto, y origen es la clase.
            CreateMap<DetalleVenta, DetalleVentaDTO>()
            .ForMember(destino =>
            destino.DescripcionProductos,
            opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
            )
            .ForMember(destino =>
            destino.PrecioTexto,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Precio, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.TotalTexto,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Total, new CultureInfo("es-AR")))
            );

            CreateMap<DetalleVentaDTO, DetalleVenta>()
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.Total,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR")))
            );
            #endregion

            #region Reportes
            CreateMap<DetalleVenta, ReporteDTO>()
            .ForMember(destino =>
            destino.FechaRegistro,
            opt => opt.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("ddMMyyyy")
            ))
            .ForMember(destino =>
            destino.NumeroDocumento,
            opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento))
            .ForMember(destino =>
            destino.Tipopago,
            opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago))
            .ForMember(destino =>
            destino.TotalVenta,
            opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-AR")))
            )
            .ForMember(destino =>
            destino.Producto,
            opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToString(origen.IdProductoNavigation.Precio.Value, new CultureInfo("es-AR"))))
            .ForMember(destino =>
             destino.Total,
             opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))));
            #endregion

        }
    }
}
