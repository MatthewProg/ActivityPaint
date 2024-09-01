using ActivityPaint.Application.DTOs.Repository;

namespace ActivityPaint.Client.Components.Configuration;

public static class ConfigurationModelMap
{
    public static RepositoryConfigModel ToRepositoryConfigModel(this ConfigurationModel model) => new(
        MessageFormat: model.MessageFormat,
        AuthorEmail: model.AuthorEmail,
        AuthorFullName: model.AuthorFullName
    );

    public static void MapFromRepositoryConfigModel(RepositoryConfigModel repositoryConfigModel, ConfigurationModel configurationModel)
    {
        configurationModel.MessageFormat = repositoryConfigModel.MessageFormat;
        configurationModel.AuthorEmail = repositoryConfigModel.AuthorEmail;
        configurationModel.AuthorFullName = repositoryConfigModel.AuthorFullName;
    }
}
