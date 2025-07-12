using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers.Doings;

[ApiController]
[Route("[controller]")]
public class DoingsController: ControllerBase
{
    // Lista estática para guardar los Doings
    private static List<Models.Doings.Doings> _doings = new List<Models.Doings.Doings>();
    private static int _lastId = 0;
    
    private static readonly string[] Names = new[]
    {
        "Gym", "Langosta Bañera", "Niño Horno", "Bañera Horno", "Niño Langosta", "Bicicleta con los panas", "Bicicleta con las langostas"
    };
    
    [HttpGet]
    public IEnumerable<Models.Doings.Doings> Get()
    {
        if (_doings.Any())
            return _doings;

        for (int i = 0; i < 5; i++)
        {
            var name = Names[Random.Shared.Next(Names.Length)];
            _doings.Add(CreateDoing(name));
        }

        return _doings;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var doing = _doings.FirstOrDefault(d => d.ID == id);
        if (doing == null)
            return NotFound();

        return Ok(doing);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] Models.Doings.Doings newDoing)
    {
        if (newDoing == null || string.IsNullOrWhiteSpace(newDoing.Name))
            return BadRequest("El objeto o el nombre no pueden ser nulos.");

        var createdDoing = CreateDoing(newDoing.Name, newDoing.State);
        _doings.Add(createdDoing);

        // Devuelvo Created (201) con el nuevo objeto y su ubicación
        return CreatedAtAction(nameof(GetById), new { id = createdDoing.ID }, createdDoing);
    }
    
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] Models.Doings.Doings updatedDoing)
    {
        if (updatedDoing == null)
            return BadRequest();

        var doing = _doings.FirstOrDefault(d => d.ID == id);
        if (doing == null)
            return NotFound();

        // Evitar cambiar ID
        updatedDoing.ID = doing.ID;

        // Actualizar solo los campos permitidos, por ejemplo:
        if (!string.IsNullOrWhiteSpace(updatedDoing.Name))
            doing.Name = updatedDoing.Name;

        doing.State = updatedDoing.State;

        // Actualizar summary si cambia el Name
        doing.Summary = SummaryGenerator(doing.Name);

        return Ok(doing);  // Devuelvo el objeto actualizado
    }
    private Models.Doings.Doings CreateDoing(string name, Models.Doings.Doings.DoingState? state = null)
    {
        var values = Enum.GetValues(typeof(Models.Doings.Doings.DoingState));
        var randomState = (Models.Doings.Doings.DoingState)values.GetValue(Random.Shared.Next(values.Length));

        return new Models.Doings.Doings
        {
            ID = ++_lastId,
            Name = name,
            State = state ?? randomState,  // si state es null, uso random
            Summary = SummaryGenerator(name)
        };
    }

    private String SummaryGenerator(String RandomName)
    {
        switch (RandomName)
        {
            case "Gym":
            case "Bicicleta con los panas":
                return "Ir a " + RandomName + " es beneficioso ya que te pondrá en forma.";
            case "Langosta Bañera":
                return "Dejarte la " + RandomName + " es peligroso ya que puede quedarse sin oxigeno en el agua.";
            case "Niño Horno":
                return "Dejarte el " + RandomName + " es peligroso ya que el niño no podría salir.";
            case "Bañera Horno":
                return "Dejarte la " + RandomName + " es difícil ya que no debería caber.";
            default:
                return "La variable " + RandomName + " no era esperada por el sistema.";
        }
    }
}