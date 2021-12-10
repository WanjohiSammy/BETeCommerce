using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETeCommerce.DataLayer.Models
{
    public class DbEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool? Status { get; set; }
        public string CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DateCreated { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public bool IsDeleted { get; set; }

        public DbEntity()
        {
            DateCreated = DateTimeOffset.Now;
            Status = true;
            IsDeleted = false;
        }
    }
}
