namespace HamHam.Domain.Events
{
    public record BookmarkEvent(string Type, Guid EntityId, Guid UserId);
    public record CategoryEvent(string Type, Guid EntityId, Guid UserId);
}
