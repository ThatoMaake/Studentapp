using Microsoft.EntityFrameworkCore;
using StudentResultApp.Data;
using StudentResultApp.Models;

namespace StudentResultApp.Services
{
    public class StudentResultService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public StudentResultService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<StudentResult>> GetAllAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.StudentResults
                .Include(r => r.Module)
                .AsNoTracking()
                .OrderBy(r => r.StudentNumber)
                .ToListAsync();
        }

        public async Task AddAsync(StudentResult studentResult)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            studentResult.Result =
                studentResult.Mark >= 50
                    ? "Pass"
                    : "Fail";

            context.StudentResults.Add(studentResult);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentResult studentResult)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var existingResult =
                await context.StudentResults
                    .FirstOrDefaultAsync(r => r.Id == studentResult.Id);

            if (existingResult == null)
                return;

            existingResult.StudentNumber =
                studentResult.StudentNumber;

            existingResult.FullName =
                studentResult.FullName;

            existingResult.ModuleId =
                studentResult.ModuleId;

            existingResult.Mark =
                studentResult.Mark;

            existingResult.Result =
                studentResult.Mark >= 50
                    ? "Pass"
                    : "Fail";

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var studentResult =
                await context.StudentResults
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (studentResult == null)
                return;

            context.StudentResults.Remove(studentResult);

            await context.SaveChangesAsync();
        }
    }
}