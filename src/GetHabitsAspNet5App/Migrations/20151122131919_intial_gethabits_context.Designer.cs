using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using GetHabitsAspNet5App.Models.DomainModels;

namespace GetHabitsAspNet5App.Migrations
{
    [DbContext(typeof(GetHabitsContext))]
    [Migration("20151122131919_intial_gethabits_context")]
    partial class intial_gethabits_context
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GetHabitsAspNet5App.Models.DomainModels.Checkin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<long?>("HabitId");

                    b.Property<int>("State");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("GetHabitsAspNet5App.Models.DomainModels.Habit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("UserId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("GetHabitsAspNet5App.Models.DomainModels.Checkin", b =>
                {
                    b.HasOne("GetHabitsAspNet5App.Models.DomainModels.Habit")
                        .WithMany()
                        .HasForeignKey("HabitId");
                });
        }
    }
}
