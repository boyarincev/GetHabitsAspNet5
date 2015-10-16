using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace GetHabitsAspNet5App.Migrations
{
    public partial class initial_migration : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "Habit",
                columns: table => new
                {
                    Id = table.Column(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habit", x => x.Id);
                });
            migration.CreateTable(
                name: "Checkin",
                columns: table => new
                {
                    Id = table.Column(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Date = table.Column(type: "datetime2", nullable: false),
                    HabitId = table.Column(type: "bigint", nullable: true),
                    State = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkin_Habit_HabitId",
                        columns: x => x.HabitId,
                        referencedTable: "Habit",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Checkin");
            migration.DropTable("Habit");
        }
    }
}
