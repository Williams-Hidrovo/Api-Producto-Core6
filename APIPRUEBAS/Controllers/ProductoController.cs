﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using APIPRUEBAS.Models;
using Microsoft.AspNetCore.Cors;
using System;

namespace APIPRUEBAS.Controllers
{
    //ACTIVAR CORS EN EL CONTROLLER
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DBApiContext _dbcontext;
        //constructor
        public ProductoController(DBApiContext _context) {
            _dbcontext = _context;
        }


        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista=_dbcontext.Productos.Include(c => c.oCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new {mensaje="ok",Response=lista});
            }catch (Exception ex) {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = lista });
            }
        }


        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = new Producto();
            if(oProducto == null) {
                return BadRequest("Producto no encontrado");
            }
            try
            {
                oProducto = _dbcontext.Productos.Include(c => c.oCategoria).Where(p  => p.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = oProducto });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public ActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                _dbcontext.Productos.Add(objeto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});

            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public ActionResult Editar([FromBody] Producto objeto)
        {
            Producto oProducto = _dbcontext.Productos.Find(objeto.IdProducto);
            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto.CodigoBarra=objeto.CodigoBarra is null ? oProducto.CodigoBarra : objeto.CodigoBarra;
                oProducto.Descripcion = objeto.Descripcion is null ? oProducto.Descripcion : objeto.Descripcion;
                oProducto.Marca = objeto.Marca is null ? oProducto.Marca : objeto.Marca;
                oProducto.IdCategoria = objeto.IdCategoria is null ? oProducto.IdCategoria : objeto.IdCategoria;
                oProducto.Precio = objeto.Precio is null ? oProducto.Precio : objeto.Precio;

                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public ActionResult Eliminar(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);
            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
               
                _dbcontext.Productos.Remove(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }

        }









    }
}
