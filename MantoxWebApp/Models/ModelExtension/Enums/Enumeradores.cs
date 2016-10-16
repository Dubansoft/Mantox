using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MantoxWebApp
{

    /// <summary>
    /// Listado de roles de usuario, deben concordar con la tabla dbo.Roles de la base de datos.
    /// </summary>
    public enum RoleDeUsuario
    {
        Desarrollador = 1,
        Administrador = 2,
        Reportes = 3

    }

    /// <summary>
    /// Listado de estados, deben concordar con la tabla dbo.Estados de la base de datos.
    /// </summary>
    public enum EstadoMantox
    {
        Activo = 1,
        Inactivo = 2,
        Mantenimiento = 3,
        Reparación = 4,
        Garantía = 5,
        Baja = 6,
        Contingencia = 7
    }
}