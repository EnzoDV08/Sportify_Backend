using System;
using System.Collections.Generic;

namespace SportifyApi.DTOs
{
    public class EventWithParticipantsDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;

        public UserDto EventAdmin { get; set; } = new UserDto();
        public List<EventParticipantDto> Participants { get; set; } = new List<EventParticipantDto>();
    }
}
