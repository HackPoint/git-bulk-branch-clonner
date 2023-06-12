using FluentValidation;

namespace Application.Audit.Commands.CreateAudit;

public class CreateAuditCommandValidator : AbstractValidator<CreateAuditCommand> {
    public CreateAuditCommandValidator() {
        RuleFor(v => v.UpdatedBy)
            .NotEmpty();
    }
}