namespace Domain.Entities
{
    internal interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
