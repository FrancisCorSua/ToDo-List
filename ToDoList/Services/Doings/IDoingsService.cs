using ToDoList.Models.Doings;
using ToDoList.Models.Enums;

namespace ToDoList.Services.Doings;

public interface IDoingsService
{
    IEnumerable<Doing> GetAll();
    Doing? GetById(int id);
    Doing Create(string name, string? stateStr);
    UpdateResult Update(int id, DoingRequestDto? updatedDto);
    UpdateResult Delete(int id);
}