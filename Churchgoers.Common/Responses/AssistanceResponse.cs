namespace Churchgoers.Common.Responses
{
    public class AssistanceResponse
    {
        public int Id { get; set; }

        public UserResponse User { get; set; }

        public MeetingResponse Meeting { get; set; }

        public bool IsPresent { get; set; }
    }
}
