using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        // POST: api/tags
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] Tag tag)
        {
            var createdTag = await _tagService.CreateTagAsync(tag);
            return CreatedAtAction(nameof(GetAllTags), new { id = createdTag.Id }, createdTag);
        }

        // PUT: api/tags/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag tag)
        {
            var updatedTag = await _tagService.UpdateTagAsync(id, tag);
            if (updatedTag == null)
            {
                return NotFound();
            }

            return Ok(updatedTag);
        }

        // DELETE: api/tags/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var deleted = await _tagService.DeleteTagAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: api/tags/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTagToTask([FromBody] AssignTagRequest request)
        {
            var assigned = await _tagService.AssignTagToTaskAsync(request.TaskId, request.TagId);
            if (!assigned)
            {
                return BadRequest("Failed to assign tag to task");
            }

            return Ok("Tag assigned successfully");
        }
    }
}
