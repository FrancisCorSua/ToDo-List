namespace ToDoList.Models.Doings;

public class Doings
{
    public enum DoingState
    {
        Pending,
        InProgress,
        Completed
    }
    
    public int ID { get; set; }
    
    public string Name { get; set; }
    
    public DoingState State { get; set; }
    
    public string? Summary { get; set; }
}