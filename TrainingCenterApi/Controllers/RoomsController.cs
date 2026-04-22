using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Cotrollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(InMemoryData.Rooms);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);
            if(room == null) return NotFound();
            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public IActionResult GetByBuilding(string buildingCode)
        {
            var rooms = InMemoryData.Rooms
                .Where(r=>r.BuildingCode == buildingCode)
                .ToList();
            return Ok(rooms);
        }

        [HttpGet("filter")]
        public IActionResult Filter(int? minCapacity, bool? hasProjector, bool? activeOnly)
        {
            var query = InMemoryData.Rooms.AsQueryable();

            if(minCapacity.HasValue)
                query = query.Where(r => r.Capacity >= minCapacity);
            
            if(hasProjector.HasValue)
                query = query.Where(r => r.HasProjector == hasProjector);
            
            if(activeOnly == true)
                query = query.Where(r => r.IsActive);
            
            return Ok(query.ToList());
        }

        [HttpPost]
        public IActionResult Create (Room room)
        {
            room.Id = InMemoryData.Rooms.Max(r => r.Id) + 1;
            InMemoryData.Rooms.Add(room);

            return CreatedAtAction(nameof(GetById), new {id = room.Id}, room);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Room updatedRoom)
        {
            var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);
            if(room ==null) return NotFound();

            room.Name = updatedRoom.Name;
            room.BuildingCode = updatedRoom.BuildingCode;
            room.Floor = updatedRoom.Floor;
            room.Capacity = updatedRoom.Capacity;
            room.HasProjector = updatedRoom.HasProjector;
            room.IsActive = updatedRoom.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            var hasReservations = InMemoryData.Reservations.Any(r => r.RoomId == id);
            if(hasReservations)
                return Conflict("Room has reservations");
            InMemoryData.Rooms.Remove(room);
            return NoContent();
        }


    }
}