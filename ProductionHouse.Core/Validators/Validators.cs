using FluentValidation;
using ProductionHouse.Core.DTOs;

namespace ProductionHouse.Core.Validators
{
    // ===========================
    // Add Category
    // ===========================
    public class AddCategoryDtoValidator : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidator()
        {
            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Image is required.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("At least one translation is required.");

            RuleForEach(x => x.Translations)
                .SetValidator(new AddCategoryTranslationDtoValidator());
        }
    }

    // ===========================
    // Category Translation
    // ===========================
    public class AddCategoryTranslationDtoValidator
        : AbstractValidator<AddCategoryTranslationDto>
    {
        public AddCategoryTranslationDtoValidator()
        {
            RuleFor(x => x.LanguageCode)
                .NotEmpty()
                .WithMessage("Language code is required.")
                .Length(2)
                .WithMessage("Language code must be 2 characters.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters.");
        }
    }

    // ===========================
    // Update Category
    // ===========================
    public class UpdateCategoryDtoValidator
        : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category id.");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Image is required.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("At least one translation is required.");

            RuleForEach(x => x.Translations)
                .SetValidator(new AddCategoryTranslationDtoValidator());
        }
    }

    // ===========================
    // Add Project
    // ===========================
    public class AddProjectDtoValidator
        : AbstractValidator<AddProjectDto>
    {
        public AddProjectDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category is required.");

            RuleFor(x => x.ClientName)
                .NotEmpty()
                .WithMessage("Client name is required.")
                .MaximumLength(100);

            RuleFor(x => x.CoverImage)
                .NotEmpty()
                .WithMessage("Cover image is required.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("At least one translation is required.");

            RuleForEach(x => x.Translations)
                .SetValidator(new ProjectTranslationDtoValidator());
        }
    }

    // ===========================
    // Project Translation
    // ===========================
    public class ProjectTranslationDtoValidator
        : AbstractValidator<ProjectTranslationDto>
    {
        public ProjectTranslationDtoValidator()
        {
            RuleFor(x => x.LanguageCode)
                .NotEmpty()
                .WithMessage("Language code is required.")
                .Length(2)
                .WithMessage("Language code must be 2 characters.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(2000)
                .WithMessage("Description cannot exceed 2000 characters.");
        }
    }

    // ===========================
    // Update Project
    // ===========================
    public class UpdateProjectDtoValidator
        : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid project id.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category is required.");

            RuleFor(x => x.ClientName)
                .NotEmpty()
                .WithMessage("Client name is required.")
                .MaximumLength(100);

            RuleFor(x => x.CoverImage)
                .NotEmpty()
                .WithMessage("Cover image is required.");

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("At least one translation is required.");

            RuleForEach(x => x.Translations)
                .SetValidator(new ProjectTranslationDtoValidator());
        }
    }
}