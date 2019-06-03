using System.ComponentModel.DataAnnotations;
using System.Web;
using System;


namespace MusicBeta1.Models
{
    public class MusicViewModel
    {
        public int ID;
        [Required]
        public string Title { get; set; }

        public string Artist { get; set; }
        public string Genre { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Music Upload")]
        public HttpPostedFileBase MusicUpload { get; set; }

        public string MusicDisplayUrl { get; set; }

        public string UniqueMusicFileNameBase { get; set; }

        [Display(Name = "Song Path")]
        public string MusicPath { get; set; }

        [Display(Name = "File Name")]
        public string OriginalFileName { get; set; }
    }
}