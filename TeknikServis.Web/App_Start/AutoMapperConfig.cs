using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TeknikServis.Models.Entities;
using TeknikServis.Models.IdentityModels;
using TeknikServis.Models.ViewModels;

namespace TeknikServis.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                IssueMapping(cfg);
                RegisterUserMapping(cfg);
                ProfileUserMapping(cfg);
                ChangePasswordMapping(cfg);
                SurveyMapping(cfg);
            });
        }

        private static void SurveyMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Survey, SurveyVM>()
                .ForMember(dest=>dest.SurveyId,opt=>opt.MapFrom(x=>x.Id))
                .ReverseMap();
        }

        private static void ChangePasswordMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, ChangePasswordVM>()
                .ReverseMap();
        }

        private static void ProfileUserMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserProfileVM>()
                .ForMember(dest => dest.AvatarPath, opt => opt.MapFrom((s, d) => s.AvatarPath ?? "/assets/images/icon-noprofile.png"))
                .ReverseMap();
        }

        private static void RegisterUserMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, RegisterVM>()
                .ReverseMap();
        }

        private static void IssueMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Issue, IssueVM>()
                .ForMember(dest => dest.IssueId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap(); 
        }
    }
}