
namespace GameSchedule.Game
{
    public class GameTemplate
    {
        public long Id { get; set; }
        public int? PlayerMax { get; set; }
        public int? PlayerMin { get; set; }
        public string GameName { get; set; } = string.Empty;
        public DateTime RegistrationDate {  get; set; }
    }
}
