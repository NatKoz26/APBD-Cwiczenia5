using System.ComponentModel.DataAnnotations;

namespace TrainingCenterApi.Models;

    public class Reservation
    {
        public int Id{get; set;}

        public int RoomId{get;set;}
        
        public required string OrganizerName{get; set;}

        public required string Topic {get; set;}

        public DateTime Date {get; set;}

        public TimeSpan StartTime{get;set;}

        public TimeSpan EndTime{get;set;}
        
        public required string Status {get;set;}
    }
