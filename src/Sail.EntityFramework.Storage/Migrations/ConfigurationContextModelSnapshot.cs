﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sail.EntityFramework.Storage;

#nullable disable

namespace Sail.EntityFramework.Storage.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    partial class ConfigurationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sail.Core.Entities.Certificate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Cert")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("Sail.Core.Entities.Cluster", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LoadBalancingPolicy")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("Sail.Core.Entities.Destination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Health")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Host")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.ToTable("Destination");
                });

            modelBuilder.Entity("Sail.Core.Entities.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorizationPolicy")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<string>("CorsPolicy")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uuid");

                    b.Property<long?>("MaxRequestBodySize")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("RateLimiterPolicy")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<TimeSpan?>("Timeout")
                        .HasColumnType("interval");

                    b.Property<string>("TimeoutPolicy")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.HasIndex("MatchId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteHeader", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCaseSensitive")
                        .HasMaxLength(10)
                        .HasColumnType("boolean");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uuid");

                    b.Property<int>("Mode")
                        .HasMaxLength(20)
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Values")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("RouteHeader");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteMatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Hosts")
                        .HasColumnType("text");

                    b.Property<string>("Methods")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("RouteMatch");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteQueryParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCaseSensitive")
                        .HasMaxLength(10)
                        .HasColumnType("boolean");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uuid");

                    b.Property<int>("Mode")
                        .HasMaxLength(20)
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Values")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("RouteQueryParameter");
                });

            modelBuilder.Entity("Sail.Core.Entities.SNI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CertificateId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HostName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CertificateId");

                    b.ToTable("SNI");
                });

            modelBuilder.Entity("Sail.Core.Entities.Destination", b =>
                {
                    b.HasOne("Sail.Core.Entities.Cluster", "Cluster")
                        .WithMany("Destinations")
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cluster");
                });

            modelBuilder.Entity("Sail.Core.Entities.Route", b =>
                {
                    b.HasOne("Sail.Core.Entities.Cluster", "Cluster")
                        .WithMany()
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sail.Core.Entities.RouteMatch", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cluster");

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteHeader", b =>
                {
                    b.HasOne("Sail.Core.Entities.RouteMatch", "Match")
                        .WithMany("Headers")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteQueryParameter", b =>
                {
                    b.HasOne("Sail.Core.Entities.RouteMatch", "Match")
                        .WithMany("QueryParameters")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Sail.Core.Entities.SNI", b =>
                {
                    b.HasOne("Sail.Core.Entities.Certificate", "Certificate")
                        .WithMany("SNIs")
                        .HasForeignKey("CertificateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Certificate");
                });

            modelBuilder.Entity("Sail.Core.Entities.Certificate", b =>
                {
                    b.Navigation("SNIs");
                });

            modelBuilder.Entity("Sail.Core.Entities.Cluster", b =>
                {
                    b.Navigation("Destinations");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteMatch", b =>
                {
                    b.Navigation("Headers");

                    b.Navigation("QueryParameters");
                });
#pragma warning restore 612, 618
        }
    }
}
