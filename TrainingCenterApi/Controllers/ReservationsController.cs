using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(InMemoryData.Reservations);
        }
    

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
        {
            var res = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();

            return Ok(res);
        }
    
    [HttpGet("filter")]
        public IActionResult Filter(DateTime? date, string status, int? roomId)
        {
            var query = InMemoryData.Reservations.AsQueryable();

            if (date.HasValue)
                query = query.Where(r => r.Date.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId);

            return Ok(query.ToList());
        }
    
    [HttpPost]
    public IActionResult Create(Reservation reservation)
        {
            var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

            if(room == null) return NotFound("Room not found");
            if(!room.IsActive) return Conflict("Room is inactive");

            bool conflict = InMemoryData.Reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.Date.Date == reservation.Date.Date &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime
            );

            if (conflict)
                return Conflict("Time conflict");

            reservation.Id = InMemoryData.Reservations.Max(r => r.Id) + 1;
            InMemoryData.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

    
        [HttpPut("{id}")]
        public IActionResult Update(int id, Reservation updated)
        {
            var res = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();

            res.RoomId = updated.RoomId;
            res.OrganizerName = updated.OrganizerName;
            res.Topic = updated.Topic;
            res.Date = updated.Date;
            res.StartTime = updated.StartTime;
            res.EndTime = updated.EndTime;
            res.Status = updated.Status;

            return Ok(res);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();

            InMemoryData.Reservations.Remove(res);
            return NoContent();
        } 
    

}