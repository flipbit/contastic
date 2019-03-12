namespace Contastic.Binding
{
    internal class BindResult : CanBindResult 
    {
        public virtual object Value { get; set; }
    }

    internal class BindResult<T> : BindResult 
    {
        public new T Value
        {
            get => (T) base.Value;
            set => base.Value = value;
        }
    }
}
