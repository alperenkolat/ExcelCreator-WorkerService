using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelCreateWebUI.Models
{


    public enum FileStatus
    {
        Creating,
        Completed

    }
    public class UserFile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }

        public string FilePath { get; set; }
        public DateTime? CreatedTime { get; set; }

        public FileStatus FileStatus { get; set; }


        [NotMapped]//database geçmemesi için 
        public string GetCreatedDate => CreatedTime.HasValue ? CreatedTime.Value.ToShortDateString() : "-";



    }
}