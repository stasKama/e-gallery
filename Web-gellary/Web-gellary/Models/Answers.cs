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
    
    public partial class Answers
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }
        public string Text { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Images Images { get; set; }
    }
}
