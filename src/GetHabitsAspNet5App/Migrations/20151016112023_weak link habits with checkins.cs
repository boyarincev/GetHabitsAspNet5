using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace GetHabitsAspNet5App.Migrations
{
    public partial class weaklinkhabitswithcheckins : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "HabitId",
                table: "Checkin",
                type: "bigint",
                nullable: true);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "HabitId",
                table: "Checkin",
                type: "bigint",
                nullable: false);
        }
    }
}
