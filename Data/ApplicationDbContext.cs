using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apiReceitasC_.Models;
using apiReceitasC_.Enums;

namespace apiReceitasC_.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<Receita> Receitas { get; set; }

    public virtual DbSet<ReceitaIngrediente> ReceitaIngredientes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<UnidadeMedida>();

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredientes_pkey");

            entity.ToTable("ingredientes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Receita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receitas_pkey");

            entity.ToTable("receitas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .HasColumnName("titulo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Receita)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("receitas_usuario_id_fkey");
        });

        modelBuilder.Entity<ReceitaIngrediente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receita_ingredientes_pkey");

            entity.ToTable("receita_ingredientes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngredienteId).HasColumnName("ingrediente_id");
            entity.Property(e => e.Quantidade)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade");
            entity.Property(e => e.ReceitaId).HasColumnName("receita_id");

            entity.Property(e => e.Unidade).HasColumnName("unidade");

            entity.HasOne(d => d.Ingrediente).WithMany(p => p.ReceitaIngredientes)
                .HasForeignKey(d => d.IngredienteId)
                .HasConstraintName("receita_ingredientes_ingrediente_id_fkey");

            entity.HasOne(d => d.Receita).WithMany(p => p.ReceitaIngredientes)
                .HasForeignKey(d => d.ReceitaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("receita_ingredientes_receita_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .HasColumnName("senha");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
