namespace ToDoList.Models.Doings
{
    public class Doing
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DoingState State { get; set; }
        public string Summary { get; set; } = string.Empty;

        public enum DoingState
        {
            Pending,
            InProgress,
            Completed
        }

        public void UpdateName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name != Name)
            {
                Name = name;
                UpdateSummary(name);
            }
        }

        public void UpdateState(DoingState state)
        {
            State = state;
        }

        public void UpdateSummary(string name)
        {
            string completeSummary = name switch
            {
                "Gym" or "Bicicleta con los panas" => "Ir a " + name + " es beneficioso ya que te pondrá en forma.",
                "Langosta Bañera" => "Dejarte la " + name +
                                     " es peligroso ya que puede quedarse sin oxigeno en el agua.",
                "Niño Horno" => "Dejarte el " + name + " es peligroso ya que el niño no podría salir.",
                "Bañera Horno" => "Dejarte la " + name + " es difícil ya que no debería caber.",
                _ => "La variable " + name + " no era esperada por el sistema."
            };

            Summary = completeSummary;
        }
    }
}