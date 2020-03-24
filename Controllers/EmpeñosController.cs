﻿using Aplicada2ProyectoFinal.Data;
using Aplicada2ProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aplicada2ProyectoFinal.Controllers
{
    public class EmpeñosController
    {
        public bool Guardar(Empeños Empeño)
        {
            Contexto contexto = new Contexto();
            bool paso = false;
            try
            {
                if (Empeño.EmpeñoId == 0)
                {
                    paso = Insertar(Empeño);

                }
                else
                {
                    paso = Modificar(Empeño);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return paso;
        }
        private bool Insertar(Empeños Empeño)
        {
            Contexto contexto = new Contexto();
            bool paso = false;

            try
            {
                if (contexto.Empeños.Add(Empeño) != null)
                {
                    foreach (var item in Empeño.Detalle)
                    {
                        contexto.Articulos.Find(item.ArticuloId).Inventario += item.Cantidad;
                    }
                }
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }
        public bool Modificar(Empeños empeño)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                var recibos = Buscar(empeño.EmpeñoId);
                if (recibos != null)
                {
                    foreach (var item in recibos.Detalle)
                    {
                        contexto.Articulos.Find(item.ArticuloId).Inventario -= item.Cantidad;
                        if (!empeño.Detalle.ToList().Exists(v => v.ID == item.ID))
                        {
                            item.Articulos = null;
                            contexto.Entry(item).State = EntityState.Deleted;
                        }
                    }
                    foreach (var item in empeño.Detalle)
                    {
                        contexto.Articulos.Find(item.ArticuloId).Inventario += item.Cantidad;
                        var estado = item.ID > 0 ? EntityState.Modified : EntityState.Added;
                        contexto.Entry(item).State = estado;
                    }
                    Empeños EntradaAnterior = Buscar(empeño.EmpeñoId);
                    decimal diferencia;
                    diferencia = EntradaAnterior.MontoTotal - empeño.MontoTotal;
                    contexto.Entry(empeño).State = EntityState.Modified;
                }
                if (contexto.SaveChanges() > 0)
                {
                    paso = true;
                }
                contexto.Dispose();
            }
            catch (Exception) { throw; }
            return paso;
        }

        public bool Eliminar(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                Empeños recibo = contexto.Empeños.Find(id);
                if (recibo != null)
                {
                    foreach (var item in recibo.Detalle)
                    {
                        contexto.Articulos.Find(item.ArticuloId).Inventario -= item.Cantidad;
                    }
                    recibo.Detalle.Count();
                    contexto.Empeños.Remove(recibo);
                }
                if (contexto.SaveChanges() > 0)
                {
                    paso = true;
                }
                contexto.Dispose();
            }
            catch (Exception) { throw; }
            return paso;
        }
        public bool EliminarParaCobro(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                Empeños recibos = contexto.Empeños.Find(id);
                if (recibos != null)
                {
                    recibos.Detalle.Count();
                    contexto.Empeños.Remove(recibos);
                }
                if (contexto.SaveChanges() > 0)
                {
                    paso = true;
                    contexto.Dispose();
                }
            }
            catch (Exception) { throw; }
            return paso;
        }
        public Empeños Buscar(int id)
        {
            Empeños recibo = new Empeños();
            Contexto contexto = new Contexto();
            try
            {
                recibo = contexto.Empeños.Find(id);
                if (recibo != null)
                {
                    recibo.Detalle.Count();
                    foreach (var item in recibo.Detalle)
                    {
                        string s = item.Articulos.Nombre;
                    }
                }
                contexto.Dispose();
            }
            catch (Exception) { throw; }
            return recibo;
        }
        public List<Empeños> GetList(Expression<Func<Empeños, bool>> expression)
        {
            Contexto contexto = new Contexto();
            List<Empeños> recibo = new List<Empeños>();
            try
            {
                recibo = contexto.Empeños.Where(expression).ToList();

                foreach (var item in recibo)
                {
                    item.Detalle.Count();
                }
                contexto.Dispose();
            }
            catch (Exception) { throw; }
            return recibo;
        }
        public static bool ModificarEspecial(Empeños recibo)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            EmpeñosController controller = new EmpeñosController();
            try
            {
                Empeños Anterior = controller.Buscar(recibo.EmpeñoId);
                decimal diferencia;
                diferencia = Anterior.Abono - recibo.Abono;
                Empeños recibos = controller.Buscar(recibo.EmpeñoId);
                recibos.Abono = Math.Abs(recibos.Abono - diferencia);
                contexto.Entry(recibo).State = EntityState.Modified;
                if (contexto.SaveChanges() > 0)
                {
                    paso = true;
                }
                contexto.Dispose();
            }
            catch (Exception) { throw; }
            return paso;
        }

    }
}
