﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommunityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Community
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
        {
            return await _context.Communities.ToListAsync();
        }

        // GET: api/Community/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        // POST: api/Community
        [HttpPost]
        public async Task<ActionResult<Community>> CreateCommunity(Community community)
        {
            community.CreatedAt = DateTime.UtcNow;
            _context.Communities.Add(community);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommunity), new { id = community.Id }, community);
        }

        // PUT: api/Community/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunity(int id, Community community)
        {
            if (id != community.Id)
            {
                return BadRequest();
            }

            _context.Entry(community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Community/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return _context.Communities.Any(e => e.Id == id);
        }
    }
}