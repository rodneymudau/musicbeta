using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicBeta1.Models
{
    public partial class Music
    { 
     public int ID { get; set; }

    public string Title { get; set; }


    public string Artist { get; set; }
    public string Genre { get; set; }
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
    public DateTime UploadDate { get; set; }

    [Display(Name = "File Name")]
    public string OriginalFileName { get; set; }

        [Display(Name = "Mp3 File")]
    public string MusicPath { get; set; }






}

public class MusicDBContext : DbContext
{
    public DbSet<Music> Musics { get; set; }
}
}