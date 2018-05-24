﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PlanningPokerBackend.Models;
using System;

namespace PlanningPokerBackend.Migrations
{
    [DbContext(typeof(PlanningPokerDbContext))]
    partial class PlanningPokerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlanningPokerBackend.Models.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("InviterId");

                    b.Property<int?>("ParticipantId");

                    b.Property<int?>("PlayTableId");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("InviterId");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("PlayTableId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("PlanningPokerBackend.Models.PlayTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AdminId");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("PlayTables");
                });

            modelBuilder.Entity("PlanningPokerBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsOnline");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<int?>("PlayTableId");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("PlayTableId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlanningPokerBackend.Models.Invitation", b =>
                {
                    b.HasOne("PlanningPokerBackend.Models.User", "Inviter")
                        .WithMany()
                        .HasForeignKey("InviterId");

                    b.HasOne("PlanningPokerBackend.Models.User", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantId");

                    b.HasOne("PlanningPokerBackend.Models.PlayTable", "PlayTable")
                        .WithMany()
                        .HasForeignKey("PlayTableId");
                });

            modelBuilder.Entity("PlanningPokerBackend.Models.PlayTable", b =>
                {
                    b.HasOne("PlanningPokerBackend.Models.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("PlanningPokerBackend.Models.User", b =>
                {
                    b.HasOne("PlanningPokerBackend.Models.PlayTable", "PlayTable")
                        .WithMany("Participants")
                        .HasForeignKey("PlayTableId");
                });
#pragma warning restore 612, 618
        }
    }
}
