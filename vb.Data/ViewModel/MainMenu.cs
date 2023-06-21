namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MainMenu
    {
        public MainMenu()   
        {
            submenu = new List<SubMenu>();  
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainTier {get;set;}
        public int SubTier { get; set; }
        public IList<SubMenu> submenu { get; set; }          
    }

}