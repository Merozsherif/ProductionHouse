using AutoMapper;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Interfaces;

namespace ProductionHouse.Infrastructure.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ===================== CREATE =====================
    public async Task AddAsync(AddCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== DELETE =====================
    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException("Category not found.");

        _unitOfWork.Categories.Delete(category);

        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== GET ALL =====================
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

    // ===================== GET BY ID =====================
    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException("Category not found.");

        return _mapper.Map<CategoryDto>(category);
    }

    // ===================== UPDATE =====================
    public async Task UpdateAsync(UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);

        if (category == null)
            throw new NotFoundException("Category not found.");

        _mapper.Map(dto, category);

        _unitOfWork.Categories.Update(category);

        await _unitOfWork.SaveChangesAsync();
    }
}