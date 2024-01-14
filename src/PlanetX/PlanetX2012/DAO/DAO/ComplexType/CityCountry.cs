
using System;
using System.Collections;
using System.Collections.Generic;
namespace DAO.DAO
{


    public partial class CityCountry
    {
        public int CityId { get; set; }
        public string City { get; set; }
        public string CountryId { get; set; }
    }


    public partial class CityCountryID : IEquatable<CityCountryID>
    {

        public string City { get; set; }
        public string CountryId { get; set; }

        public bool Equals(CityCountryID cityObj)
        {
            return cityObj.City== this.City
                  && cityObj.CountryId == this.CountryId;
        }


        public override bool Equals(Object obj)
        {

            CityCountryID cityObj = obj as CityCountryID;

            if ((object)cityObj == null && this == null)
            {
                return true;
            }
            if ((object)cityObj == null || this == null)
            {
                return false;
            }
            else
            {
                return Equals(cityObj);
            }

        }

        public override int GetHashCode()
        {
            return (this.City + this.CountryId).GetHashCode();
        }
    }
}
