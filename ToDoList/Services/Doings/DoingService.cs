using ToDoList.Models.Doings;
using ToDoList.Models.Enums;

namespace ToDoList.Services.Doings;

public class DoingsService : IDoingsService
{
    private static readonly List<Doing> _doings = [];
    private static int _lastId = 0;

    public IEnumerable<Doing> GetAll()
    {
        if (_doings.Count == 0)
            SeedDoings();
        return _doings;
    }

    public Doing? GetById(int id)
    {
        return _doings.FirstOrDefault(d => d.Id == id);
    }

    public Doing Create(string name, string? stateStr)
    {
        var state = ParseState(stateStr) ?? Doing.DoingState.Pending;
        var doing = new Doing
        {
            Id = ++_lastId,
            Name = name,
            State = state,
            Summary = SummaryGenerator(name)
        };
        _doings.Add(doing);
        return doing;
    }

    public UpdateResult Update(int id, DoingRequestDto? updatedDto)
    {
        var doing = GetById(id);
        if (doing == null) return UpdateResult.NotFound;

        if (!string.IsNullOrWhiteSpace(updatedDto.Name) && doing.Name != updatedDto.Name)
        {
            doing.UpdateName(updatedDto.Name);
            doing.Summary = SummaryGenerator(updatedDto.Name);
        }

        if (string.IsNullOrWhiteSpace(updatedDto.State)) return UpdateResult.Success;
        var state = ParseState(updatedDto.State);
        if (state == null) return UpdateResult.InvalidState;
        doing.UpdateState(state.Value);

        return UpdateResult.Success;
    }

    public UpdateResult Delete(int id)
    {
        var doing = GetById(id);
        if (doing == null) return UpdateResult.NotFound;

        _doings.Remove(doing);
        return UpdateResult.Success;
    }

    private Doing.DoingState? ParseState(string stateStr)
    {
        return Enum.TryParse<Doing.DoingState>(stateStr, ignoreCase: true, out var state)
            ? state
            : (Doing.DoingState?)null;
    }

    private void SeedDoings()
    {
        var names = new[]
        {
            "Gym", "Langosta Bañera", "Niño Horno", "Bañera Horno", "Niño Langosta", "Bicicleta con los panas", "Bicicleta con las langostas"
        };

        var random = new Random();
        for (int i = 0; i < 5; i++)
        {
            var name = names[random.Next(names.Length)];
            Create(name, null);
        }
    }

    private static string SummaryGenerator(string name)
    {
        return name switch
        {
            "Gym" or "Bicicleta con los panas" => $"Ir a {name} es beneficioso ya que te pondrá en forma.",
            "Langosta Bañera" => $"Dejarte la {name} es peligroso ya que puede quedarse sin oxigeno en el agua.",
            "Niño Horno" => $"Dejarte el {name} es peligroso ya que el niño no podría salir.",
            "Bañera Horno" => $"Dejarte la {name} es difícil ya que no debería caber.",
            _ => $"La variable {name} no era esperada por el sistema."
        };
    }
}