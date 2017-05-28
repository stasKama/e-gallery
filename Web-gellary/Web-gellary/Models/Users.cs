//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_gellary.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.Answers = new HashSet<Answers>();
            this.CommentsToImages = new HashSet<CommentsToImages>();
            this.Images = new HashSet<Images>();
            this.LikesToImages = new HashSet<LikesToImages>();
            this.PicturesWaiting = new HashSet<PicturesWaiting>();
            this.Verification = new HashSet<Verification>();
            this.Avatar = "http://www.teniteatr.ru/assets/no_avatar-e557002f44d175333089815809cf49ce.png";
            this.State = "online";
            this.Permission = 10;
            this.Role = "User";
            this.CodeLanguage = "en";
        }
    
        public int Id { get; set; }
        public string UserURL { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string State { get; set; }
        public int Permission { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string CodeLanguage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answers> Answers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentsToImages> CommentsToImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Images> Images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikesToImages> LikesToImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PicturesWaiting> PicturesWaiting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Verification> Verification { get; set; }
    }
}
