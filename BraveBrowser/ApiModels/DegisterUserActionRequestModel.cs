﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BraveBrowser.ApiModels
{
    public class DegisterUserActionRequestModel
    {
        public string UserName { get; set; }

        public long UserGroupId { get; set; }

        public string Ip { get; set; }

        public List<ActionSettingModel> ActionSettings { get; set; }

    }

    public class DegisterUserActionModelRequestValidation : AbstractValidator<DegisterUserActionRequestModel>
    {
        public DegisterUserActionModelRequestValidation()
        {
            this.RuleFor(x => x.UserName).NotNull().NotEmpty();
            this.RuleFor(x => x.Ip).NotNull().NotEmpty();
            this.RuleFor(x => x.UserGroupId).GreaterThanOrEqualTo(1);
            this.RuleForEach(x => x.ActionSettings).SetValidator(new ActionSettingModelValidation());
        }
    }
}