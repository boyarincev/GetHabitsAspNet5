using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using GetHabitsAspNet5App.Models.DomainModels;

namespace GetHabitsAspNet5App.Migrations
{
    [ContextType(typeof(GetHabitsContext))]
    partial class GetHabitsContextModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:ValueGeneration", "Identity");
            
            builder.Entity("GetHabitsAspNet5App.Models.DomainModels.Checkin", b =>
                {
                    b.Property<long>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<DateTime>("Date");
                    
                    b.Property<long?>("HabitId");
                    
                    b.Property<int>("State");
                    
                    b.Key("Id");
                });
            
            builder.Entity("GetHabitsAspNet5App.Models.DomainModels.Habit", b =>
                {
                    b.Property<long>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<string>("Name");
                    
                    b.Key("Id");
                });
            
            builder.Entity("GetHabitsAspNet5App.Models.DomainModels.Checkin", b =>
                {
                    b.Reference("GetHabitsAspNet5App.Models.DomainModels.Habit")
                        .InverseCollection()
                        .ForeignKey("HabitId");
                });
        }
    }
}
