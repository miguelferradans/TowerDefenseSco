namespace TD.Logic.Events
{
    public class EventsFacade
    {
        public EntityEvents Entity { get; private set; } = new EntityEvents();
        public LevelEvents Level { get; private set; } = new LevelEvents();
        public SlotEvents Slot { get; private set; } = new SlotEvents();
        public TowerEvents Tower { get; private set; } = new TowerEvents();
        public WaveEvents Waves { get; private set; } = new WaveEvents();
    }
}