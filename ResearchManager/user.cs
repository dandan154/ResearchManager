//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResearchManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public user()
        {
            this.changes = new HashSet<change>();
            this.projects = new HashSet<project>();
        }
    
        public int userID { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public int staffPosition { get; set; }
        public string hash { get; set; }
        public string salt { get; set; }
    
        public virtual ICollection<change> changes { get; set; }
        public virtual ICollection<project> projects { get; set; }
    }
}
