﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeknikServis.Models.ViewModels
{
    public class SurveyVM
    {
        public string SurveyId { get; set; }

        [DisplayName("Genel Memnuniyet")]
        public double Satisfaction { get; set; } = 0;

        [DisplayName("Teknisyen")]
        public double TechPoint { get; set; } = 0;

        [DisplayName("Hız")]
        public double Speed { get; set; } = 0;

        [DisplayName("Fiyat")]
        public double Pricing { get; set; } = 0;

        [DisplayName("Çözüm Odaklılık")]
        public double Solving { get; set; } = 0;

        [DisplayName("Görüşleriniz")]
        [StringLength(200, ErrorMessage = "Max 200 karakter giriniz.")]
        public string Suggestions { get; set; }
    }
}
