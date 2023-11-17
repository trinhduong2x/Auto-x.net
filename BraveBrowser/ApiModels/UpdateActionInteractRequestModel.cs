using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BraveBrowser.ApiModels
{
    public class UpdateActionInteractRequestModel
    {
        public string UserNameFrom { get; set; } 

        public string UserNameTo { get; set; }

        public string Action { get; set; }

        public string Link { get; set; }
    }

    public class UpdateActionInteractRequestModelValidation : AbstractValidator<UpdateActionInteractRequestModel>
    {
        public UpdateActionInteractRequestModelValidation()
        {
            this.RuleFor(x => x.UserNameFrom).NotNull().NotEmpty();
            this.RuleFor(x => x.UserNameTo).NotNull().NotEmpty();
            this.RuleFor(x => x.Link).NotNull().NotEmpty();
            this.RuleFor(x => x.Action).NotNull().NotEmpty();
        }
    }
}