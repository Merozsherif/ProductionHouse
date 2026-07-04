using AutoMapper;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductionHouse.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ===========================
            // Project
            // ===========================
            CreateMap<Project, ProjectDto>();
            CreateMap<AddProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            // ===========================
            // Project Translation
            // ===========================
            CreateMap<ProjectTranslation, ProjectTranslationDto>();
            CreateMap<ProjectTranslationDto, ProjectTranslation>();

            // ===========================
            // Category
            // ===========================
            CreateMap<Category, CategoryDto>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // ===========================
            // Category Translation
            // ===========================
            CreateMap<CategoryTranslation, CategoryTranslationDto>();
            CreateMap<AddCategoryTranslationDto, CategoryTranslation>();
        }
    }
}