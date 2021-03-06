﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MantoxWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MantoxDBEntities : DbContext
    {
        public MantoxDBEntities()
            : base("name=MantoxDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Comentarios> Comentarios { get; set; }
        public virtual DbSet<Edificio> Edificios { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Licencia> Licencias { get; set; }
        public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<Movimiento> Movimientos { get; set; }
        public virtual DbSet<Parte> Partes { get; set; }
        public virtual DbSet<Propietario> Propietarios { get; set; }
        public virtual DbSet<Razon_Movimiento> Razones_Movimiento { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<Sede> Sedes { get; set; }
        public virtual DbSet<Sistema_Operativo> Sistemas_Operativos { get; set; }
        public virtual DbSet<Tipo_Equipo> Tipos_Equipo { get; set; }
        public virtual DbSet<Tipo_Licencia> Tipos_Licencia { get; set; }
        public virtual DbSet<Tipo_Mantenimiento> Tipos_Mantenimiento { get; set; }
        public virtual DbSet<Version_Office> Versiones_Office { get; set; }
        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<V_Edificios> V_Edificios { get; set; }
        public virtual DbSet<V_Modelos> V_Modelos { get; set; }
        public virtual DbSet<V_Sedes> V_Sedes { get; set; }
        public virtual DbSet<V_Usuarios> V_Usuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<V_Areas> V_Areas { get; set; }
        public virtual DbSet<V_Empresas> V_Empresas { get; set; }
        public virtual DbSet<V_Equipos> V_Equipos { get; set; }
        public virtual DbSet<Equipo> Equipos { get; set; }
    }
}
