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
            });
        }

        private static void ChangePasswordMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, ChangePasswordVM>().ReverseMap();
        }

        private static void ProfileUserMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserProfileVM>()
                .ForMember(dest => dest.AvatarPath, opt => opt.MapFrom((s, d) => s.AvatarPath == null ? "/assets/images/icon-noprofile.png" : s.AvatarPath))
                .ReverseMap();
        }

        private static void RegisterUserMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, RegisterVM>().ReverseMap();
        }

        private static void IssueMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Issue, IssueVM>().ReverseMap();
        }
    }
}