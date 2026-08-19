using Microsoft.EntityFrameworkCore;
using StudentResultApp.Data;
using StudentResultApp.Models;

namespace StudentResultApp.Services
{
    public class ModuleService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ModuleService(
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Module>> GetAllAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Modules
                .AsNoTracking()
                .OrderBy(m => m.Code)
                .ToListAsync();
        }

        public async Task<Module?> GetByIdAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Module module)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            context.Modules.Add(module);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Module module)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var existingModule =
                await context.Modules
                    .FirstOrDefaultAsync(m => m.Id == module.Id);

            if (existingModule == null)
                return;

            existingModule.Code = module.Code;
            existingModule.Name = module.Name;
            existingModule.AcademicYear = module.AcademicYear;
            existingModule.StudentCount = module.StudentCount;
            existingModule.Status = module.Status;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var module =
                await context.Modules
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (module == null)
                return;

            context.Modules.Remove(module);

            await context.SaveChangesAsync();
        }

        public async Task<int> GetTotalModulesAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Modules
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<List<ModulePerformanceDto>> GetModulePerformanceAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Modules
                .AsNoTracking()
                .Select(m => new ModulePerformanceDto
                {
                    Code = m.Code,
                    Name = m.Name,

                    AverageMark = context.StudentResults
                        .Where(r => r.ModuleId == m.Id)
                        .Select(r => (double?)r.Mark)
                        .Average() ?? 0
                })
                .OrderByDescending(m => m.AverageMark)
                .ToListAsync();
        }
    }
}