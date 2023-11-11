using FluentValidation;

namespace BraveBrowser.ApiModels
{
    public class ActionSettingModel
    {
        public string ActionName { get; set; }

        public int AcctionDailyFrom { get; set; }

        public int AcctionDailyTo { get; set; }
    }

    public class ActionSettingModelValidation : AbstractValidator<ActionSettingModel>
    {
        public ActionSettingModelValidation() { 
            this.RuleFor(x => x.AcctionDailyFrom).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.AcctionDailyTo).GreaterThanOrEqualTo(1);
            this.RuleFor(x => x.AcctionDailyFrom).LessThan(x => x.AcctionDailyTo);
        }
    }
}