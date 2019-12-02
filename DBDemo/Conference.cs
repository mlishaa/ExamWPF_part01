using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBDemo
{
   public class Conference
    {

        public int Id { get; set; }
        public string Name { get; set; }

        private string _contactNum;
        public string ContactNum { get
            {
                return _contactNum;
            }
            set {

                if (!Regex.Match(value, @"[0-9]{3}-[0-9]{3}-[0-9]{4}").Success)
                    throw new ArgumentException("Phone number should match 000-000-0000");
                 _contactNum = value;


            } 
        }
        public DateTime ConferenceDate { get; set; }






    }
}
