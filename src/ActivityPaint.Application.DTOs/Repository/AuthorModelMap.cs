namespace ActivityPaint.Application.DTOs.Repository;

public static class AuthorModelMap
{
    public static AuthorModel ToAuthorModel(this RepositoryConfigModel model) => new(
        FullName: model.AuthorFullName ?? "Activity Paint",
        Email: model.AuthorEmail ?? "email@example.com"
    );
}
