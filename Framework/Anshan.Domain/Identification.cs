namespace Anshan.Domain
{
    public class Identification : ValueObject
    {
        private Identification()
        {
        }

        public Identification(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static implicit operator string(Identification id)
        {
            return id.Value;
        }

        public static implicit operator Identification(string id)
        {
            return new(id);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}