using System.ComponentModel.DataAnnotations;

namespace TrainingCenterApi.Models;

    public class Room
    {
        public int Id{get; set;}
       
        public required string Name {get; set; }

        public required string BuildingCode{get; set;}
        
        public int Floor {get; set;}

        [Range (1, int.MaxValue)]
        public int Capacity {get; set;}

        public bool HasProjector {get; set; }

        public bool IsActive {get; set; }
    }
