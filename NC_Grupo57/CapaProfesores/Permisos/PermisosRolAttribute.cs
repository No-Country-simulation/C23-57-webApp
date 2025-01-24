using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio;

namespace CapaProfesores.Permisos
{
    public class PermisosRolAttribute : ActionFilterAttribute
    {

        private int _idRol;

        public PermisosRolAttribute(int idRol)
        {
            _idRol = idRol;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/login");
                return;
            }

            Usuario user = HttpContext.Current.Session["Usuario"] as Usuario;

            if (user == null || user.Id_Rol != _idRol || !user.Activo)
            {
                filterContext.Result = new RedirectResult("~/Home/login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}