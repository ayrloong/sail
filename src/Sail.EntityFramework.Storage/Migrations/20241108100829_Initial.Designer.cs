﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sail.EntityFramework.Storage;

#nullable disable

namespace Sail.EntityFramework.Storage.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20241108100829_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sail.Core.Entities.Cluster", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LoadBalancingPolicy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

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
                        .HasColumnType("text");

                    b.Property<Guid?>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Health")
                        .HasColumnType("text");

                    b.Property<string>("Host")
                        .HasColumnType("text");

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
                        .HasColumnType("text");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<string>("CorsPolicy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uuid");

                    b.Property<long?>("MaxRequestBodySize")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("RateLimiterPolicy")
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("Timeout")
                        .HasColumnType("interval");

                    b.Property<string>("TimeoutPolicy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteHeader", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCaseSensitive")
                        .HasColumnType("boolean");

                    b.Property<int>("Mode")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("RouteMatchId")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("Values")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("RouteMatchId");

                    b.ToTable("RouteHeader");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteMatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("Hosts")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Methods")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RouteMatch");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteQueryParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCaseSensitive")
                        .HasColumnType("boolean");

                    b.Property<int>("Mode")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("RouteMatchId")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("Values")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("RouteMatchId");

                    b.ToTable("RouteQueryParameter");
                });

            modelBuilder.Entity("Sail.Core.Entities.WeightedCluster", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClusterId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("RouteId")
                        .HasColumnType("uuid");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("WeightedCluster");
                });

            modelBuilder.Entity("Sail.Core.Entities.Destination", b =>
                {
                    b.HasOne("Sail.Core.Entities.Cluster", null)
                        .WithMany("Destinations")
                        .HasForeignKey("ClusterId");
                });

            modelBuilder.Entity("Sail.Core.Entities.Route", b =>
                {
                    b.HasOne("Sail.Core.Entities.RouteMatch", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteHeader", b =>
                {
                    b.HasOne("Sail.Core.Entities.RouteMatch", null)
                        .WithMany("Headers")
                        .HasForeignKey("RouteMatchId");
                });

            modelBuilder.Entity("Sail.Core.Entities.RouteQueryParameter", b =>
                {
                    b.HasOne("Sail.Core.Entities.RouteMatch", null)
                        .WithMany("QueryParameters")
                        .HasForeignKey("RouteMatchId");
                });

            modelBuilder.Entity("Sail.Core.Entities.WeightedCluster", b =>
                {
                    b.HasOne("Sail.Core.Entities.Route", null)
                        .WithMany("WeightedClusters")
                        .HasForeignKey("RouteId");
                });

            modelBuilder.Entity("Sail.Core.Entities.Cluster", b =>
                {
                    b.Navigation("Destinations");
                });

            modelBuilder.Entity("Sail.Core.Entities.Route", b =>
                {
                    b.Navigation("WeightedClusters");
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