namespace HamHam.Api.Models
{
    public record PublicImportRequest(
        List<string> PublicBookmarkIds,
        string TargetCategoryId,
        string Color,
        bool Glass
    );
}
