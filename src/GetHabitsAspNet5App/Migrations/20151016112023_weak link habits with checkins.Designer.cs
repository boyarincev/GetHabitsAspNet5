using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using GetHabitsAspNet5App.Models.DomainModels;

namespace GetHabitsAspNet5App.Migrations
{
    [ContextType(typeof(GetHabitsContext))]
    partial class weaklinkhabitswithcheckins
    {
        public override string Id
        {
            get { return "20151016112023_weak link habits with checkins"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta5-13549"; }
        }
        
        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:DefaultSequenceName", "DefaultSequence")
                .Annotation("SqlServer:Sequence:.DefaultSequence", "'DefaultSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .Annotation("SqlServer:ValueGeneration", "Sequence");
            
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
