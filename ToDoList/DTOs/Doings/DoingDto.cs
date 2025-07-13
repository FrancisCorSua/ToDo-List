namespace ToDoList.Models.Doings
{
    // For POST y PATCH
    public class DoingRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;  // string for easier validation
    }

    // For GET (response)
    public class DoingResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}