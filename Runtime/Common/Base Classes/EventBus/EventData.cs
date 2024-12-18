namespace ModularEventArchitecture
{
    public class newType : IEventType
    {
        public static readonly newType TestEventNew = new newType(EnumNew.TestEventNew);

        private EnumNew _type;

        private newType(EnumNew type)
        {
            _type = type;
        }

        public int GetEventId() => (int)_type;
        public string GetEventName() => _type.ToString();

        public enum EnumNew
        {
            TestEventNew
        }
    }
}