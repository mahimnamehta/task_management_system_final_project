using MongoDB.Driver;

public interface ITagService
{
    Task<List<Task>> GetAllTagsAsync();
    Task<Task> GetTagByIdAsync(int id);
    Task<Task> CreateTagAsync(Tag tag);
    Task<Task> UpdateTagAsync(int id, Tag tag);
    Task<bool> DeleteTagAsync(int id);
    Task<bool> AssignTagToTaskAsync(int taskId, int tagId);
}
